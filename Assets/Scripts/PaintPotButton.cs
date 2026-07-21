using UnityEngine;

namespace PlayableAds
{
    public class PaintPotButton : MonoBehaviour
    {
        [SerializeField] private PaintMixer _mixer;
        [SerializeField] private Color _paint = Color.white;

        public void Pour()
        {
            if (_mixer != null)
            {
                _mixer.AddPaint(_paint);
            }
        }
    }
}
