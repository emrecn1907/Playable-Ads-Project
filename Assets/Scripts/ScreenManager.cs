using System;
using UnityEngine;

namespace PlayableAds
{
    public enum PlayableScreen
    {
        Opening,
        Gameplay,
        Result
    }

    public class ScreenManager : MonoBehaviour
    {
        [SerializeField] private GameObject _openingScreen;
        [SerializeField] private GameObject _gameplayScreen;
        [SerializeField] private GameObject _resultScreen;
        [SerializeField] private PlayableScreen _initialScreen = PlayableScreen.Opening;

        public event Action<PlayableScreen> ScreenChanged;

        public PlayableScreen Current { get; private set; }

        private void Start()
        {
            Show(_initialScreen);
        }

        public void Show(PlayableScreen screen)
        {
            if (_openingScreen != null)
            {
                _openingScreen.SetActive(screen == PlayableScreen.Opening);
            }

            if (_gameplayScreen != null)
            {
                _gameplayScreen.SetActive(screen == PlayableScreen.Gameplay);
            }

            if (_resultScreen != null)
            {
                _resultScreen.SetActive(screen == PlayableScreen.Result);
            }

            Current = screen;
            ScreenChanged?.Invoke(screen);
        }

        public void ShowOpening()
        {
            Show(PlayableScreen.Opening);
        }

        public void ShowGameplay()
        {
            Show(PlayableScreen.Gameplay);
        }

        public void ShowResult()
        {
            Show(PlayableScreen.Result);
        }
    }
}
