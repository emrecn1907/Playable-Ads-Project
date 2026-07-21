using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlayableAds.GameB
{
    public class RugDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private DipAnimator _dipAnimator;
        [SerializeField] private DyeBucket[] _buckets;
        [SerializeField] private float _catchDistance = 300f;

        private RectTransform _rect;
        private Vector2 _homePosition;
        private bool _dragging;

        public bool InputLocked { get; set; }
        public Vector2 HomePosition => _homePosition;

        public event Action<DyeBucket> DroppedOnBucket;

        private void Awake()
        {
            _rect = (RectTransform)transform;
            _homePosition = _rect.anchoredPosition;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (InputLocked)
            {
                return;
            }

            _dragging = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_dragging || InputLocked)
            {
                return;
            }

            float scale = _canvas != null ? _canvas.scaleFactor : 1f;
            _rect.anchoredPosition += eventData.delta / scale;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_dragging)
            {
                return;
            }

            _dragging = false;

            if (InputLocked)
            {
                return;
            }

            DyeBucket target = FindNearbyBucket();
            if (target != null)
            {
                DroppedOnBucket?.Invoke(target);
            }
            else if (_dipAnimator != null)
            {
                _dipAnimator.ReturnHome();
            }
        }

        private DyeBucket FindNearbyBucket()
        {
            if (_buckets == null || _canvas == null)
            {
                return null;
            }

            RectTransform canvasRect = (RectTransform)_canvas.transform;
            Vector2 rugCenter = canvasRect.InverseTransformPoint(_rect.position);
            Vector2 rugBottom = rugCenter + new Vector2(0f, -_rect.rect.height * 0.5f * _rect.localScale.y);

            DyeBucket best = null;
            float bestDistance = _catchDistance;

            for (int i = 0; i < _buckets.Length; i++)
            {
                if (_buckets[i] == null)
                {
                    continue;
                }

                Vector2 bucketCenter = canvasRect.InverseTransformPoint(_buckets[i].Rect.position);
                float distance = Vector2.Distance(rugBottom, bucketCenter);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    best = _buckets[i];
                }
            }

            return best;
        }
    }
}
