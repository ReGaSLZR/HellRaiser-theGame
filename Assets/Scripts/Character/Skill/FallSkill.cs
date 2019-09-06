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
        [Range(1.1f, 5f)]
        private float m_gravityScale;

        protected override void Awake()
        {
            base.Awake();

            m_compRigidbody2D.bodyType = RigidbodyType2D.Static;
        }

        protected override void OnSkillStart()
        {
            base.OnSkillStart();
            m_compRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            m_compRigidbody2D.gravityScale = m_gravityScale;
        }

    }

}