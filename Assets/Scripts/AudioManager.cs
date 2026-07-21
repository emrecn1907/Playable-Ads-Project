using UnityEngine;
using UnityEngine.UI;

namespace PlayableAds
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private ScreenManager _screens;
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioClip _click;
        [SerializeField] private AudioClip _paint;
        [SerializeField] private AudioClip _success;
        [SerializeField] private AudioClip _drop;
        [SerializeField] private AudioClip _splash;
        [SerializeField] private AudioClip _cut;
        [SerializeField] private AudioClip _thud;
        [SerializeField] private Image _muteIcon;
        [SerializeField] private Sprite _onSprite;
        [SerializeField] private Sprite _offSprite;

        private bool _muted;

        private void OnEnable()
        {
            if (_screens != null)
            {
                _screens.ScreenChanged += OnScreenChanged;
            }
        }

        private void OnDisable()
        {
            if (_screens != null)
            {
                _screens.ScreenChanged -= OnScreenChanged;
            }
        }

        private void Start()
        {
            if (_musicSource != null && _musicSource.clip != null)
            {
                _musicSource.Play();
            }

            UpdateIcon();
        }

        private void OnScreenChanged(PlayableScreen screen)
        {
            if (screen == PlayableScreen.Result)
            {
                PlaySuccess();
            }
        }

        public void PlayClick()
        {
            PlayOneShot(_click);
        }

        public void PlayPaint()
        {
            PlayOneShot(_paint);
        }

        public void PlaySuccess()
        {
            PlayOneShot(_success);
        }

        public void PlayDrop()
        {
            PlayOneShot(_drop);
        }

        public void PlaySplash()
        {
            PlayOneShot(_splash);
        }

        public void PlayCut()
        {
            PlayOneShot(_cut);
        }

        public void PlayThud()
        {
            PlayOneShot(_thud);
        }

        public void ToggleMute()
        {
            _muted = !_muted;
            AudioListener.volume = _muted ? 0f : 1f;
            UpdateIcon();
        }

        private void PlayOneShot(AudioClip clip)
        {
            if (_sfxSource != null && clip != null)
            {
                _sfxSource.PlayOneShot(clip);
            }
        }

        private void UpdateIcon()
        {
            if (_muteIcon != null && _onSprite != null && _offSprite != null)
            {
                _muteIcon.sprite = _muted ? _offSprite : _onSprite;
            }
        }
    }
}
