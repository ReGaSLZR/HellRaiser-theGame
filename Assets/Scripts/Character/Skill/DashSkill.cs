using UnityEngine;

namespace Character.Skill {

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class DashSkill : BaseSkill
    {

        //COMPONENTS
        private Rigidbody2D m_compRigidbody2D;
        private SpriteRenderer m_compSpriteRenderer;

        [SerializeField]
        [Range(100f, 5000f)]
        private float m_dashForce = 1f;

        private RigidbodyConstraints2D m_constraintsOriginal;
        private RigidbodyConstraints2D m_constraintsDash;

        protected override void Awake()
        {
            base.Awake();

            m_compRigidbody2D = GetComponent<Rigidbody2D>();
            m_compSpriteRenderer = GetComponent<SpriteRenderer>();

            m_constraintsOriginal = m_compRigidbody2D.constraints;
            m_constraintsDash = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }

        protected override void ExecuteUseSkill()
        {
            m_compRigidbody2D.AddForce(GetDirection() * m_dashForce);
            m_compRigidbody2D.constraints = m_constraintsDash;
        }

        protected override void OnSkillFinish()
        {
            base.OnSkillFinish();

            m_compRigidbody2D.constraints = m_constraintsOriginal;
            m_compRigidbody2D.velocity = Vector2.zero;
        }

        private Vector2 GetDirection() {
            return (m_compSpriteRenderer.flipX) ? Vector2.left : Vector2.right;
        }

    }


}