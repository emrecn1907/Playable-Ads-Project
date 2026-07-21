using UnityEngine;

namespace PlayableAds.GameC
{
    public class LandingEffects : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _dust;
        [SerializeField] private PlayableAds.AudioManager _audio;

        public void Play(Vector3 worldPosition)
        {
            if (_dust != null)
            {
                _dust.transform.position = worldPosition;
                _dust.Play();
            }

            if (_audio != null)
            {
                _audio.PlayThud();
            }
        }
    }
}
