using UnityEngine;

namespace PlayableAds
{
    public static class ColorMath
    {
        private static readonly float MaxDistance = Mathf.Sqrt(3f);

        public static float MatchPercent(Color a, Color b)
        {
            float dr = a.r - b.r;
            float dg = a.g - b.g;
            float db = a.b - b.b;
            float distance = Mathf.Sqrt(dr * dr + dg * dg + db * db);
            return Mathf.Clamp01(1f - distance / MaxDistance) * 100f;
        }
    }
}
