using Character.Ground;
using NaughtyAttributes;
using UniRx;
using UnityEngine;

namespace Character.Movement {

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class BaseCharacterMovement : MonoBehaviour
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
        protected float m_movementSpeed = 0.1f;

        protected ReactiveProperty<bool> m_reactiveIsMovEnabled { private set; get; }

        protected virtual void Awake()
        {
            m_reactiveIsMovEnabled = new ReactiveProperty<bool>(true);

            m_compAnimator = GetComponent<Animator>();
            m_compSpriteRenderer = GetComponent<SpriteRenderer>();
            m_compRigidBody2D = GetComponent<Rigidbody2D>();
        }

        protected void StartMovement(Vector2 movementDirection) {
            if(!m_reactiveIsMovEnabled.Value)
            {
                LogUtil.PrintInfo(gameObject, GetType(), "StartMovement(): Movement is stopped for the moment.");
                return;
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

        public void SetMovementEnabled(bool isEnabled) {
            m_reactiveIsMovEnabled.Value = isEnabled;
        }

    }

}