using Character.Ground;
using GamePlayInput;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Character.Movement
{

    public class PlayerMovement : BaseMovement
    {

        //INJECTIBLES
        [Inject]
        private BaseInputModel m_modelInput;

        [Header("Wall Slide")]
        [SerializeField]
        private float m_wallSlideNormalFrictionDrag = 1f;
        [SerializeField]
        private float m_wallSlideFriction = 10f;

        [Header("Jump")]
        [SerializeField]
        private float m_jumpVelocity = 1.2f;
        [SerializeField]
        [Range(1.1f, 5f)]
        private float m_jumpFallMultiplier = 1.5f;
        [SerializeField]
        private float m_jumpHeight = 5f;

        [Space]

        [SerializeField]
        private int m_jumpTimesMax = 1;
        [SerializeField]
        private float m_jumpsInterval = 0.5f;
        [SerializeField]
        private bool m_canJumpInMidair;
        private int m_jumpsLeft = 1;

        [Header("Animation Params")]
        [SerializeField] private string m_animOnGround;
        [SerializeField] private string m_animOnWall;

        private DateTimeOffset m_jumpLastTimeStamp;

        private void Start()
        {
            InitObservers();
        }

        private void InitObservers()
        {
            //idle
            this.FixedUpdateAsObservable()
                .Where(_ => (m_modelInput.m_run == 0f))
                .Subscribe(_ => m_compAnimator.SetBool(m_animMove, false))
                .AddTo(this);

            //run
            this.FixedUpdateAsObservable()
                .Select(_ => m_modelInput.m_run)
                .Where(horizontalMovement => (horizontalMovement != 0) && m_reactiveIsMovEnabled.Value)
                .Subscribe(horizontalMovement =>
                {
                    CheckFlipHorizontal();

                    Vector2 movement = Vector2.zero;
                    movement.x = horizontalMovement;
                    StartMovement(movement);
                })
                .AddTo(this);

            //jump
            this.FixedUpdateAsObservable()
                .Select(_ => m_modelInput.m_jump)
                .Where(hasJumped => (hasJumped && (m_jumpsLeft > 0)))
                .Timestamp()
                .Where(x => x.Timestamp > m_jumpLastTimeStamp.AddSeconds(m_jumpsInterval))
                .Subscribe(x =>
                {
                    if(((m_ground.IsOnGround().Value || m_ground.IsWallHit().Value) && !m_canJumpInMidair) 
                        || m_canJumpInMidair)
                    {
                        Jump();
                        m_jumpLastTimeStamp = x.Timestamp;
                    }
                })
                .AddTo(this);

            //jump fall
            this.FixedUpdateAsObservable()
                .Select(_ => m_compRigidBody2D.velocity)
                .Where(velocity => (velocity.y < 0))
                .Subscribe(_ => {
                    //reference: "Better Jumping in Unity >> https://www.youtube.com/watch?v=7KiK0Aqtmzc
                    m_compRigidBody2D.velocity += (Vector2.up * Physics2D.gravity.y *
                        (m_jumpFallMultiplier - 1f) * Time.fixedDeltaTime);
                })
                .AddTo(this);

            //on ground
            m_ground.IsOnGround()
                .Subscribe(isOnGround => {
                    AnimateChangeGround(m_animOnGround, isOnGround);

                    if (m_ground.IsWallHit().Value)
                    { //to allow flipping due to wall sliding
                        CheckFlipHorizontal();
                    }
                })
                .AddTo(this);

            //wall sliding
            m_ground.IsWallHit()
                .Subscribe(isSliding =>
                {
                    AnimateChangeGround(m_animOnWall, isSliding);

                    if (isSliding && (m_modelInput.m_run == 0))
                    {
                        CheckFlipHorizontal();
                    }

                    ApplyWallSlide(isSliding);
                })
                .AddTo(this);

        }

        private void CheckFlipHorizontal()
        {
            //flip based on wall side, else flip based on movement direction
            bool shouldFlip = (m_ground.IsWallHit().Value && (!m_ground.IsOnGround().Value)) ?
                          ((m_ground.GetWallSide() == GroundType.Wall_Plus) ? true : false) :
                          (m_modelInput.m_run == 0f) ? m_compSpriteRenderer.flipX : (m_modelInput.m_run < 0f);

            //condition is to prevent jittering
            //do NOT flip if the previous value is the same as the new one
            if (m_compSpriteRenderer.flipX != shouldFlip)
            {
                m_compSpriteRenderer.flipX = shouldFlip;
            }
        }

        private void Jump()
        {
            //m_compRigidBody2D.AddForce(Vector2.up * (m_jumpHeight * m_jumpVelocity) 
            //    * Time.fixedDeltaTime, ForceMode2D.Impulse);
            m_compRigidBody2D.velocity = (Vector2.up * m_jumpVelocity);
            m_jumpsLeft--;
        }

        private void AnimateChangeGround(string paramGround, bool isOnGround)
        {
            if (m_compAnimator.GetBool(paramGround) != isOnGround)
            {
                m_compAnimator.SetBool(paramGround, isOnGround);
            }

            //reset jumps
            if (isOnGround) { m_jumpsLeft = m_jumpTimesMax; }
        }

        private void ApplyWallSlide(bool isActive)
        {
            m_compRigidBody2D.drag = (isActive) ? m_wallSlideFriction : m_wallSlideNormalFrictionDrag;
        }

    }


}