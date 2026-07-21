using UnityEngine;

namespace PlayableAds
{
    [CreateAssetMenu(fileName = "PaintMixConfig", menuName = "Playable/Paint Mix Config")]
    public class PaintMixConfig : ScriptableObject
    {
        [SerializeField] private Color _red = new Color(0.86f, 0.22f, 0.18f, 1f);
        [SerializeField] private Color _yellow = new Color(0.95f, 0.76f, 0.18f, 1f);
        [SerializeField] private Color _blue = new Color(0.20f, 0.42f, 0.80f, 1f);
        [SerializeField] private Color _emptyMix = Color.white;
        [SerializeField] private int _maxUnitsPerColor = 3;

        public Color Red => _red;
        public Color Yellow => _yellow;
        public Color Blue => _blue;
        public Color EmptyMix => _emptyMix;
        public int MaxUnitsPerColor => _maxUnitsPerColor;

        public Color[] BasePaints => new[] { _red, _yellow, _blue };
    }
}
