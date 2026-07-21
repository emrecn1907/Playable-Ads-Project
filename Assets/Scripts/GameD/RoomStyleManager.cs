using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PlayableAds.GameD
{
    public class RoomStyleManager : MonoBehaviour
    {
        [SerializeField] private DropZone[] _zones;
        [SerializeField] private Image _dimOverlay;
        [SerializeField] private GameObject _scorePanel;
        [SerializeField] private Text _scoreText;
        [SerializeField] private Text _hint;
        [SerializeField] private PlayableAds.AudioManager _audio;
        [SerializeField] private ParticleSystem _sparkle;
        [SerializeField] private float _dimStart = 0.22f;
        [SerializeField] private float _brightenDuration = 0.9f;

        private bool _completed;

        public bool Completed => _completed;
        public event Action RoomCompleted;

        private void Start()
        {
            if (_dimOverlay != null)
            {
                _dimOverlay.color = new Color(0f, 0f, 0f, _dimStart);
            }
            if (_scorePanel != null)
            {
                _scorePanel.SetActive(false);
            }
        }

        private void Update()
        {
            if (_completed || _zones == null || _zones.Length == 0)
            {
                return;
            }

            foreach (var z in _zones)
            {
                if (z == null || !z.IsFilled)
                {
                    return;
                }
            }

            Complete();
        }

        private void Complete()
        {
            _completed = true;

            StartCoroutine(Brighten());

            if (_hint != null)
            {
                _hint.text = "Harika oda!";
            }
            if (_scorePanel != null)
            {
                _scorePanel.SetActive(true);
                StartCoroutine(PopIn(_scorePanel.transform));
            }
            if (_scoreText != null)
            {
                _scoreText.text = "Stil Puanı: 100";
            }
            if (_audio != null)
            {
                _audio.PlaySuccess();
            }
            if (_sparkle != null)
            {
                _sparkle.Play();
            }

            RoomCompleted?.Invoke();
        }

        private IEnumerator Brighten()
        {
            float a0 = _dimOverlay != null ? _dimOverlay.color.a : 0f;
            float t = 0f;
            while (t < _brightenDuration)
            {
                t += Time.deltaTime;
                float k = Mathf.Clamp01(t / _brightenDuration);
                if (_dimOverlay != null)
                {
                    _dimOverlay.color = new Color(0f, 0f, 0f, Mathf.Lerp(a0, 0f, k));
                }
                yield return null;
            }
            if (_dimOverlay != null)
            {
                _dimOverlay.color = new Color(0f, 0f, 0f, 0f);
            }
        }

        private IEnumerator PopIn(Transform tr)
        {
            float t = 0f, d = 0.35f;
            while (t < d)
            {
                t += Time.deltaTime;
                float k = Mathf.Clamp01(t / d);
                float eased = 1f - (1f - k) * (1f - k);
                tr.localScale = Vector3.LerpUnclamped(Vector3.one * 0.5f, Vector3.one, eased);
                yield return null;
            }
            tr.localScale = Vector3.one;
        }

        public void ResetRoom()
        {
            _completed = false;
            if (_dimOverlay != null)
            {
                _dimOverlay.color = new Color(0f, 0f, 0f, _dimStart);
            }
            if (_scorePanel != null)
            {
                _scorePanel.SetActive(false);
            }
        }
    }
}
