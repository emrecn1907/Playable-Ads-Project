using UnityEngine;

namespace PlayableAds.GameC
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CarpetBody : MonoBehaviour
    {
        [SerializeField] private float _restVelocity = 0.15f;

        public Rigidbody2D Body { get; private set; }

        public bool IsMoving => Body != null && Body.linearVelocity.sqrMagnitude > _restVelocity * _restVelocity;

        private void Awake()
        {
            Body = GetComponent<Rigidbody2D>();
        }

        public void FreezeInPlace()
        {
            if (Body == null)
            {
                return;
            }

            Body.linearVelocity = Vector2.zero;
            Body.angularVelocity = 0f;
            Body.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}
