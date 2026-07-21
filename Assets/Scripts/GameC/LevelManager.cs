using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PlayableAds.GameC
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private CarpetBody _carpet;
        [SerializeField] private SpriteRenderer _carpetRenderer;
        [SerializeField] private CarpetSettle _settle;
        [SerializeField] private TargetZone _zone;
        [SerializeField] private RopeCutter _cutter;
        [SerializeField] private Text _hint;
        [SerializeField] private Sprite _ropeSprite;
        [SerializeField] private Sprite _rolledSprite;
        [SerializeField] private float _transitionDelay = 1.4f;

        [SerializeField]
        private RopeLevel[] _levels =
        {
            new RopeLevel { name = "Bölüm 1", carpetY = 2.0f, ropeXs = new[] { -1.4f, 1.4f }, targetPosition = new Vector2(0f, -3.2f), targetWidth = 2.8f, hint = "İpleri kes, halı insin!" },
            new RopeLevel { name = "Bölüm 2", carpetY = 2.0f, ropeXs = new[] { -1.4f, 1.4f }, targetPosition = new Vector2(0f, -3.2f), targetWidth = 2.0f, hint = "Bölüm 2 - tam ortaya indir!" },
            new RopeLevel { name = "Bölüm 3", carpetY = 2.2f, ropeXs = new[] { -1.6f, 0f, 1.6f }, targetPosition = new Vector2(0f, -3.3f), targetWidth = 1.6f, hint = "Bölüm 3 - üç ipi de kes!" },
        };

        public int CurrentIndex { get; private set; }
        public int LevelCount => _levels.Length;
        public bool AllCompleted { get; private set; }

        public event Action AllLevelsComplete;

        private void OnEnable()
        {
            if (_zone != null)
            {
                _zone.WonEvent += OnWon;
                _zone.LostEvent += OnLost;
            }
        }

        private void OnDisable()
        {
            if (_zone != null)
            {
                _zone.WonEvent -= OnWon;
                _zone.LostEvent -= OnLost;
            }
        }

        private void Start()
        {
            LoadLevel(0);
        }

        public void LoadLevel(int index)
        {
            CurrentIndex = Mathf.Clamp(index, 0, _levels.Length - 1);
            RopeLevel level = _levels[CurrentIndex];

            Rigidbody2D body = _carpet.GetComponent<Rigidbody2D>();

            
            body.bodyType = RigidbodyType2D.Kinematic;
            body.linearVelocity = Vector2.zero;
            body.angularVelocity = 0f;

            
            foreach (Rope rope in UnityEngine.Object.FindObjectsByType<Rope>(FindObjectsSortMode.None))
            {
                Destroy(rope.gameObject);
            }
            foreach (HingeJoint2D joint in _carpet.GetComponents<HingeJoint2D>())
            {
                Destroy(joint);
            }

            
            _carpet.transform.position = new Vector3(0f, level.carpetY, 0f);
            _carpet.transform.rotation = Quaternion.identity;
            _carpet.transform.localScale = Vector3.one;
            if (_carpetRenderer != null && _rolledSprite != null)
            {
                _carpetRenderer.sprite = _rolledSprite;
            }

            float carpetTop = level.carpetY + 0.9f;   

            
            for (int i = 0; i < level.ropeXs.Length; i++)
            {
                CreateRope(level.ropeXs[i], carpetTop);
            }

            body.bodyType = RigidbodyType2D.Dynamic;

            
            if (_settle != null)
            {
                _settle.ResetSettle();
            }
            if (_zone != null)
            {
                _zone.ResetZone(level.targetPosition, level.targetWidth);
            }
            if (_cutter != null)
            {
                _cutter.SetEnabled(true);
            }
            if (_hint != null)
            {
                _hint.text = level.hint;
            }
        }

        private void CreateRope(float x, float carpetTop)
        {
            var go = new GameObject("Rope_" + x, typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(Rope));
            var sr = go.GetComponent<SpriteRenderer>();
            sr.sprite = _ropeSprite;
            sr.sortingOrder = 0;

            float halfH = sr.sprite.bounds.size.y * 0.5f;   // ~0.71
            go.transform.position = new Vector3(x, carpetTop + halfH, 0f);

            var rb = go.GetComponent<Rigidbody2D>();
            rb.mass = 0.3f;
            rb.gravityScale = 1f;
            rb.linearDamping = 0.6f;
            rb.angularDamping = 0.9f;

            var box = go.GetComponent<BoxCollider2D>();
            box.size = sr.sprite.bounds.size;
            box.offset = sr.sprite.bounds.center;

            var top = go.AddComponent<HingeJoint2D>();
            top.autoConfigureConnectedAnchor = true;
            top.connectedBody = null;
            top.anchor = new Vector2(0f, halfH);

            
            var carpetHinge = _carpet.gameObject.AddComponent<HingeJoint2D>();
            carpetHinge.autoConfigureConnectedAnchor = true;
            carpetHinge.connectedBody = rb;
            carpetHinge.anchor = new Vector2(x, 0.9f);
            carpetHinge.enableCollision = false;
        }

        private void OnWon()
        {
            if (_cutter != null)
            {
                _cutter.SetEnabled(false);
            }

            if (CurrentIndex + 1 < _levels.Length)
            {
                StartCoroutine(NextLevelAfterDelay());
            }
            else
            {
                AllCompleted = true;
                AllLevelsComplete?.Invoke();
            }
        }

        private void OnLost()
        {
            if (_cutter != null)
            {
                _cutter.SetEnabled(false);
            }
            StartCoroutine(RetryAfterDelay());
        }

        private IEnumerator NextLevelAfterDelay()
        {
            yield return new WaitForSeconds(_transitionDelay);
            LoadLevel(CurrentIndex + 1);
        }

        private IEnumerator RetryAfterDelay()
        {
            yield return new WaitForSeconds(_transitionDelay);
            LoadLevel(CurrentIndex);
        }
    }
}
