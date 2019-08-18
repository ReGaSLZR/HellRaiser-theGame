using Character.Ground;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using static UnityEngine.RectTransform;

namespace Character.Movement {

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class BaseMovement : MonoBehaviour
    {

        //COMPONENTS
        protected Animator m_compAnimator;
        protected Rigidbody2D m_compRigidBody2D;
        protected SpriteRenderer m_compSpriteRenderer;

        [SerializeField]
        [Required]
        protected GroundManager m_ground;

        [Space]

        [SerializeField]
        protected string m_animMove;
        [SerializeField]
        private string m_animStunned;

        [Space]

        [SerializeField]
        protected Axis m_movementDirection;

        [SerializeField]
        protected bool m_shouldFlipSprite;

        private float m_movementSpeed = 0.1f;

        protected ReactiveProperty<bool> m_reactiveIsMovEnabled { private set; get; }

        protected virtual void Awake()
        {
            m_reactiveIsMovEnabled = new ReactiveProperty<bool>(true);

            m_compAnimator = GetComponent<Animator>();
            m_compSpriteRenderer = GetComponent<SpriteRenderer>();
            m_compRigidBody2D = GetComponent<Rigidbody2D>();
        }

        protected void StartMovement(Vector2 movementDirection) {
            if (!m_reactiveIsMovEnabled.Value)
            {
                LogUtil.PrintInfo(gameObject, GetType(), "StartMovement(): Movement is stopped for the moment.");
                return;
            }

            if (!m_compAnimator.GetBool(m_animMove))
            { //to prevent jitters
                m_compAnimator.SetBool(m_animMove, true);
            }

            if (RigidbodyType2D.Dynamic == m_compRigidBody2D.bodyType)
            {
                m_compRigidBody2D.position = (m_compRigidBody2D.position +
                    (movementDirection * m_movementSpeed * Time.fixedDeltaTime));
            }
            else {
                m_compRigidBody2D.MovePosition(m_compRigidBody2D.position +
                    (movementDirection * m_movementSpeed * Time.fixedDeltaTime));
            }
        }

        protected bool ShouldFlip(Transform transformToFace) {
            if (!m_shouldFlipSprite) { //quickly return if condition is satisfied; else, perform direction calculation
                return false;
            }

            float direction = ((Axis.Horizontal == m_movementDirection) ? 
                gameObject.transform.position.x : gameObject.transform.position.y) 
                - ((Axis.Horizontal == m_movementDirection) ? 
                transformToFace.position.x : transform.position.y);

            return (direction >= 0f);
        }

        protected bool ShouldFlip(float movement) {
            return m_shouldFlipSprite ? (movement < 0f) : false;
        }

        public void Face(Transform transformToFace) {
            bool shouldFlip = ShouldFlip(transformToFace);

            if (Axis.Horizontal == m_movementDirection)
            {
                m_compSpriteRenderer.flipX = shouldFlip;
            }
            else
            {
                m_compSpriteRenderer.flipY = shouldFlip;
            }
        }

        public void SetMovementSpeed(float movementSpeed) {
            m_movementSpeed = movementSpeed;
        }

        public void SetMovementEnabled(bool isEnabled) {
            m_reactiveIsMovEnabled.Value = isEnabled;
        }

        public void StunMovement() {
            SetMovementEnabled(false);
            m_compAnimator.SetBool(m_animMove, false);
            m_compAnimator.SetBool(m_animStunned, true);
        }

        public void UnStunMovement() {
            SetMovementEnabled(true);
            m_compAnimator.SetBool(m_animStunned, false);
        }

    }

}