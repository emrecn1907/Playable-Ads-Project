using UnityEngine;
using UnityEngine.UI;

namespace PlayableAds.GameB
{
    public class StepIndicator : MonoBehaviour
    {
        [SerializeField] private Image[] _dots;
        [SerializeField] private Color _emptyColor = new Color(1f, 1f, 1f, 0.35f);

        private void Awake()
        {
            ResetSteps();
        }

        public void MarkStep(int index, Color color)
        {
            if (_dots == null || index < 0 || index >= _dots.Length)
            {
                return;
            }

            if (_dots[index] != null)
            {
                _dots[index].color = new Color(color.r, color.g, color.b, 1f);
            }
        }

        public void ResetSteps()
        {
            if (_dots == null)
            {
                return;
            }

            for (int i = 0; i < _dots.Length; i++)
            {
                if (_dots[i] != null)
                {
                    _dots[i].color = _emptyColor;
                }
            }
        }
    }
}
