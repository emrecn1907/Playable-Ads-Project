using UnityEngine;
using UnityEngine.UI;

namespace PlayableAds
{
    public class ResultView : MonoBehaviour
    {
        [SerializeField] private ScreenManager _screens;
        [SerializeField] private ColorMatchGame _game;
        [SerializeField] private Image _sourceCarpet;
        [SerializeField] private Image _resultCarpet;
        [SerializeField] private Text _resultTitle;

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
            if (screen != PlayableScreen.Result)
            {
                return;
            }

            if (_resultCarpet != null && _sourceCarpet != null)
            {
                _resultCarpet.color = _sourceCarpet.color;
            }

            if (_resultTitle != null && _game != null)
            {
                int pct = Mathf.RoundToInt(_game.MatchPercent);
                _resultTitle.text = "%" + pct + " — " + (pct >= 70 ? "Harika iş!" : "Güzel deneme!");
            }
        }
    }
}
