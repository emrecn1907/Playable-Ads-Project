using System.Collections;
using UnityEngine;

namespace PlayableAds.GameC
{
    public class GameCResultView : MonoBehaviour
    {
        [SerializeField] private LevelManager _levels;
        [SerializeField] private ScreenManager _screens;
        [SerializeField] private float _delayBeforeResult = 1.7f;

        private bool _pending;

        private void OnEnable()
        {
            if (_levels != null)
            {
                _levels.AllLevelsComplete += OnComplete;
            }
        }

        private void OnDisable()
        {
            if (_levels != null)
            {
                _levels.AllLevelsComplete -= OnComplete;
            }
        }

        
        public void Rearm()
        {
            _pending = false;
        }

        private void OnComplete()
        {
            if (_pending)
            {
                return;
            }

            _pending = true;
            StartCoroutine(ShowAfterDelay());
        }

        private IEnumerator ShowAfterDelay()
        {
            yield return new WaitForSeconds(_delayBeforeResult);

            if (_screens != null)
            {
                _screens.ShowResult();
            }

            _pending = false;
        }
    }
}
