using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayableAds.GameC
{
    public class RopeCutter : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private PlayableAds.AudioManager _audio;
        [SerializeField] private bool _enabled = true;

        private bool _dragging;
        private Vector2 _prevWorld;

        public int CutCount { get; private set; }

        public void SetEnabled(bool value)
        {
            _enabled = value;
            _dragging = false;
        }

        private void Awake()
        {
            if (_camera == null)
            {
                _camera = Camera.main;
            }
        }

        private void Update()
        {
            if (!_enabled)
            {
                return;
            }

            Pointer pointer = Pointer.current;
            if (pointer == null)
            {
                return;
            }

            bool pressed = pointer.press.isPressed;
            Vector2 screen = pointer.position.ReadValue();

            if (pressed && !_dragging)
            {
                _dragging = true;
                _prevWorld = ToWorld(screen);
                return;
            }

            if (!pressed)
            {
                _dragging = false;
                return;
            }

            Vector2 current = ToWorld(screen);
            if (current != _prevWorld)
            {
                TrySlice(_prevWorld, current);
                _prevWorld = current;
            }
        }

        private void TrySlice(Vector2 from, Vector2 to)
        {
            RaycastHit2D hit = Physics2D.Linecast(from, to);
            if (hit.collider == null)
            {
                return;
            }

            Rope rope = hit.collider.GetComponent<Rope>();
            if (rope != null && !rope.IsCut)
            {
                rope.Cut();
                CutCount++;

                if (_audio != null)
                {
                    _audio.PlayCut();
                }
            }
        }

        private Vector2 ToWorld(Vector2 screen)
        {
            Vector3 world = _camera.ScreenToWorldPoint(new Vector3(screen.x, screen.y, 0f));
            return new Vector2(world.x, world.y);
        }
    }
}
