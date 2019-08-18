using UnityEngine;
using UniRx.Triggers;
using UniRx;
using System.Collections.Generic;
using NaughtyAttributes;

namespace Character {

    [RequireComponent(typeof(Collider2D))]
    public class TargetDetector : MonoBehaviour
    {

        [SerializeField]
        [TagSelector]
        private List<string> m_targetTags;

        [Tooltip("If set to FALSE, this will capture ALL targets within its range upon detection." +
            "If set to TRUE, disregard the value of Range.")]
        [SerializeField]
        private bool m_isLockedToFirstSingleTarget;

        [SerializeField]
        [DisableIf("m_isLockedToFirstSingleTarget")]
        private float m_detectionRange = 5f;

        public ReactiveProperty<bool> m_isTargetDetected { get; private set; }
        public List<Collider2D> m_targets { get; private set; }

        private void Awake()
        {
            m_isTargetDetected = new ReactiveProperty<bool>(false);
            m_targets = new List<Collider2D>();
        }

        private void Start()
        {
            this.OnTriggerEnter2DAsObservable()
                .Where(otherCollider2D => IsMatchingTag(otherCollider2D.tag))
                .Subscribe(otherCollider2D => {
                    if (m_isTargetDetected.Value == false)
                    {
                        RefreshTargets(otherCollider2D);
                        m_isTargetDetected.Value = true;
                    }
                })
                .AddTo(this);

            this.OnTriggerExit2DAsObservable()
                .Where(otherCollider2D => IsMatchingTag(otherCollider2D.tag))
                .Subscribe(otherCollider2D => {
                    if (m_isTargetDetected.Value == true)
                    {
                        RefreshTargets(otherCollider2D);
                        m_isTargetDetected.Value = false;
                    }
                })
                .AddTo(this);

            this.OnCollisionEnter2DAsObservable()
                .Where(otherCollision2D => IsMatchingTag(otherCollision2D.gameObject.tag))
                .Subscribe(otherCollision2D => {
                    if (m_isTargetDetected.Value == false)
                    {
                        RefreshTargets(otherCollision2D.collider);
                        m_isTargetDetected.Value = true;
                    }
                })
                .AddTo(this);

            this.OnCollisionExit2DAsObservable()
                .Where(otherCollision2D => IsMatchingTag(otherCollision2D.gameObject.tag))
                .Subscribe(otherCollision2D => {
                    if (m_isTargetDetected.Value == true)
                    {
                        RefreshTargets(otherCollision2D.collider);
                        m_isTargetDetected.Value = false;
                    }
                })
                .AddTo(this);
        }

        private void RefreshTargets(Collider2D detectedCollider)
        {
            if (m_isLockedToFirstSingleTarget && IsMatchingTag(detectedCollider.tag))
            {
                m_targets.Clear();
                m_targets.Add(detectedCollider);
            }
            else
            {
                Collider2D[] tempTargets = Physics2D.OverlapCircleAll(transform.position, m_detectionRange);
                m_targets.Clear();
                //filter targets by tags

                foreach (Collider2D collider2D in tempTargets)
                {
                    if (IsMatchingTag(collider2D.tag))
                    {
                        m_targets.Add(collider2D);
                    }
                }
            }
        }

        private bool IsMatchingTag(string tag)
        {
            return m_targetTags.Contains(tag);
        }

    }


}