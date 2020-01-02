using NaughtyAttributes;
using UnityEngine;

namespace Character.Skill
{

    /// <summary>
    /// The difference between the BaseSkill and this child class
    /// is that it will only "reveal" the object ONCE.
    /// Whereas, the BaseSkill's m_childFX is revealed and hidden
    /// depending on skill use.
    /// </summary>
    public class RevealSkill : BaseSkill
    {

        [Header("----- Child variables -----")]
        [Required]
        [SerializeField] private GameObject m_objectToReveal;

        private bool m_isRevealed;

        private void OnEnable()
        {
            if(m_objectToReveal != null)
            {
                m_objectToReveal.SetActive(false);
            }
        }

        public override void UseSkill()
        {
            if (!m_isRevealed)
            {
                base.UseSkill();
            }
        }

        protected override void OnSkillStart()
        {
            if ((m_objectToReveal != null) &&
                !m_objectToReveal.activeInHierarchy)
            {
                base.OnSkillStart();
                m_objectToReveal.SetActive(true);
            }

            m_isRevealed = true;
        }

    }

}