using System;
using System.Collections;
using UnityEngine;

namespace PlayableAds.GameB
{
    public class DipAnimator : MonoBehaviour
    {
        [SerializeField] private RugDragger _dragger;
        [SerializeField] private float _dipDuration = 0.45f;
        [SerializeField] private float _pauseInDye = 0.35f;
        [SerializeField] private float _riseDuration = 0.6f;
        [SerializeField] private float _returnDuration = 0.3f;
        [SerializeField] private Vector2 _dipOffset = new Vector2(0f, -60f);
        [SerializeField] private Vector3 _dipScale = new Vector3(0.34f, 0.24f, 1f);

        private RectTransform _rect;
        private Coroutine _routine;

        public bool IsBusy => _routine != null;

        public event Action<DyeBucket> DipStarted;
        public event Action<DyeBucket> RugDipped;
        public event Action<DyeBucket> DipFinished;

        private void Awake()
        {
            _rect = (RectTransform)transform;
        }

        private void OnEnable()
        {
            if (_dragger != null)
            {
                _dragger.DroppedOnBucket += PlayDip;
            }
        }

        private void OnDisable()
        {
            if (_dragger != null)
            {
                _dragger.DroppedOnBucket -= PlayDip;
            }
        }

        public void PlayDip(DyeBucket bucket)
        {
            if (_routine != null || bucket == null)
            {
                return;
            }

            _routine = StartCoroutine(DipRoutine(bucket));
        }

        public void ReturnHome()
        {
            if (_routine != null)
            {
                return;
            }

            _routine = StartCoroutine(ReturnRoutine());
        }

        private IEnumerator DipRoutine(DyeBucket bucket)
        {
            if (_dragger != null)
            {
                _dragger.InputLocked = true;
            }

            DipStarted?.Invoke(bucket);

            Vector2 dipPosition = ToParentLocal(bucket.Rect) + _dipOffset;
            yield return Animate(dipPosition, _dipScale, _dipDuration, easeIn: true);

            RugDipped?.Invoke(bucket);
            yield return new WaitForSeconds(_pauseInDye);

            yield return Animate(_dragger != null ? _dragger.HomePosition : _rect.anchoredPosition, Vector3.one, _riseDuration, easeIn: false);

            if (_dragger != null)
            {
                _dragger.InputLocked = false;
            }

            _routine = null;
            DipFinished?.Invoke(bucket);
        }

        private IEnumerator ReturnRoutine()
        {
            if (_dragger != null)
            {
                _dragger.InputLocked = true;
            }

            yield return Animate(_dragger != null ? _dragger.HomePosition : _rect.anchoredPosition, Vector3.one, _returnDuration, easeIn: false);

            if (_dragger != null)
            {
                _dragger.InputLocked = false;
            }

            _routine = null;
        }

        private IEnumerator Animate(Vector2 targetPosition, Vector3 targetScale, float duration, bool easeIn)
        {
            Vector2 startPosition = _rect.anchoredPosition;
            Vector3 startScale = _rect.localScale;
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                float k = Mathf.Clamp01(time / duration);
                float eased = easeIn ? k * k : 1f - (1f - k) * (1f - k);
                _rect.anchoredPosition = Vector2.LerpUnclamped(startPosition, targetPosition, eased);
                _rect.localScale = Vector3.LerpUnclamped(startScale, targetScale, eased);
                yield return null;
            }

            _rect.anchoredPosition = targetPosition;
            _rect.localScale = targetScale;
        }

        private Vector2 ToParentLocal(RectTransform target)
        {
            RectTransform parent = (RectTransform)_rect.parent;
            return parent.InverseTransformPoint(target.position);
        }
    }
}
