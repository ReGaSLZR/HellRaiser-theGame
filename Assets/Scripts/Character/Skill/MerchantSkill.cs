using GamePlay.Stats;
using Scriptables;
using UnityEngine;
using Zenject;

namespace Character.Skill {

    public class MerchantSkill : BaseSkill
    {

        [Inject]
        private readonly MerchantModel.Setter m_modelMerchant;

        [Header("----- Child variables -----")]

        [SerializeField]
        private MerchantItem[] m_goodies = new MerchantItem[3];

        protected override void OnSkillStart()
        {
            base.OnSkillStart();
            m_modelMerchant.SetMerchantItems(m_goodies);
            m_modelMerchant.SetIsViewingMerchantGoods(true);
        }

    }

}