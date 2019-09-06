using UnityEngine;
using NaughtyAttributes;

namespace Character.Skill
{

    public class CamouflageSkill : BaseSkill
    {

        [Header("----- Child variables -----")]

        //COMPONENTS
        [SerializeField]
        [Required]
        private Collider2D m_compCollider2D;

        [SerializeField]
        [TagSelector]
        private string m_tagOnCamouflage;

        [SerializeField]
        private Collider2D m_colliderOnCamouflage;

        private string m_tagOriginal;
        private Collider2D m_colliderOriginal;

        protected override void Awake()
        {
            base.Awake();

            m_colliderOriginal = m_compCollider2D;
            m_tagOriginal = gameObject.tag;
        }

        protected override void OnSkillStart()
        {
            base.OnSkillStart();
            gameObject.tag = m_tagOnCamouflage;

            if(m_colliderOnCamouflage != null) {
                m_compCollider2D = m_colliderOnCamouflage;
            }
        }

        protected override void OnSkillFinish()
        {
            base.OnSkillFinish();
            gameObject.tag = m_tagOriginal;
            m_compCollider2D = m_colliderOriginal;
        }

    }

}
