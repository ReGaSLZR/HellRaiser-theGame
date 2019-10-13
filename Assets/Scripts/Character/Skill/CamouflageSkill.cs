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
        private string m_layerOnCamouflage = "Default";
        private int m_layerOriginal;

        protected override void Awake()
        {
            base.Awake();

            m_layerOriginal = gameObject.layer;
        }

        protected override void OnSkillStart()
        {
            base.OnSkillStart();
            gameObject.layer = LayerMask.NameToLayer(m_layerOnCamouflage);
        }

        protected override void OnSkillFinish()
        {
            base.OnSkillFinish();
            gameObject.layer = m_layerOriginal;
        }

    }

}
