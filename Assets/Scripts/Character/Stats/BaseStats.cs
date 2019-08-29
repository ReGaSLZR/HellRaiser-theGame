using NaughtyAttributes;
using Scriptables;
using System.Collections;
using TMPro;
using UniRx;
using UnityEngine;
using Utils;
using Zenject;
using static Scriptables.PlaySettings;

namespace Character.Stats {

    public class BaseStats : MonoBehaviour
    {

        [Inject]
        protected readonly ColorScheme m_colorScheme;

        [SerializeField]
        [Required]
        protected Scriptables.CharacterInfo m_info;

        [Space]

        [SerializeField]
        protected TextMeshPro m_textStatChange;

        protected ReactiveProperty<int> m_reactiveHealth = new ReactiveProperty<int>();
        protected ReactiveProperty<int> m_reactiveStamina = new ReactiveProperty<int>();
        protected ReactiveProperty<bool> m_reactiveIsHurt = new ReactiveProperty<bool>(false);

        protected CompositeDisposable m_disposables = new CompositeDisposable();

        protected virtual void Awake()
        {
            m_info.ResetHealthStamina();
            m_reactiveHealth.Value = m_info.m_health;
            m_reactiveStamina.Value = m_info.m_stamina;
        }

        private void OnDisable()
        {
            m_disposables.Clear();
        }

        public string GetCharacterName() {
            return m_info.m_infoUI.m_name;
        }

        public CharacterInfoSkill[] GetCharacterSkills() {
            return m_info.m_skillsInOrder;
        }

        public StatMovement GetStatMovement() {
            return m_info.m_statMovement;
        }

        public StatOffense GetStatOffense() {
            return m_info.m_statOffense;
        }

        public ReactiveProperty<int> GetHealth() {
            return m_reactiveHealth;
        }

        public ReactiveProperty<bool> IsHurt() {
            return m_reactiveIsHurt;
        }

        private void DealDamage(int damage, bool isCritical, ReactiveProperty<int> valueHolder, Color color)
        {
            if (damage <= 0)
            {
                LogUtil.PrintWarning(gameObject, GetType(), "DealDamage(): Invalid value of " + damage);
                return;
            }

            int damageReceived = StatsUtil.GetDamageReceived(damage, m_info.m_defense);

            string damageAppend = (isCritical) ? " CRIT!" : "";
            UpdateStatChangeText("-" + damageReceived.ToString() + damageAppend, color);
            ForceShowStatChangeText();

            valueHolder.Value = Mathf.Clamp(
                (valueHolder.Value - damageReceived), 0, valueHolder.Value);

            StopAllCoroutines();
            StartCoroutine(CorStun());

        }

        private void Recover(int value, int maxValue, ReactiveProperty<int> valueHolder, Color color) {
            if (value <= 0)
            {
                LogUtil.PrintWarning(gameObject, GetType(), "Recover(): Invalid value of " + value);
                return;
            }

            UpdateStatChangeText("+" + value.ToString(), color);
            ForceShowStatChangeText();

            valueHolder.Value = Mathf.Clamp(valueHolder.Value + value, 0, maxValue);
        }

        public virtual void RecoverHealth(int health) {
            Recover(health, Scriptables.CharacterInfo.HEALTH_MAX, m_reactiveHealth, m_colorScheme.m_healthGain);
        }

        public virtual void DealHealthDamage(int damage, bool isCritical) {
            DealDamage(damage, isCritical, m_reactiveHealth, m_colorScheme.m_healthLoss);
        }

        public virtual void RecoverStamina(int stamina) {
            Recover(stamina, Scriptables.CharacterInfo.STAMINA_MAX, m_reactiveStamina, m_colorScheme.m_staminaGain);
        }

        public virtual void DealStaminaDamage(int damage, bool isCritical) {
            DealDamage(damage, isCritical, m_reactiveStamina, m_colorScheme.m_staminaLoss);
        }

        protected void UpdateStatChangeText(string text, Color color) {
            if (m_textStatChange == null) {
                LogUtil.PrintWarning(gameObject, GetType(), "UpdateStatChangeText(): NULL stat change text.");
                return;
            }

            m_textStatChange.text = text;
            m_textStatChange.color = color;
        }

        /// <summary>
        /// Make sure variable m_textStatChange has an animation that plays 
        /// upon gameObject.SetActive(true) and hides itself after;
        /// </summary>
        protected void ForceShowStatChangeText() {
            if (m_textStatChange == null) {
                LogUtil.PrintInfo(gameObject, GetType(), "ForceShowStatChangeText(): NULL stat change text.");
                return;
            }

            if (m_textStatChange.gameObject.activeSelf) {
                m_textStatChange.gameObject.SetActive(false);
            }

            m_textStatChange.gameObject.SetActive(true);
        }

        private IEnumerator CorStun() {
            m_reactiveIsHurt.Value = true;
            yield return new WaitForSeconds(m_info.m_statMovement.m_stunLength);
            m_reactiveIsHurt.Value = false;
        }

    }

}