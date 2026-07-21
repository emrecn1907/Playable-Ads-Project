using UnityEngine;

namespace PlayableAds.GameB
{
    public class SplashEffect : MonoBehaviour
    {
        [SerializeField] private DipAnimator _dipAnimator;
        [SerializeField] private DyeBucket[] _buckets;
        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private AudioManager _audio;
        [SerializeField] private RectTransform _canvasRect;

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
            if (bucket != null)
            {
                MoveToBucket(bucket);
                Recolor(bucket.DyeColor);
            }

            if (_particles != null)
            {
                _particles.Play();
            }

            if (_audio != null)
            {
                _audio.PlaySplash();
            }
        }

        private void MoveToBucket(DyeBucket bucket)
        {
            if (_particles == null)
            {
                return;
            }

            Vector3 top = bucket.Rect.position;
            top += new Vector3(0f, bucket.Rect.rect.height * 0.5f * bucket.Rect.lossyScale.y, 0f);
            _particles.transform.position = new Vector3(top.x, top.y, _particles.transform.position.z);
        }

        private void Recolor(Color color)
        {
            if (_particles == null)
            {
                return;
            }

            var main = _particles.main;
            main.startColor = new ParticleSystem.MinMaxGradient(
                new Color(color.r, color.g, color.b, 1f),
                new Color(color.r, color.g, color.b, 0.6f));
        }
    }
}
