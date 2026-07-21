using UnityEngine;

namespace PlayableAds.GameC
{
    [System.Serializable]
    public class RopeLevel
    {
        public string name = "Bölüm";
        public float carpetY = 2.0f;
        public float[] ropeXs = { -1.4f, 1.4f };
        public Vector2 targetPosition = new Vector2(0f, -3.2f);
        public float targetWidth = 2.8f;
        [TextArea] public string hint = "İpleri kes, halı insin!";
    }
}
