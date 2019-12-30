using Character.Stats;
using GamePlay.Base;
using Injection;
using NaughtyAttributes;
using Pooling;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Zenject;

namespace Character.Skill {

    /// <summary>
    /// Ren's Note: This class was derived from SpotHitSkill (projects: LittleHellRaiser and ArcadeShooter)
    /// </summary>
    public class DamageHealSkill : BaseSkill
    {

        private const int SKILL_VARIATION_DAMAGE = 0;
        private const int SKILL_VARIATION_HEAL = 1;

        private const int STAT_AFFECTED_HEALTH = 0;
        private const int STAT_AFFECTED_STAMINA = 1;

        private DropdownList<int> m_dropdownVariation = new DropdownList<int>()
        {
            {"Damage", SKILL_VARIATION_DAMAGE},
            {"Heal", SKILL_VARIATION_HEAL},
        };

        private DropdownList<int> m_dropdownStatAffected = new DropdownList<int>()
        {
            {"Health", STAT_AFFECTED_HEALTH},
            {"Stamina", STAT_AFFECTED_STAMINA},
        };

        [Inject]
        private readonly Instantiator m_instantiator;

        [Header("----- Child variables -----")]

        [SerializeField]
        [Required]
        private TargetDetector m_targetDetector;

        [Space]

        [SerializeField]
        private StatInflictionType m_inflictType;

        [SerializeField]
        [Dropdown("m_dropdownVariation")]
        private int m_variation;

        [SerializeField]
        [Dropdown("m_dropdownStatAffected")]
        private int m_statAffected;

        [SerializeField]
        [MinMaxSlider(1, 100)]
        protected Vector2 m_skillValueRange = new Vector2(1, 10);

        [Space]

        [SerializeField]
        private bool m_isDestroyedOnUse;

        [SerializeField]
        [Range(0f, 5f)]
        [EnableIf("m_isDestroyedOnUse")]
        private float m_delayBeforeDestruction = 0f;

        [SerializeField]
        [EnableIf("m_isDestroyedOnUse")]
        private BaseTrigger m_triggerOnDeath;

        [Space]

        [SerializeField]
        [EnableIf("m_isDestroyedOnUse")]
        private bool m_isPooled;

        [SerializeField]
        [EnableIf("m_isPooled")]
        private ObjectInPool m_poolItemReference;

        protected override void Awake()
        {
            base.Awake();

            if (m_isPooled && (m_poolItemReference == null))
            {
                LogUtil.PrintError(this, GetType(), "Awake(): " +
                    "No ObjectInPool reference. Switching to default destroy.");
                m_isPooled = false;
            }
        }

        protected override void OnSkillStart()
        {
            base.OnSkillStart();
            ExecuteSkill();

            if (m_isDestroyedOnUse)
            {
                if (m_triggerOnDeath != null)
                {
                    m_triggerOnDeath.Execute();
                }

                if (m_isPooled)
                {
                    m_poolItemReference.PutBackToPool(m_delayBeforeDestruction);
                    m_tempStopRepeatingSkill = false;
                    m_isExecutionFinished.Value = true;
                }
                else
                {
                    Destroy(this.gameObject, m_delayBeforeDestruction);
                }
                
            }
        }

        private void ExecuteSkill() {
            List<BaseStats> receivers = GetReceivers();
            int rawValue = StatsUtil.GetRawDamage(ValuesUtil.GetValueFromVector2Range(m_skillValueRange), m_statOffense, m_inflictType);
            int critValue = StatsUtil.GetCritDamage(rawValue, m_statOffense);

            for (int x=0; x<receivers.Count; x++) {
                DealDamageOrHeal(receivers[x], (rawValue + critValue), (critValue > 0));
            }
        }

        private List<BaseStats> GetReceivers()
        {
            List<BaseStats> receivers = new List<BaseStats>();
            List<Collider2D> targetsDetected = m_targetDetector.GetTargets();

            for (int x = 0; x < targetsDetected.Count; x++)
            {
                Collider2D targetDetected = targetsDetected[x];

                //safe check for when the target was destroyed by something else before this method fired
                if (targetDetected != null)
                { 
                    BaseStats receiver = targetDetected.gameObject.GetComponent<BaseStats>();

                    if (receiver != null)
                    {
                        receivers.Add(receiver);
                    }
                }
            }

            return receivers;
        }

        private void DealDamageOrHeal(BaseStats receiver, int value, bool isCritical) {
            if ((SKILL_VARIATION_DAMAGE == m_variation) && (STAT_AFFECTED_HEALTH == m_statAffected))
            {
                receiver.DealHealthDamage(value, isCritical, m_inflictType);
            }
            else if ((SKILL_VARIATION_DAMAGE == m_variation) && (STAT_AFFECTED_STAMINA == m_statAffected))
            {
                receiver.DealStaminaDamage(value, isCritical, m_inflictType);
            }
            else if ((SKILL_VARIATION_HEAL == m_variation) && (STAT_AFFECTED_HEALTH == m_statAffected))
            {
                receiver.RecoverHealth(value, isCritical, m_inflictType);
            }
            else if ((SKILL_VARIATION_HEAL == m_variation) && (STAT_AFFECTED_STAMINA == m_statAffected))
            {
                receiver.RecoverStamina(value, isCritical, m_inflictType);
            }
            else {
                LogUtil.PrintInfo(gameObject, GetType(), "DealDamageOrHeal(): Cannot find the Variation " +
                    "+ StatAffected combo of your choice. Combo is Variation: " + m_variation + 
                    " | StatAffected: " + m_statAffected);
            }

        }

    }

}
