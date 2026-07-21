using System;
using UnityEngine;
using UnityEngine.UI;

namespace PlayableAds
{
    public class ColorMatchGame : MonoBehaviour
    {
        [SerializeField] private PaintMixConfig _config;
        [SerializeField] private PaintMixer _mixer;
        [SerializeField] private Image _mixPreview;
        [SerializeField] private Image _targetSwatch;
        [SerializeField] private Text _matchLabel;

        public Color TargetColor { get; private set; }
        public float MatchPercent { get; private set; }

        public event Action<float> MatchChanged;

        private void OnEnable()
        {
            if (_mixer != null)
            {
                _mixer.MixChanged += OnMixChanged;
            }
        }

        private void OnDisable()
        {
            if (_mixer != null)
            {
                _mixer.MixChanged -= OnMixChanged;
            }
        }

        private void Start()
        {
            NewRound();
        }

        public void NewRound()
        {
            if (_mixer != null)
            {
                _mixer.ResetMix();
            }

            TargetColor = GenerateTarget();

            if (_targetSwatch != null)
            {
                _targetSwatch.color = TargetColor;
            }

            RefreshFromMix(_mixer != null ? _mixer.CurrentColor : Color.white);
        }

        private void OnMixChanged(Color current)
        {
            RefreshFromMix(current);
        }

        private void RefreshFromMix(Color current)
        {
            if (_mixPreview != null)
            {
                _mixPreview.color = current;
            }

            MatchPercent = ColorMath.MatchPercent(current, TargetColor);

            if (_matchLabel != null)
            {
                _matchLabel.text = "%" + Mathf.RoundToInt(MatchPercent) + " eşleşme";
            }

            MatchChanged?.Invoke(MatchPercent);
        }

        private Color GenerateTarget()
        {
            if (_config == null)
            {
                return Color.gray;
            }

            Color[] bases = _config.BasePaints;
            Color sum = Color.clear;
            int count = 0;

            for (int i = 0; i < bases.Length; i++)
            {
                int units = UnityEngine.Random.Range(0, _config.MaxUnitsPerColor + 1);
                for (int u = 0; u < units; u++)
                {
                    sum += bases[i];
                    count++;
                }
            }

            if (count == 0)
            {
                return bases[0];
            }

            return sum / count;
        }
    }
}
