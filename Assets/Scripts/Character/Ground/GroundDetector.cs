using NaughtyAttributes;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Character.Ground {

    [RequireComponent(typeof(Collider2D))]
    public class GroundDetector : MonoBehaviour
    {
       
        [SerializeField]
        [Required]
        private GroundManager m_groundManager;

        [SerializeField]
        private GroundType m_groundType;

        [SerializeField]
        private bool m_bypassTagChecking;

        [SerializeField]
        [Tag]
        [DisableIf("m_bypassTagChecking")]
        private string m_groundTag1;

        [SerializeField]
        [Tag]
        [DisableIf("m_bypassTagChecking")]
        private string m_groundTag2;

        private void Awake()
        {
            if (m_groundTag2 == null || m_groundTag2.Equals(""))
            {
                m_groundTag2 = "UNSET";
            }
        }

        private void Start()
        {
            this.OnTriggerEnter2DAsObservable()
                .Where(otherCollider2D => IsGroundTagMet(otherCollider2D))
                .Subscribe(_ => m_groundManager.SetGround(this, m_groundType, true))
                .AddTo(this);

            this.OnTriggerExit2DAsObservable()
                .Where(otherCollider2D => IsGroundTagMet(otherCollider2D))
                .Subscribe(_ => m_groundManager.SetGround(this, m_groundType, false))
                .AddTo(this);
        }

        private bool IsGroundTagMet(Collider2D otherCollider2D)
        {
            return m_bypassTagChecking ? true 
                : (m_groundTag1.Equals(otherCollider2D.tag) || m_groundTag2.Equals(otherCollider2D.tag));
        }

    }

}