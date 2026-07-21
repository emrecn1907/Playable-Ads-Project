using System.Collections;
using UnityEngine;

namespace PlayableAds
{
    public class ScreenFader : MonoBehaviour
    {
        [SerializeField] private ScreenManager _screens;
        [SerializeField] private CanvasGroup _openingGroup;
        [SerializeField] private CanvasGroup _gameplayGroup;
        [SerializeField] private CanvasGroup _resultGroup;
        [SerializeField] private float _fadeDuration = 0.35f;

        private Coroutine _routine;

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

        private void OnScreenChanged(PlayableScreen screen)
        {
            CanvasGroup group = Resolve(screen);
            if (group == null)
            {
                return;
            }

            if (_routine != null)
            {
                StopCoroutine(_routine);
            }

            _routine = StartCoroutine(FadeIn(group));
        }

        private CanvasGroup Resolve(PlayableScreen screen)
        {
            switch (screen)
            {
                case PlayableScreen.Opening: return _openingGroup;
                case PlayableScreen.Gameplay: return _gameplayGroup;
                case PlayableScreen.Result: return _resultGroup;
                default: return null;
            }
        }

        private IEnumerator FadeIn(CanvasGroup group)
        {
            float time = 0f;
            group.alpha = 0f;

            while (time < _fadeDuration)
            {
                time += Time.deltaTime;
                group.alpha = Mathf.Clamp01(time / _fadeDuration);
                yield return null;
            }

            group.alpha = 1f;
            _routine = null;
        }
    }
}
