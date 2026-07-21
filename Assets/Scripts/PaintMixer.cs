using System;
using UnityEngine;

namespace PlayableAds
{
    public class PaintMixer : MonoBehaviour
    {
        [SerializeField] private Color _emptyColor = Color.white;

        private Color _sum;
        private int _count;

        public event Action<Color> MixChanged;

        public Color CurrentColor => _count == 0 ? _emptyColor : _sum / _count;
        public int DropCount => _count;

        public void AddPaint(Color paint)
        {
            _sum += paint;
            _count++;
            MixChanged?.Invoke(CurrentColor);
        }

        public void ResetMix()
        {
            _sum = Color.clear;
            _count = 0;
            MixChanged?.Invoke(CurrentColor);
        }
    }
}
