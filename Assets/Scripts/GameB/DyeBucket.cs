using UnityEngine;

namespace PlayableAds.GameB
{
    public class DyeBucket : MonoBehaviour
    {
        [SerializeField] private Color _dyeColor = Color.red;

        public Color DyeColor => _dyeColor;
        public RectTransform Rect => (RectTransform)transform;
    }
}
