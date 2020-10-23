using NaughtyAttributes;
using UnityEngine;

namespace Character.Skill {

    public class DashSkill : BaseSkill
    {

        [Header("----- Child variables -----")]

        //COMPONENTS
        [SerializeField]
        [Required]
        private Rigidbody2D m_compRigidbody2D;

        [SerializeField]
        [Required]
        private SpriteRenderer m_compSpriteRenderer;

        [SerializeField]
        [Range(100f, 5000f)]
        private float m_dashForce = 1f;

        private RigidbodyConstraints2D m_constraintsOriginal;
        private RigidbodyConstraints2D m_constraintsDash;

        protected override void Awake()
        {
            base.Awake();

            m_constraintsOriginal = m_compRigidbody2D.constraints;
            m_constraintsDash = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }

        protected override void OnSkillStart()
        {
            base.OnSkillStart();
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