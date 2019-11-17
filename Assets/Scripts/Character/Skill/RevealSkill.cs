using NaughtyAttributes;
using UnityEngine;

namespace Character.Skill
{

    public class RevealSkill : BaseSkill
    {

        [Header("----- Child variables -----")]
        [Required]
        [SerializeField] private GameObject m_objectToReveal;

        private void OnEnable()
        {
            if(m_objectToReveal != null)
            {
                m_objectToReveal.SetActive(false);
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
            
        }

    }

}