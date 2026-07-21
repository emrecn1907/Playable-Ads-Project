using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PlayableAds.GameB
{
    [RequireComponent(typeof(Image))]
    public class PatternReveal : MonoBehaviour
    {
        [SerializeField] private DipAnimator _dipAnimator;
        [SerializeField] private DyeBucket _bucket;
        [SerializeField] private float _revealDuration = 0.9f;

        private Image _image;
        private Coroutine _routine;

        public bool IsRevealed { get; private set; }

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

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
            if (bucket != _bucket || IsRevealed)
            {
                return;
            }

            Reveal(bucket.DyeColor);
        }

        public void Reveal(Color dyeColor)
        {
            if (_routine != null)
            {
                StopCoroutine(_routine);
            }

            _routine = StartCoroutine(RevealRoutine(dyeColor));
        }

        public void ResetReveal()
        {
            if (_routine != null)
            {
                StopCoroutine(_routine);
                _routine = null;
            }

            IsRevealed = false;
            _image.fillAmount = 0f;
            Color c = _image.color;
            c.a = 0f;
            _image.color = c;
        }

        private IEnumerator RevealRoutine(Color dyeColor)
        {
            IsRevealed = true;

            float time = 0f;
            while (time < _revealDuration)
            {
                time += Time.deltaTime;
                float k = Mathf.Clamp01(time / _revealDuration);
                float eased = 1f - (1f - k) * (1f - k);
                _image.fillAmount = eased;
                _image.color = new Color(dyeColor.r, dyeColor.g, dyeColor.b, k);
                yield return null;
            }

            _image.fillAmount = 1f;
            _image.color = dyeColor;
            _routine = null;
        }
    }
}
