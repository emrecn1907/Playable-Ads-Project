using UnityEngine;

namespace PlayableAds.GameC
{
    public class Rope : MonoBehaviour
    {
        [SerializeField] private HingeJoint2D _topJoint;
        [SerializeField] private Rigidbody2D _body;

        public bool IsCut { get; private set; }

        public Rigidbody2D Body
        {
            get
            {
                if (_body == null)
                {
                    _body = GetComponent<Rigidbody2D>();
                }
                return _body;
            }
        }

        public void Cut()
        {
            if (IsCut)
            {
                return;
            }

            IsCut = true;

            if (_topJoint == null)
            {
                _topJoint = GetComponent<HingeJoint2D>();
            }

            if (_topJoint != null)
            {
                Destroy(_topJoint);
            }
        }
    }
}
