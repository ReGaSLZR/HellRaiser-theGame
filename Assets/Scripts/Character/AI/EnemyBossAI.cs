using Character.Movement;
using Character.Skill;
using Character.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Character.AI {

    public class EnemyBossAI : BaseAI
    {

        [System.Serializable]
        public class BossPhase {

            public TargetDetector m_targetDetector;
            public BaseMovement m_movement;
            public BaseSkill m_skill;
            public BaseStats m_stats;

            public BossPhase(
                TargetDetector targetDetector,
                BaseMovement movement,
                BaseSkill skill,
                BaseStats baseStats) {
                m_targetDetector = targetDetector;
                m_movement = movement;
                m_skill = skill;
                m_stats = baseStats;
            }

            public void SetPhaseEnabled(bool isEnabled) {
                m_targetDetector.enabled = isEnabled;
                m_movement.enabled = isEnabled;
                m_skill.enabled = isEnabled;
                m_stats.enabled = isEnabled;
            }

        }

        [Header("----- Child variables -----")]

        [SerializeField]
        [Tooltip("Do NOT include in this list the entries for MAIN Movement, Stats and Skill. They will be put as index 0 in runtime.")]
        private List<BossPhase> m_phases;

        protected override void Awake()
        {
            base.Awake();

            if (m_phases.Count == 0)
            {
                LogUtil.PrintWarning(gameObject, GetType(), "Awake(): An enemy boss with no phases? You can just use an EnemyAI for this character then.");
            }
            else {
                //inserting the main Movement and Skill defined on BaseAI as BossPhase index 0
                m_phases.Insert(0, new BossPhase(
                    m_targetDetector,
                    m_movement,
                    m_skillMain,
                    m_stats));
            }


            DisableAllPhases();
            m_phases[0].SetPhaseEnabled(true);
        }

        protected override void OnDeath()
        {
            if (m_phases.Count > 1) {
                m_skillMain.StopSkill(true);
                StartCoroutine(CorSwitchToNewPhase());
            }
            else
            {
                base.OnDeath();
            }
        }

        private void DisableAllPhases() {
            for (int x=0; x<m_phases.Count; x++) {
                m_phases[x].SetPhaseEnabled(false);
            }
        }

        private IEnumerator CorSwitchToNewPhase()
        {
            yield return new WaitForSeconds(m_stats.GetStatMovement().m_deathLength);
            DeleteOldPhase();
            MoveToNewPhase();
            ResetBase();
        }

        private void DeleteOldPhase() {
            m_phases[0].SetPhaseEnabled(false);

            m_phases.RemoveAt(0);
            m_disposables.Clear();
        }

        private void MoveToNewPhase() {
            BossPhase newPhase = m_phases[0];

            newPhase.SetPhaseEnabled(true);

            m_targetDetector = newPhase.m_targetDetector;
            m_movement = newPhase.m_movement;
            m_skillMain = newPhase.m_skill;
            m_stats = newPhase.m_stats;
        }

        private void ResetBase()
        {
            m_disposables.Clear();
            m_disposablesUncontrolled.Clear();

            DistributeStats();
            InitControlledObservers();
            InitUncontrolledObservers();
        }

    }


}