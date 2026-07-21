using System.Collections;
using UnityEngine;

namespace PlayableAds.GameB
{
    public class DipResultView : MonoBehaviour
    {
        [SerializeField] private DipDyeGame _game;
        [SerializeField] private ScreenManager _screens;
        [SerializeField] private float _delayBeforeResult = 1.1f;

        private bool _pending;

        private void OnEnable()
        {
            if (_game != null)
            {
                _game.DesignCompleted += OnDesignCompleted;
            }
        }

        private void OnDisable()
        {
            if (_game != null)
            {
                _game.DesignCompleted -= OnDesignCompleted;
            }
        }

        private void OnDesignCompleted()
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
