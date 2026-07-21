using System.Collections;
using UnityEngine;

namespace PlayableAds.GameC
{
    public class CarpetSettle : MonoBehaviour
    {
        [SerializeField] private CarpetBody _carpet;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Sprite _openSprite;
        [SerializeField] private LandingEffects _landing;
        [SerializeField] private float _moveDuration = 0.2f;
        [SerializeField] private float _unrollDuration = 0.45f;

        public bool Settled { get; private set; }

        public void ResetSettle()
        {
            StopAllCoroutines();
            Settled = false;
        }

        public void PlaySettle(Vector3 restPosition)
        {
            if (!Settled)
            {
                StartCoroutine(Run(restPosition));
            }
        }

        private IEnumerator Run(Vector3 restPosition)
        {
            Settled = true;

            if (_carpet != null)
            {
                _carpet.FreezeInPlace();
            }

            if (_landing != null)
            {
                _landing.Play(transform.position);
            }

            
            Vector3 startPos = transform.position;
            Quaternion startRot = transform.rotation;
            float t = 0f;
            while (t < _moveDuration)
            {
                t += Time.deltaTime;
                float k = Mathf.Clamp01(t / _moveDuration);
                transform.position = Vector3.Lerp(startPos, restPosition, k);
                transform.rotation = Quaternion.Slerp(startRot, Quaternion.identity, k);
                yield return null;
            }
            transform.position = restPosition;
            transform.rotation = Quaternion.identity;

            
            if (_openSprite != null && _renderer != null)
            {
                _renderer.sprite = _openSprite;
            }

            Vector3 full = transform.localScale;
            Vector3 rolled = new Vector3(full.x * 0.35f, full.y, full.z);
            transform.localScale = rolled;

            t = 0f;
            while (t < _unrollDuration)
            {
                t += Time.deltaTime;
                float k = Mathf.Clamp01(t / _unrollDuration);
                float eased = 1f - (1f - k) * (1f - k);
                transform.localScale = Vector3.Lerp(rolled, full, eased);
                yield return null;
            }
            transform.localScale = full;
        }
    }
}
