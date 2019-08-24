using UnityEngine;

namespace Character.Skill {

    [RequireComponent(typeof(Rigidbody2D))]
    public class FallSkill : BaseSkill
    {

        //COMPONENTS
        private Rigidbody2D m_compRigidbody2D;

        [Space]

        [SerializeField]
        [Range(1.1f, 5f)]
        private float m_gravityScale;

        protected override void Awake()
        {
            base.Awake();

            m_compRigidbody2D = GetComponent<Rigidbody2D>();
            m_compRigidbody2D.bodyType = RigidbodyType2D.Static;
        }

        protected override void ExecuteUseSkill()
        {
            m_compRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            m_compRigidbody2D.gravityScale = m_gravityScale;
        }

    }

}