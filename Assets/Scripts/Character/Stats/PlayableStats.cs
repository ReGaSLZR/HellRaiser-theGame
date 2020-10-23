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

        private ReactiveProperty<int> m_reactiveMoney = new ReactiveProperty<int>(0);

        protected override void Awake()
        {
            if (!m_info.m_isPlayable)
            {
                LogUtil.PrintError(gameObject, GetType(), "Awake(): PlayableStats.m_info has non-playable stats. Destroying...");
                Destroy(this);
            }
            else
            {
                m_info.SetToFullHealthStamina(); //this is to ignore the changes to the SO's values due to the previous play mode
            }

            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            m_modelStatsSetter.RegisterCharacterForStats(m_info);
            m_reactiveMoney.Value = m_modelStatsGetter.GetInventoryMoney().Value;

            InitObservers();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            m_compCollider2D.enabled = true;
            //resetting money back to 0 to not allow the textStatChange to trigger (and display +0) when the character is re-enabled
            m_reactiveMoney.Value = 0;
        }

        private void InitObservers()
        {
            m_modelStatsGetter.GetActiveCharacterHealth()
                .Where(health => IsActiveCharacter() &&
                    (health != m_reactiveHealth.Value)) //last condition is to check if the change was already applied on BaseStat.Recover()/DealDamage()
                .Subscribe(newHealth =>
                {
                    ApplyStatChangeFXFromNonBaseStatCall(m_reactiveHealth, newHealth, m_colorScheme.m_healthGain, m_colorScheme.m_healthLoss);
                })
                .AddTo(m_disposables);

            m_modelStatsGetter.GetActiveCharacterStamina()
                .Where(stamina => IsActiveCharacter() &&
                    (stamina != m_reactiveStamina.Value)) //last condition is to check if the change was already applied on BaseStat.Recover()/DealDamage()
                .Subscribe(newStamina =>
                {
                    ApplyStatChangeFXFromNonBaseStatCall(m_reactiveStamina, newStamina, m_colorScheme.m_staminaGain, m_colorScheme.m_staminaLoss);
                })
                .AddTo(m_disposables);

            m_modelStatsGetter.GetInventoryMoney()
                .Where(money => (m_reactiveMoney.Value != money))
                //.Where(money => IsActiveCharacter())
                .Subscribe(newMoneyValue =>
                {
                    ApplyStatChangeFXFromNonBaseStatCall(m_reactiveMoney, newMoneyValue, m_colorScheme.m_moneyGain, m_colorScheme.m_moneyLoss);
                })
                .AddTo(m_disposables);
        }

        private bool IsActiveCharacter()
        {
            return m_info.m_avatar.m_name.Equals(
                    m_modelStatsGetter.GetActiveCharacter().Value.m_avatar.m_name);
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

        public override void DealHealthDamage(int damage, bool isCritical, StatInflictionType type)
        {
            base.DealHealthDamage(damage, isCritical, type);
            m_modelStatsSetter.UpdateCharacterHealth(m_info.m_avatar.m_name, m_reactiveHealth.Value);
        }

        public override void RecoverHealth(int health, bool isCritical, StatInflictionType type)
        {
            base.RecoverHealth(health, isCritical, type);
            m_modelStatsSetter.UpdateCharacterHealth(m_info.m_avatar.m_name, m_reactiveHealth.Value);
        }

        public override void DealStaminaDamage(int damage, bool isCritical, StatInflictionType type)
        {
            base.DealStaminaDamage(damage, isCritical, type);
            m_modelStatsSetter.UpdateCharacterStamina(m_info.m_avatar.m_name, m_reactiveStamina.Value);
        }

        public override void RecoverStamina(int stamina, bool isCritical, StatInflictionType type)
        {
            base.RecoverStamina(stamina, isCritical, type);
            m_modelStatsSetter.UpdateCharacterStamina(m_info.m_avatar.m_name, m_reactiveStamina.Value);
        }

    }

}