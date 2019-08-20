﻿using Scriptables;
using System.Collections;
using UniRx;
using UnityEngine;

namespace Character.Stats {

    public class BaseStats : MonoBehaviour
    {

        protected ReactiveProperty<int> m_reactiveHealth;
        protected ReactiveProperty<bool> m_reactiveIsHurt;

        [SerializeField]
        protected Scriptables.CharacterInfo m_stats;

        protected virtual void Awake()
        {
            m_reactiveHealth = new ReactiveProperty<int>(m_stats.m_health);
            m_reactiveIsHurt = new ReactiveProperty<bool>(false);
        }

        public StatMovement GetStatMovement() {
            return m_stats.m_statMovement;
        }

        public StatOffense GetStatOffense() {
            return m_stats.m_statOffense;
        }

        public ReactiveProperty<int> GetHealth() {
            return m_reactiveHealth;
        }

        public ReactiveProperty<bool> IsHurt() {
            return m_reactiveIsHurt;
        }

        public virtual void RecoverHealth(int health) {
            if (health <= 0) {
                LogUtil.PrintWarning(gameObject, GetType(), "AddHealth(): Invalid value of " + health);
                return;
            }

            //TODO: show added health as FX

            m_reactiveHealth.Value = Mathf.Clamp(m_reactiveHealth.Value + health, 0, Scriptables.CharacterInfo.HEALTH_MAX);
        }

        public virtual void DealDamage(int damage, bool isCritical) {
            //TODO: code deflection (chance + application)

            int damageReceived = StatsUtil.GetDamageReceived(damage, m_stats.m_defense);

            //TODO: show damage received as FX
            //TODO: show critical FX

            m_reactiveHealth.Value = Mathf.Clamp(
                (m_reactiveHealth.Value - damageReceived), 0, m_reactiveHealth.Value);
            StopAllCoroutines();
            StartCoroutine(CorStun());
        }

        private IEnumerator CorStun() {
            m_reactiveIsHurt.Value = true;
            yield return new WaitForSeconds(m_stats.m_statMovement.m_stunLength);
            m_reactiveIsHurt.Value = false;
        }

    }

}