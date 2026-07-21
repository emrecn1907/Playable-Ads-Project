using System;
using UnityEngine;
using UnityEngine.UI;

namespace PlayableAds.GameC
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TargetZone : MonoBehaviour
    {
        [SerializeField] private CarpetBody _carpet;
        [SerializeField] private CarpetSettle _settle;
        [SerializeField] private Text _hint;
        [SerializeField] private GameObject _highlight;
        [SerializeField] private float _loseBelowY = -6.5f;

        public bool Decided { get; private set; }
        public bool Won { get; private set; }

        public event Action WonEvent;
        public event Action LostEvent;

        private void Reset()
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

        public void ResetZone(Vector2 position, float width)
        {
            transform.position = new Vector3(position.x, position.y, 0f);

            var box = GetComponent<BoxCollider2D>();
            box.size = new Vector2(width, box.size.y);

            Decided = false;
            Won = false;

            if (_highlight != null)
            {
                _highlight.SetActive(true);
                var hlsr = _highlight.GetComponent<SpriteRenderer>();
                if (hlsr != null)
                {
                    hlsr.size = new Vector2(width + 1.0f, hlsr.size.y);
                }
            }
        }

        private void Update()
        {
            if (Decided || _carpet == null)
            {
                return;
            }

            
            if (_carpet.transform.position.y < _loseBelowY)
            {
                Lose();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Decided)
            {
                return;
            }

            CarpetBody body = other.GetComponent<CarpetBody>();
            if (body == null || body != _carpet)
            {
                return;
            }

            Decided = true;
            Won = true;

            if (_highlight != null)
            {
                _highlight.SetActive(false);
            }

            Vector3 rest = new Vector3(transform.position.x, transform.position.y, 0f);
            if (_settle != null)
            {
                _settle.PlaySettle(rest);
            }

            if (_hint != null)
            {
                _hint.text = "Mükemmel yerleşim!";
            }

            WonEvent?.Invoke();
        }

        private void Lose()
        {
            Decided = true;
            Won = false;

            if (_hint != null)
            {
                _hint.text = "Iskaladın! Tekrar dene";
            }

            LostEvent?.Invoke();
        }
    }
}
