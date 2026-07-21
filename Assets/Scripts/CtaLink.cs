using UnityEngine;

namespace PlayableAds
{
    public class CtaLink : MonoBehaviour
    {
        [SerializeField] private string _url = "https://www.example.com";

        public void OpenStore()
        {
            if (!string.IsNullOrEmpty(_url))
            {
                Application.OpenURL(_url);
            }
        }
    }
}
