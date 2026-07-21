using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayableAds.GameB
{
    public class DipDyeGame : MonoBehaviour
    {
        [SerializeField] private DipAnimator _dipAnimator;
        [SerializeField] private DyeBucket[] _buckets;
        [SerializeField] private PatternReveal[] _patterns;
        [SerializeField] private StepIndicator _indicator;
        [SerializeField] private Text _hint;
        [SerializeField] private int _requiredColors = 3;

        private readonly List<DyeBucket> _dipped = new List<DyeBucket>();

        public bool IsComplete => _dipped.Count >= _requiredColors;

        public event Action DesignCompleted;

        private void OnEnable()
        {
            if (_dipAnimator != null)
            {
                _dipAnimator.RugDipped += OnRugDipped;
            }
        }

        private void OnDisable()
        {
            if (_dipAnimator != null)
            {
                _dipAnimator.RugDipped -= OnRugDipped;
            }
        }

        private void OnRugDipped(DyeBucket bucket)
        {
            BringPatternsToFront(bucket);

            if (_dipped.Contains(bucket))
            {
                return;
            }

            _dipped.Add(bucket);

            if (_indicator != null)
            {
                _indicator.MarkStep(_dipped.Count - 1, bucket.DyeColor);
            }

            UpdateHint();

            if (IsComplete)
            {
                DesignCompleted?.Invoke();
            }
        }

        private void BringPatternsToFront(DyeBucket bucket)
        {
            for (int i = 0; i < _buckets.Length; i++)
            {
                if (_buckets[i] == bucket && _patterns[i] != null)
                {
                    _patterns[i].transform.SetAsLastSibling();
                }
            }
        }

        private void UpdateHint()
        {
            if (_hint == null)
            {
                return;
            }

            int left = _requiredColors - _dipped.Count;
            _hint.text = left > 0 ? "Harika! " + left + " renk kaldı" : "Tasarımın tamamlandı!";
        }

        public void ResetGame()
        {
            _dipped.Clear();

            if (_indicator != null)
            {
                _indicator.ResetSteps();
            }

            if (_hint != null)
            {
                _hint.text = "Halıyı boyaya daldır!";
            }

            for (int i = 0; i < _patterns.Length; i++)
            {
                if (_patterns[i] != null)
                {
                    _patterns[i].ResetReveal();
                }
            }
        }
    }
}
