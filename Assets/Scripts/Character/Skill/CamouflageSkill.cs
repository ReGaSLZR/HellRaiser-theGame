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

        [Space]

        [SerializeField]
        private string m_layerOnCamouflage = "Default";
        [SerializeField]
        [TagSelector]
        private string m_tagOnCamouflage = "";

        [Space]

        [SerializeField]
        [Required]
        private Rigidbody2D m_compRigidbody2D;

        [SerializeField]
        [Range(1.1f, 15f)]
        private float m_gravityScaleOnCamouflage = 1.1f;

        private float m_gravityScaleOriginal;
        private int m_layerOriginal;
        private string m_tagOriginal;

        protected override void Awake()
        {
            base.Awake();

            m_gravityScaleOriginal = m_compRigidbody2D.gravityScale;
            m_layerOriginal = m_camouflageParent.gameObject.layer;
            m_tagOriginal = m_camouflageParent.gameObject.tag;
        }

        protected override void OnSkillStart()
        {
            base.OnSkillStart();

            m_camouflageParent.gameObject.layer = LayerMask.NameToLayer(m_layerOnCamouflage);
            m_camouflageParent.gameObject.tag = m_tagOnCamouflage;
            m_compRigidbody2D.gravityScale = m_gravityScaleOnCamouflage;
        }

        protected override void OnSkillFinish()
        {
            base.OnSkillFinish();

            m_camouflageParent.gameObject.layer = m_layerOriginal;
            m_camouflageParent.gameObject.tag = m_tagOriginal;
            m_compRigidbody2D.gravityScale = m_gravityScaleOriginal;
        }

    }

}
