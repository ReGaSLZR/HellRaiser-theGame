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
        private string m_layerOnCamouflage = "Default";

        [SerializeField]
        [Required]
        private Rigidbody2D m_compRigidbody2D;

        [SerializeField]
        [Range(1.1f, 15f)]
        private float m_gravityScaleOnCamouflage = 1.1f;

        private float m_gravityScaleOriginal;
        private int m_layerOriginal;

        protected override void Awake()
        {
            base.Awake();

            m_gravityScaleOriginal = m_compRigidbody2D.gravityScale;
            m_layerOriginal = m_camouflageParent.gameObject.layer;
        }

        protected override void OnSkillStart()
        {
            base.OnSkillStart();

            m_camouflageParent.gameObject.layer = LayerMask.NameToLayer(m_layerOnCamouflage);
            m_compRigidbody2D.gravityScale = m_gravityScaleOnCamouflage;
        }

        protected override void OnSkillFinish()
        {
            base.OnSkillFinish();

            m_camouflageParent.gameObject.layer = m_layerOriginal;
            m_compRigidbody2D.gravityScale = m_gravityScaleOriginal;
        }

    }

}
