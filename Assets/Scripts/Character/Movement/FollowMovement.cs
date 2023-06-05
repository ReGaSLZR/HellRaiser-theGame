using Character.Ground;
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

        [Header("Jump")]
        [SerializeField]
        private bool m_canJump;

        [SerializeField]
        private float m_jumpHeight = 5f;

        [SerializeField]
        private float m_jumpVelocity = 1.2f;

        [SerializeField]
        private int m_jumpTimes = 1;

        private int m_jumpsLeft = 1;

        [SerializeField]
        private GroundManager m_ground;

        private float m_actualFollowIgnoreDistance;

        protected override void Awake()
        {
            base.Awake();

            m_actualFollowIgnoreDistance= (m_followDistance + m_followIgnoreDistance);
        }

        private void Start()
        {
            m_ground.IsOnGround().Subscribe(isOnGround =>
            {
                if (isOnGround)
                {
                    m_jumpsLeft = m_jumpTimes;
                }
            }).AddTo(this);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            this.UpdateAsObservable()
                .Where(_ => m_reactiveIsMovEnabled.Value && (m_followTarget != null))
                .Select(_ => Vector2.Distance(m_followTarget.position, gameObject.transform.position))
                .Subscribe(distance =>
                {
                    if (distance > m_actualFollowIgnoreDistance)
                    {
                        m_followTarget = null;
                    }
                    else if ((distance <= m_actualFollowIgnoreDistance) && (distance > m_followDistance))
                    {
                        //reference for getting the normalized Vector direction from 2 positions:
                        //https://docs.unity3d.com/Manual/DirectionDistanceFromOneObjectToAnother.html
                        StartMovement((m_followTarget.position - gameObject.transform.position).normalized);
                        m_compSpriteRenderer.flipX = ShouldFlip(m_followTarget.transform);

                        CheckJump();
                    }
                    else
                    {
                        m_compAnimator.SetBool(m_animMove, false);
                    }
                    
                })
                .AddTo(m_disposables);

            this.UpdateAsObservable()
                .Where(_ => m_followTarget == null && m_compAnimator.GetBool(m_animMove))
                .Subscribe(_ => m_compAnimator.SetBool(m_animMove, false))
                .AddTo(m_disposables);

        }

        private void CheckJump()
        {
            if (!m_canJump)
            {
                return;
            }

            var yDistance = m_followTarget.position.y - gameObject.transform.position.y;

            if (m_jumpHeight < yDistance || yDistance < 0)
            {
                return;
            }

            if (m_ground == null)
            {
                return;
            }

            if (m_ground.IsOnGround().Value && m_jumpsLeft > 0)
            {
                Jump();
            }
        }

        [ContextMenu("Jump")]
        private void Jump()
        {
            m_compRigidBody2D.velocity = m_jumpHeight * m_jumpVelocity * Vector2.up;

            m_jumpsLeft--;
        }

        public void SetFollowTarget(Transform newTarget) {
            if ((m_shouldFollowUntilDeath && (m_followTarget == null)) || !m_shouldFollowUntilDeath) {
                m_followTarget = newTarget;
                return;
            }

            LogUtil.PrintInfo(gameObject, GetType(), "SetFollowTarget(): " +
                "Still following current target. Not renewing target.");
        }

    }

}