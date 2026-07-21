using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlayableAds.GameD
{
    public class DragDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private DropZone _zone;
        [SerializeField] private Sprite _placedSprite;
        [SerializeField] private RectTransform _dragPreview;
        [SerializeField] private Text _hint;
        [SerializeField] private float _snapRadius = 430f;
        [SerializeField] private string _defaultHint = "Parçaları odaya sürükle!";
        [SerializeField] private string _wrongHint = "Oraya olmaz, doğru yeri dene!";

        private RectTransform _canvasRect;
        private Image _previewImage;
        private bool _placed;

        public bool Placed => _placed;

        public void ResetToPalette()
        {
            _placed = false;
            transform.localScale = Vector3.one;
            gameObject.SetActive(true);
        }

        private void Awake()
        {
            if (_canvas != null)
            {
                _canvasRect = (RectTransform)_canvas.transform;
            }
            if (_dragPreview != null)
            {
                _previewImage = _dragPreview.GetComponent<Image>();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_placed || _dragPreview == null)
            {
                return;
            }

            _previewImage.sprite = _placedSprite;
            _previewImage.color = new Color(1f, 1f, 1f, 0.9f);
            _dragPreview.sizeDelta = _zone.Rect.sizeDelta;
            _dragPreview.gameObject.SetActive(true);
            MovePreview(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_placed || _dragPreview == null)
            {
                return;
            }

            MovePreview(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_placed || _dragPreview == null)
            {
                return;
            }

            _dragPreview.gameObject.SetActive(false);

            Vector2 dropLocal = ToCanvasLocal(eventData);
            Vector2 zoneLocal = _zone.Rect.anchoredPosition;

            
            if (!_zone.IsFilled && Vector2.Distance(dropLocal, zoneLocal) <= _snapRadius)
            {
                _zone.Place();
                _placed = true;
                gameObject.SetActive(false);
                SetHint(_defaultHint);
            }
            else if (_hint != null)
            {
                StartCoroutine(FlashWrongHint());
            }
        }

        private IEnumerator FlashWrongHint()
        {
            SetHint(_wrongHint);
            yield return new WaitForSeconds(1.2f);
            SetHint(_defaultHint);
        }

        private void SetHint(string text)
        {
            if (_hint != null)
            {
                _hint.text = text;
            }
        }

        private void MovePreview(PointerEventData eventData)
        {
            _dragPreview.anchoredPosition = ToCanvasLocal(eventData);
        }

        private Vector2 ToCanvasLocal(PointerEventData eventData)
        {
            Vector2 local;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvasRect, eventData.position, _canvas.worldCamera, out local);
            return local;
        }
    }
}
