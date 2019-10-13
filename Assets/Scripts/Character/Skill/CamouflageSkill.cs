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
        private Transform m_camouflageParent;

        [SerializeField]
        [Required]
        private Collider2D m_compCollider2D;

        [SerializeField]
        private string m_layerOnCamouflage = "Default";
        private int m_layerOriginal;

        protected override void Awake()
        {
            base.Awake();

            m_layerOriginal = m_camouflageParent.gameObject.layer;
        }

        protected override void OnSkillStart()
        {
            base.OnSkillStart();
            m_camouflageParent.gameObject.layer = LayerMask.NameToLayer(m_layerOnCamouflage);
        }

        protected override void OnSkillFinish()
        {
            base.OnSkillFinish();
            m_camouflageParent.gameObject.layer = m_layerOriginal;
        }

    }

}
