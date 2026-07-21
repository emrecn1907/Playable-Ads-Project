using UnityEngine;
using UnityEngine.UI;

namespace PlayableAds
{
    public class CarpetPainter : MonoBehaviour
    {
        [SerializeField] private PaintMixer _mixer;
        [SerializeField] private ColorMatchGame _game;
        [SerializeField] private Image _carpet;
        [SerializeField] private Text _feedback;
        [SerializeField] private Color _unpaintedTint = new Color(0.82f, 0.82f, 0.82f, 1f);

        public bool HasPainted { get; private set; }

        private void Start()
        {
            ResetCarpet();
        }

        public void Paint()
        {
            if (_carpet != null && _mixer != null)
            {
                _carpet.color = _mixer.CurrentColor;
            }

            float match = _game != null ? _game.MatchPercent : 0f;
            if (_feedback != null)
            {
                _feedback.text = "%" + Mathf.RoundToInt(match) + " — " + Evaluate(match);
            }

            HasPainted = true;
        }

        public void ResetCarpet()
        {
            if (_carpet != null)
            {
                _carpet.color = _unpaintedTint;
            }

            if (_feedback != null)
            {
                _feedback.text = "Halıyı boya!";
            }

            HasPainted = false;
        }

        private string Evaluate(float match)
        {
            if (match >= 90f) return "Harika!";
            if (match >= 70f) return "Çok iyi!";
            if (match >= 50f) return "Az daha!";
            return "Tekrar dene!";
        }
    }
}
