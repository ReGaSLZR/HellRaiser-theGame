using Character.Ground;
using GamePlayInput;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Character.Movement {

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerMovement : MonoBehaviour
    {

        //COMPONENTS
        private Animator m_compAnimator;
        private Rigidbody2D m_compRigidBody2D;
        private SpriteRenderer m_compSpriteRenderer;

        //INJECTIBLES
        [Inject]
        private PlayerGroundModel.Getter m_modelGround;
        [Inject]
        private BaseInputModel m_modelInput;

        //MOVEMENT STATS
        [Header("Horizontal Movement")]
        [SerializeField]
        private float m_runSpeed = 0.1f;

        [Header("Wall Slide")]
        [SerializeField]
        private float m_wallSlideNormalFrictionDrag = 1f;
        [SerializeField]
        private float m_wallSlideFriction = 10f;

        [Header("Jump")]
        [SerializeField]
        private float m_jumpSpeed = 1.2f;
        [SerializeField]
        private float m_jumpHeight = 5f;

        [Space]

        [SerializeField]
        private int m_jumpTimesMax = 1;
        [SerializeField]
        private int m_jumpsLeft = 1;
        [SerializeField]
        private float m_jumpsInterval = 0.5f;

        [Header("Animation Params")]
        [SerializeField] private string m_animRunning;
        [SerializeField] private string m_animOnGround;
        [SerializeField] private string m_animOnWall;

        private DateTimeOffset m_jumpLastTimeStamp;

        private void Awake()
        {
            m_compAnimator = GetComponent<Animator>();
            m_compSpriteRenderer = GetComponent<SpriteRenderer>();
            m_compRigidBody2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            InitObservers();
        }

        private void InitObservers()
        {
            //idle
            this.FixedUpdateAsObservable()
                .Where(_ => (m_modelInput.m_run == 0f))
                .Subscribe(_ => AnimateIdleMovement())
                .AddTo(this);

            //run
            this.FixedUpdateAsObservable()
                .Select(_ => m_modelInput.m_run)
                .Where(horizontalMovement => (horizontalMovement != 0))
                .Subscribe(horizontalMovement =>
                {
                    AnimateHorizontalMovement(horizontalMovement);
                    CheckFlipHorizontal(horizontalMovement);
                    MoveHorizontally(horizontalMovement);
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
                    Jump();
                    m_jumpLastTimeStamp = x.Timestamp;
                })
                .AddTo(this);

            //on ground
            m_modelGround.IsOnGround()
                .Subscribe(isOnGround => AnimateChangeGround(m_animOnGround, isOnGround))
                .AddTo(this);

            //wall sliding
            m_modelGround.IsOnWall()
                .Subscribe(isSliding =>
                {
                    AnimateChangeGround(m_animOnWall, isSliding);

                    if (isSliding && (m_modelInput.m_run == 0))
                    {
                        CheckFlipHorizontal(m_modelInput.m_run);
                    }

                    ApplyWallSlide(isSliding);
                })
                .AddTo(this);

        }

        private void AnimateIdleMovement()
        {
            if (m_compAnimator.GetBool(m_animRunning))
            {
                m_compAnimator.SetBool(m_animRunning, false);
            }
        }

        private void AnimateHorizontalMovement(float horizontalMovement)
        {
            bool isSliding = (horizontalMovement != 0f);

            if (isSliding != m_compAnimator.GetBool(m_animRunning))
            { //to prevent jitter
                m_compAnimator.SetBool(m_animRunning, isSliding);
            }
        }

        private void CheckFlipHorizontal(float horizontalMovement)
        {
            //flip based on wall side, else flip based on movement direction
            bool shouldFlip = (m_modelGround.IsOnWall().Value) ?
                          ((m_modelGround.GetWallSide() == PlayerGround.Wall_Right) ? true : false)
                          : (horizontalMovement < 0f);

            //condition is to prevent jittering
            //do NOT flip if the previous value is the same as the new one
            if (m_compSpriteRenderer.flipX != shouldFlip)
            {
                m_compSpriteRenderer.flipX = shouldFlip;
            }
        }

        private void MoveHorizontally(float horizontalMovement)
        {
            Vector2 movement = Vector2.zero;
            movement.x = horizontalMovement;

            m_compRigidBody2D.position = (m_compRigidBody2D.position +
                (movement * m_runSpeed * Time.fixedDeltaTime));
        }

        private void Jump()
        {
            m_compRigidBody2D.AddForce(Vector2.up * (m_jumpHeight * m_jumpSpeed), ForceMode2D.Impulse);
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