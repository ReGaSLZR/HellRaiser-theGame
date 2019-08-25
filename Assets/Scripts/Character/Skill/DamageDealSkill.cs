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
    public class DamageDealSkill : BaseSkill
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

        protected override void OnSkillStart()
        {
            base.OnSkillStart();
            ExecuteSkill();

            if (m_isDestroyedOnUse)
            {
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
            List<Collider2D> targetsDetected = m_targetDetector.GetTargets();

            for (int x = 0; x < targetsDetected.Count; x++)
            {
                Collider2D targetDetected = targetsDetected[x];

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
