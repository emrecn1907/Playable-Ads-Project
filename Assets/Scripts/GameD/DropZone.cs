using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PlayableAds.GameD
{
    public class DropZone : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _ghostSprite;
        [SerializeField] private Color _ghostColor = new Color(1f, 1f, 1f, 0.30f);
        [SerializeField] private Sprite[] _variants;     
        [SerializeField] private float _alphaHitThreshold = 0.5f;
        [SerializeField] private PlayableAds.AudioManager _audio;

        private int _variantIndex;

        public bool IsFilled { get; private set; }
        public int VariantIndex => _variantIndex;
        public RectTransform Rect => (RectTransform)transform;

        private void Awake()
        {
            if (_image == null)
            {
                _image = GetComponent<Image>();
            }
            if (_ghostSprite == null && _image != null)
            {
                _ghostSprite = _image.sprite;
            }
        }

        public void Place()
        {
            _variantIndex = 0;
            if (_image != null)
            {
                if (_variants != null && _variants.Length > 0)
                {
                    _image.sprite = _variants[0];
                }
                _image.color = Color.white;
                _image.raycastTarget = true;
                if (_alphaHitThreshold > 0f)
                {
                    _image.alphaHitTestMinimumThreshold = _alphaHitThreshold;
                }
            }
            IsFilled = true;
        }

        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!IsFilled || _variants == null || _variants.Length < 2)
            {
                return;
            }

            _variantIndex = (_variantIndex + 1) % _variants.Length;
            _image.sprite = _variants[_variantIndex];

            if (_audio != null)
            {
                _audio.PlayClick();
            }
        }

        public void ClearZone()
        {
            if (_image != null)
            {
                _image.sprite = _ghostSprite;
                _image.color = _ghostColor;
                _image.raycastTarget = false;
            }
            IsFilled = false;
            _variantIndex = 0;
        }
    }
}
