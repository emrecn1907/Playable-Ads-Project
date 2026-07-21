using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PlayableAds.GameD
{
    
    public class GameDResultView : MonoBehaviour
    {
        [SerializeField] private RoomStyleManager _room;
        [SerializeField] private GameObject _resultOverlay;
        [SerializeField] private GameObject _palette;
        [SerializeField] private DragDropItem[] _items;
        [SerializeField] private DropZone[] _zones;
        [SerializeField] private Text _hint;
        [SerializeField] private float _delayBeforeResult = 1.6f;
        [SerializeField] private string _defaultHint = "Parçaları odaya sürükle!";

        private bool _shown;

        private void OnEnable()
        {
            if (_room != null)
            {
                _room.RoomCompleted += OnRoomCompleted;
            }
        }

        private void OnDisable()
        {
            if (_room != null)
            {
                _room.RoomCompleted -= OnRoomCompleted;
            }
        }

        private void Start()
        {
            if (_resultOverlay != null)
            {
                _resultOverlay.SetActive(false);
            }
        }

        private void OnRoomCompleted()
        {
            if (_shown)
            {
                return;
            }

            _shown = true;
            StartCoroutine(ShowAfterDelay());
        }

        private IEnumerator ShowAfterDelay()
        {
            yield return new WaitForSeconds(_delayBeforeResult);

            if (_palette != null)
            {
                _palette.SetActive(false);
            }
            if (_resultOverlay != null)
            {
                _resultOverlay.SetActive(true);
                StartCoroutine(PopIn(_resultOverlay.transform));
            }
        }

        
        public void RetryGame()
        {
            _shown = false;

            if (_resultOverlay != null)
            {
                _resultOverlay.SetActive(false);
            }
            if (_palette != null)
            {
                _palette.SetActive(true);
            }
            if (_zones != null)
            {
                foreach (var z in _zones)
                {
                    if (z != null) z.ClearZone();
                }
            }
            if (_items != null)
            {
                foreach (var it in _items)
                {
                    if (it != null) it.ResetToPalette();
                }
            }
            if (_room != null)
            {
                _room.ResetRoom();
            }
            if (_hint != null)
            {
                _hint.text = _defaultHint;
            }
        }

        private IEnumerator PopIn(Transform tr)
        {
            float t = 0f, d = 0.3f;
            while (t < d)
            {
                t += Time.deltaTime;
                float k = Mathf.Clamp01(t / d);
                float eased = 1f - (1f - k) * (1f - k);
                tr.localScale = Vector3.LerpUnclamped(Vector3.one * 0.6f, Vector3.one, eased);
                yield return null;
            }
            tr.localScale = Vector3.one;
        }
    }
}
