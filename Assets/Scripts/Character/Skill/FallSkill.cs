using NaughtyAttributes;
using UnityEngine;

namespace Character.Skill {

    public class FallSkill : BaseSkill
    {

        [Header("----- Child variables -----")]

        //COMPONENTS
        [SerializeField]
        [Required]
        private Rigidbody2D m_compRigidbody2D;

        [Space]

        [SerializeField]
        [Range(1.1f, 15f)]
        private float m_gravityScale = 1.1f;

        [SerializeField]
        private bool m_isStaticBeforeFall = true;

        protected override void Awake()
        {
            base.Awake();

            if (m_isStaticBeforeFall)
            {
                m_compRigidbody2D.bodyType = RigidbodyType2D.Static;
            }
        }

        protected override void OnSkillStart()
        {
            base.OnSkillStart();
            m_compRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            m_compRigidbody2D.gravityScale = m_gravityScale;
        }

    }

}