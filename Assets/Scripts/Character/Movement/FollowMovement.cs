using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils;

namespace Character.Movement {

    public class FollowMovement : BaseMovement
    {

        [Header("----- Child variables -----")]

        [SerializeField]
        [Range(0.1f, 10f)]
        private float m_followDistance;

        [SerializeField]
        [Tooltip("The distance to ignore Follow Target when it gets too far away. " +
            "Value is added to Follow Distance for calculations.")]
        [Range(1f, 20f)]
        private float m_followIgnoreDistance = 1f;

        [SerializeField]
        private bool m_shouldFollowUntilDeath;

        [SerializeField]
        private Transform m_followTarget;

        private float m_actualFollowIgnoreDistance;

        protected override void Awake()
        {
            base.Awake();

            m_actualFollowIgnoreDistance= (m_followDistance + m_followIgnoreDistance);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            this.FixedUpdateAsObservable()
                .Where(_ => m_reactiveIsMovEnabled.Value && (m_followTarget != null))
                .Select(_ => Vector2.Distance(m_followTarget.position, gameObject.transform.position))
                .Subscribe(distance =>
                {
                    if (distance > m_actualFollowIgnoreDistance)
                    {
                        m_followTarget = null;
                        m_compAnimator.SetBool(m_animMove, false);
                    }
                    else if ((distance <= m_actualFollowIgnoreDistance) && (distance > m_followDistance))
                    {
                        //reference for getting the normalized Vector direction from 2 positions:
                        //https://docs.unity3d.com/Manual/DirectionDistanceFromOneObjectToAnother.html
                        StartMovement((m_followTarget.position - gameObject.transform.position).normalized);
                        m_compSpriteRenderer.flipX = ShouldFlip(m_followTarget.transform);
                    }
                    else
                    {
                        m_compAnimator.SetBool(m_animMove, false);
                    }
                    
                })
                .AddTo(m_disposables);

        }

        public void SetFollowTarget(Transform newTarget) {
            if ((m_shouldFollowUntilDeath && (m_followTarget == null) || !m_shouldFollowUntilDeath)) {
                m_followTarget = newTarget;
                return;
            }

            LogUtil.PrintInfo(gameObject, GetType(), "SetFollowTarget(): " +
                "Still following current target. Not renewing target.");
        }

    }

}