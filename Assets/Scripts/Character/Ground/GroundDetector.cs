using NaughtyAttributes;
using System.Collections.Generic;
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
        [TagSelector]
        [DisableIf("m_bypassTagChecking")]
        private List<string> m_groundTags;

        private void Start()
        {
            this.OnTriggerEnter2DAsObservable()
                .Where(otherCollider2D => IsGroundTagMet(otherCollider2D))
                .Subscribe(_ => m_groundManager.SetDetectedGround(this, m_groundType, true))
                .AddTo(this);

            this.OnTriggerExit2DAsObservable()
                .Where(otherCollider2D => IsGroundTagMet(otherCollider2D))
                .Subscribe(_ => m_groundManager.SetDetectedGround(this, m_groundType, false))
                .AddTo(this);
        }

        private bool IsGroundTagMet(Collider2D otherCollider2D)
        {
            return m_bypassTagChecking ? true : m_groundTags.Contains(otherCollider2D.tag);
        }

    }

}