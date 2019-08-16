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
        private float normalFrictionDrag = 1f;
        [SerializeField]
        private float wallSlideFriction = 10f;
        [SerializeField]
        private AudioClip wallSlideClip;

        [Header("Jump")]
        [SerializeField]
        private float jumpSpeed = 1.2f;
        [SerializeField]
        private float jumpHeight = 250f;
        [SerializeField]
        private AudioClip[] clipsJump;
        [SerializeField]
        private GameObject prefabJumpFX;
        [SerializeField]
        private GameObject originJumpFX;

        [Space]
        [SerializeField]
        private int jumpTimesMax = 1;
        [SerializeField]
        private int jumpsLeft = 1;
        [SerializeField]
        private float jumpInterval = 0.5f;

        [Header("Animation Params")]
        [SerializeField] private string paramIsWalking;
        [SerializeField] private string paramIsGrounded;
        [SerializeField] private string paramIsWallSliding;

        private DateTimeOffset _lastJumped;

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
            //this.FixedUpdateAsObservable()
            //    .Where(_ => (m_modelInput.m_run == 0f))
            //    .Subscribe(_ => AnimateIdleMovement())
            //    .AddTo(this);

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

            //this.FixedUpdateAsObservable()
            //    .Select(_ => m_modelInput.m_jump)
            //    .Where(hasJumped => (hasJumped && (jumpsLeft > 0)))
            //    .Timestamp()
            //    .Where(x => x.Timestamp > _lastJumped.AddSeconds(jumpInterval))
            //    .Subscribe(x => {
            //        Jump();
            //        _lastJumped = x.Timestamp;
            //    })
            //    .AddTo(this);

            //m_modelGround.IsOnGround()
            //    .Subscribe(isOnGround => AnimateChangeGround(paramIsGrounded, isOnGround))
            //    .AddTo(this);

            //m_modelGround.IsWallSliding()
            //    .Subscribe(isSliding => {
            //        AnimateChangeGround(paramIsWallSliding, isSliding);

            //        if (isSliding && (m_modelInput.m_run == 0))
            //        {
            //            CheckFlipHorizontal(m_modelInput.m_run);
            //        }

            //        ApplyWallSlide(isSliding);
            //    })
            //    .AddTo(this);

        }

        private void AnimateIdleMovement()
        {
            if (m_compAnimator.GetBool(paramIsWalking))
            {
                m_compAnimator.SetBool(paramIsWalking, false);
            }
        }

        private void AnimateHorizontalMovement(float horizontalMovement)
        {
            bool isSliding = (horizontalMovement != 0f);

            if (isSliding != m_compAnimator.GetBool(paramIsWalking))
            { //to prevent jitter
                m_compAnimator.SetBool(paramIsWalking, isSliding);
            }
        }

        private void CheckFlipHorizontal(float horizontalMovement)
        {
            bool shouldFlip = (horizontalMovement < 0f);

            //NOTE: this extra statement may negate the value of 'shouldFlip' if Player is WallSliding
            shouldFlip = (m_modelGround.IsOnWallRight().Value) ?
                          true : shouldFlip;

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

    }

}