using UniRx;
using Zenject;
using GamePlay.Stats;
using Utils;
using UnityEngine;

namespace Character.Stats
{

    public class PlayableStats : BaseStats
    {

        [Inject]
        private readonly GamePlayStatsModel.Setter m_modelStatsSetter;
        [Inject]
        private readonly GamePlayStatsModel.Getter m_modelStatsGetter;

        private ReactiveProperty<int> m_reactiveMoney = new ReactiveProperty<int>();

        protected override void Awake()
        {
            base.Awake();

            if (!m_info.m_isPlayable) {
                LogUtil.PrintError(gameObject, GetType(), "Awake(): PlayableStats.m_info has non-playable stats. Destroying...");
                Destroy(this);
            }
        }

        private void OnEnable()
        {
            m_modelStatsSetter.RegisterCharacterForStats(m_info);
            m_reactiveMoney.Value = m_modelStatsGetter.GetInventoryMoney().Value;
            
            InitObservers();
        }

        private void InitObservers()
        {
            m_modelStatsGetter.GetActiveCharacterHealth()
                .Where(health => m_info.m_infoUI.m_name.Equals(
                    m_modelStatsGetter.GetActiveCharacter().Value.m_infoUI.m_name) &&
                    (health != m_reactiveHealth.Value)) //last condition is to check if the change was already applied on BaseStat.Recover()/DealDamage()
                .Subscribe(newHealth =>
                {
                    ApplyStatChangeFXFromNonBaseStatCall(m_reactiveHealth, newHealth, m_colorScheme.m_healthGain, m_colorScheme.m_healthLoss);
                })
                .AddTo(m_disposables);

            m_modelStatsGetter.GetActiveCharacterStamina()
                .Where(stamina => m_info.m_infoUI.m_name.Equals(
                    m_modelStatsGetter.GetActiveCharacter().Value.m_infoUI.m_name) &&
                    (stamina != m_reactiveStamina.Value)) //last condition is to check if the change was already applied on BaseStat.Recover()/DealDamage()
                .Subscribe(newStamina =>
                {
                    ApplyStatChangeFXFromNonBaseStatCall(m_reactiveStamina, newStamina, m_colorScheme.m_staminaGain, m_colorScheme.m_staminaLoss);
                })
                .AddTo(m_disposables);

            m_modelStatsGetter.GetInventoryMoney()
                //SPECIAL CASE: the first condition (Time check) is to not allow model.money initialization delay
                //(probably due to save file deserialization)
                //to show on game start the previous earnings of the Player as a text change FX
                .Where(money => (Time.timeSinceLevelLoad > 0) && (m_reactiveMoney.Value != money))
                .Subscribe(newMoneyValue => {
                    ApplyStatChangeFXFromNonBaseStatCall(m_reactiveMoney, newMoneyValue, m_colorScheme.m_moneyGain, m_colorScheme.m_moneyLoss);
                })
                .AddTo(m_disposables);
        }

        private void ApplyStatChangeFXFromNonBaseStatCall(ReactiveProperty<int> valueHolder, int newValue, 
            Color colorPositive, Color colorNegative)
        {
            int previousValue = valueHolder.Value;
            valueHolder.Value = newValue;

            string text = ((previousValue > newValue) ? "-" : "+") + Mathf.Abs(newValue - previousValue);
            UpdateStatChangeText(text, (previousValue > newValue) ? colorNegative : colorPositive);
            ForceShowStatChangeText();
        }

        public override void DealHealthDamage(int damage, bool isCritical)
        {
            base.DealHealthDamage(damage, isCritical);
            m_modelStatsSetter.UpdateCharacterHealth(m_info.m_infoUI.m_name, m_reactiveHealth.Value);
        }

        public override void RecoverHealth(int health)
        {
            base.RecoverHealth(health);
            m_modelStatsSetter.UpdateCharacterHealth(m_info.m_infoUI.m_name, m_reactiveHealth.Value);
        }

        public override void DealStaminaDamage(int damage, bool isCritical)
        {
            base.DealStaminaDamage(damage, isCritical);
            m_modelStatsSetter.UpdateCharacterStamina(m_info.m_infoUI.m_name, m_reactiveStamina.Value);
        }

        public override void RecoverStamina(int stamina)
        {
            base.RecoverStamina(stamina);
            m_modelStatsSetter.UpdateCharacterStamina(m_info.m_infoUI.m_name, m_reactiveStamina.Value);
        }

    }

}