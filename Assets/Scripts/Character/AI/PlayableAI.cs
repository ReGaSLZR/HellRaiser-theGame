using Character.Skill;
using GamePlay.Input;
using GamePlay.Stats;
using NaughtyAttributes;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Character.AI {

    public class PlayableAI : BaseAI
    {

        [Inject]
        private BaseInputModel m_modelInput;
        [Inject]
        private readonly GamePlayStatsModel.Setter m_modelStatsSetter;
        [Inject]
        private readonly GamePlayStatsModel.Getter m_modelStatsGetter;

        [SerializeField]
        [Required]
        private BaseSkill m_skillSecondary;

        [SerializeField]
        [Required]
        private BaseSkill m_skillTertiary;

        private void Awake()
        {
            if(m_targetDetector != null) {
                LogUtil.PrintWarning(gameObject, GetType(), "Awake(): TargetDetector is not needed in PlayerAI.");
                m_targetDetector = null;
            }
        }

        protected override void Start()
        {
            base.Start();
            InitInputObservers();
        }

        private void InitInputObservers() {
            //skill MAIN
            this.FixedUpdateAsObservable()
                .Select(_ => m_modelInput.m_skillMain)
                .Where(isUsingSkill => isUsingSkill && IsStaminaValueEnough(m_characterStats.GetSkillCosts()[0]))
                .Subscribe(_ => {
                    m_skillMain.UseSkill();
                    UpdateStamina(m_characterStats.GetSkillCosts()[0]);
                })
                .AddTo(this);

            //skill 2
            this.FixedUpdateAsObservable()
               .Select(_ => m_modelInput.m_skill2)
               .Where(isUsingSkill => isUsingSkill && IsStaminaValueEnough(m_characterStats.GetSkillCosts()[1]))
               .Subscribe(_ =>
               {
                   m_skillSecondary.UseSkill();
                   UpdateStamina(m_characterStats.GetSkillCosts()[1]);
               })
               .AddTo(this);

            //skill 3
            this.FixedUpdateAsObservable()
               .Select(_ => m_modelInput.m_skill3)
               .Where(isUsingSkill => isUsingSkill && IsStaminaValueEnough(m_characterStats.GetSkillCosts()[2]))
               .Subscribe(isUsingSkill =>
                { 
                       m_skillTertiary.UseSkill();
                       UpdateStamina(m_characterStats.GetSkillCosts()[2]);
               })
               .AddTo(this);
        }

        private bool IsStaminaValueEnough(int decrement) {
            return (m_modelStatsGetter.GetCharacterStamina().Value >= decrement);
        }

        private void UpdateStamina(int decrement) {
            m_modelStatsSetter.UpdateCharacterStamina(
                            m_modelStatsGetter.GetCharacterStamina().Value - decrement);
        }

        protected override void OnDeath() {
            m_skillMain.StopSkill(false);
            m_skillSecondary.StopSkill(false);
            m_skillTertiary.StopSkill(false);

            base.OnDeath();
        }

    }

}