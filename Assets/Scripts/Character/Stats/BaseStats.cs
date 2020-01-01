using NaughtyAttributes;
using Scriptables;
using System.Collections;
using TMPro;
using UniRx;
using UnityEngine;
using Utils;
using Zenject;
using static Scriptables.PlaySettings;

namespace Character.Stats
{

    public class BaseStats : MonoBehaviour
    {

        [Inject]
        protected readonly ColorScheme m_colorScheme;

        [Header("----- Base variables -----")]

        [SerializeField]
        [Required]
        protected Collider2D m_compCollider2D;

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
            m_reactiveHealth.Value = m_info.m_health;
            m_reactiveStamina.Value = m_info.m_stamina;
        }

        protected virtual void OnEnable()
        {
            m_compCollider2D.enabled = true;
        }

        protected virtual void OnDisable()
        {
            m_compCollider2D.enabled = false;
            m_disposables.Clear();
        }

        public string GetCharacterName()
        {
            return m_info.m_avatar.m_name;
        }

        public CharacterSkill[] GetCharacterSkills()
        {
            return m_info.m_skillsInOrder;
        }

        public StatMovement GetStatMovement()
        {
            return m_info.m_statMovement;
        }

        public StatOffense GetStatOffense()
        {
            return m_info.m_statOffense;
        }

        public ReactiveProperty<int> GetHealth()
        {
            return m_reactiveHealth;
        }

        public ReactiveProperty<bool> IsHurt()
        {
            return m_reactiveIsHurt;
        }

        private string GetCriticalAppend(bool isCritical)
        {
            return (isCritical) ? "\n Crit!" : "";
        }

        private Color GetFeedbackTextColor(int finalInflictionValue, Color customColor)
        {
            return (finalInflictionValue == 0) ? m_colorScheme.m_damageNull : customColor;
        }

        private string GetStatChangeText(int changeValue, bool isDamage, bool isCritical, StatInflictionType type)
        {
            string sign = (isDamage) ? "-" : "+";

            return (changeValue == 0) ? "No Damage" :
                sign + changeValue.ToString() + GetCriticalAppend(isCritical) +  //sample: -99\nCRIT!
                    ((StatsUtil.IsInflictionReduceableByBane(m_info.m_statDefense.m_isMagusBane, type)) ? //sample: (if bane) \nREDUCED! (else) *blank*
                    ("\n" + StatsUtil.GetMagickDamageFeedbackOnMagusBane(m_info.m_rank, changeValue)) : "");
        }

        private void DealDamage(int damage, bool isCritical,
            StatInflictionType type, ReactiveProperty<int> valueHolder, Color color)
        {
            if (damage <= 0)
            {
                LogUtil.PrintWarning(gameObject, GetType(), "DealDamage(): Invalid value of " + damage);
                return;
            }

            int damageReceivedReducedByDefenseOrBane = StatsUtil.GetDamageReducedByDefense(damage, m_info.m_statDefense, type, m_info.m_rank);

            UpdateStatChangeText(GetStatChangeText(damageReceivedReducedByDefenseOrBane, true, isCritical, type),
                GetFeedbackTextColor(damageReceivedReducedByDefenseOrBane, color));
            ForceShowStatChangeText();

            valueHolder.Value = Mathf.Clamp(
                (valueHolder.Value - damageReceivedReducedByDefenseOrBane), 0, valueHolder.Value);

            if (damageReceivedReducedByDefenseOrBane > 0)
            {
                StopAllCoroutines();
                StartCoroutine(CorStun());
            }

        }

        private void Recover(int value, int maxValue, bool isCritical,
            StatInflictionType type, ReactiveProperty<int> valueHolder, Color color)
        {
            if (value <= 0)
            {
                LogUtil.PrintWarning(gameObject, GetType(), "Recover(): Invalid value of " + value);
                return;
            }

            int recoveryValue;
            string statChangeText;

            if (StatInflictionType.TIME == type)
            {
                recoveryValue = value;
                statChangeText = string.Concat("+", recoveryValue.ToString());
            }
            else
            {
                recoveryValue = StatsUtil.GetRecoveryValue(value, type, m_info.m_statDefense.m_isMagusBane, m_info.m_rank);
                statChangeText = (recoveryValue == 0) ? "No Effect" :
                    ("+" + recoveryValue.ToString() + GetCriticalAppend(isCritical) + //sample: +99\nCRIT!
                    (StatsUtil.IsInflictionReduceableByBane(m_info.m_statDefense.m_isMagusBane, type) ? //sample: (if bane) \nREDUCED! (else) *blank*
                        ("\n" + StatsUtil.GetMagickDamageFeedbackOnMagusBane(m_info.m_rank, recoveryValue)) : ""));
            }

            UpdateStatChangeText(statChangeText,
                    GetFeedbackTextColor(recoveryValue, color));
            ForceShowStatChangeText();

            if (valueHolder != null) {
                valueHolder.Value = Mathf.Clamp(valueHolder.Value + recoveryValue, 0, maxValue);
            }
        }

        public virtual void RecoverHealth(int health, bool isCritical, StatInflictionType type)
        {
            Recover(health, Scriptables.CharacterInfo.HEALTH_MAX, isCritical, type, m_reactiveHealth, m_colorScheme.m_healthGain);
        }

        public virtual void DealHealthDamage(int damage, bool isCritical, StatInflictionType type)
        {
            DealDamage(damage, isCritical, type, m_reactiveHealth, m_colorScheme.m_healthLoss);
        }

        public virtual void RecoverStamina(int stamina, bool isCritical, StatInflictionType type)
        {
            Recover(stamina, Scriptables.CharacterInfo.STAMINA_MAX, isCritical, type, m_reactiveStamina, m_colorScheme.m_staminaGain);
        }

        public virtual void DealStaminaDamage(int damage, bool isCritical, StatInflictionType type)
        {
            DealDamage(damage, isCritical, type, m_reactiveStamina, m_colorScheme.m_staminaLoss);
        }

        public virtual void RecoverTime(int additionalTime)
        {
            Recover(additionalTime, additionalTime, false, StatInflictionType.TIME, null, m_colorScheme.m_time);
        }

        protected void UpdateStatChangeText(string text, Color color)
        {
            if (m_textStatChange == null)
            {
                //LogUtil.PrintWarning(gameObject, GetType(), "UpdateStatChangeText(): NULL stat change text.");
                return;
            }

            m_textStatChange.text = text;
            m_textStatChange.color = color;
        }

        /// <summary>
        /// Make sure variable m_textStatChange has an animation that plays 
        /// upon gameObject.SetActive(true) and hides itself after;
        /// </summary>
        protected void ForceShowStatChangeText()
        {
            if (m_textStatChange == null)
            {
                //LogUtil.PrintInfo(gameObject, GetType(), "ForceShowStatChangeText(): NULL stat change text.");
                return;
            }

            if (m_textStatChange.gameObject.activeSelf)
            {
                m_textStatChange.gameObject.SetActive(false);
            }

            m_textStatChange.gameObject.SetActive(true);
        }

        private IEnumerator CorStun()
        {
            m_reactiveIsHurt.Value = true;
            yield return new WaitForSeconds((m_reactiveHealth.Value <= 0) ?
                m_info.m_statMovement.m_deathLength : m_info.m_statMovement.m_stunLength);
            m_reactiveIsHurt.Value = false;
        }

    }

}