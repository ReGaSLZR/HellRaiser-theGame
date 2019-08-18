using Character.Stats;
using Injection;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Character.Skill {

    /// <summary>
    /// Ren's Note: This class was derived from SpotHitSkill (projects: LittleHellRaiser and ArcadeShooter)
    /// </summary>
    public class DealDamageSkill : BaseSkill
    {

        [Inject]
        private readonly Instantiator m_instantiator;

        [SerializeField]
        [Required]
        private TargetDetector m_targetDetector;

        [Space]

        [SerializeField]
        [MinMaxSlider(1, 100)]
        protected Vector2 m_skillDamageRange = new Vector2(1, 10);

        [Space]

        [SerializeField]
        private bool m_isDestroyedOnUse;

        [SerializeField]
        [Range(0f, 5f)]
        [EnableIf("m_isDestroyedOnUse")]
        private float m_delayBeforeDestruction = 0f;

        [Space]

        [SerializeField]
        private Transform m_childMain;
        [SerializeField]
        private Transform m_childDealDamageFX;

        protected override void Awake()
        {
            base.Awake();

            m_childMain.gameObject.SetActive(true);
            m_childDealDamageFX.gameObject.SetActive(false);
        }

        protected override void ExecuteUseSkill()
        {
            ExecuteSkill();

            if (m_isDestroyedOnUse)
            {
                m_childDealDamageFX.gameObject.SetActive(true);
                m_childMain.gameObject.SetActive(false);

                Destroy(this.gameObject, m_delayBeforeDestruction);
            }
        }

        private void ExecuteSkill() {
            List<BaseStats> killables = GetKillables();

            foreach (BaseStats killable in killables) {
                int rawDamage = StatsUtil.GetRawDamageDealt(GetSkillDamageFromRange(), m_statOffense);
                int critDamage = StatsUtil.GetCritDamage(rawDamage, m_statOffense);

                killable.DealDamage((rawDamage + critDamage), (critDamage > 0));
            }
        }

        private List<BaseStats> GetKillables()
        {
            List<BaseStats> killables = new List<BaseStats>();

            for (int x = 0; x < m_targetDetector.m_targets.Count; x++)
            {
                Collider2D targetDetected = m_targetDetector.m_targets[x];

                //safe check for when the target was destroyed by something else before this method fired
                if (targetDetected != null)
                { 
                    BaseStats killable = targetDetected.gameObject.GetComponent<BaseStats>();

                    if (killable != null)
                    {
                        killables.Add(killable);
                    }
                }
            }

            return killables;
        }

        private int GetSkillDamageFromRange()
        {
            return Mathf.RoundToInt(Random.Range(m_skillDamageRange.x, m_skillDamageRange.y));
        }

        

    }

}
