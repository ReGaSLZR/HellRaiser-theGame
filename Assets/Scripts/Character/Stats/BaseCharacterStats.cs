using System.Collections;
using UniRx;
using UnityEngine;

namespace Character.Stats {

    public abstract class BaseCharacterStats : MonoBehaviour
    {

        private ReactiveProperty<int> m_reactiveHealth;
        private ReactiveProperty<bool> m_reactiveIsHurt;

        [SerializeField]
        protected CharacterStats m_stats;

        private void Awake()
        {
            m_reactiveHealth = new ReactiveProperty<int>(m_stats.m_health);
            m_reactiveIsHurt = new ReactiveProperty<bool>(false);
        }

        public float GetMovementSpeed() {
            return m_stats.m_movementSpeed;
        }

        public float GetStunLength() {
            return m_stats.m_stunLength;
        }

        public ReactiveProperty<int> GetHealth() {
            return m_reactiveHealth;
        }

        public ReactiveProperty<bool> IsHurt() {
            return m_reactiveIsHurt;
        }

        public void AddHealth(int health) {
            if (health <= 0) {
                LogUtil.PrintWarning(gameObject, GetType(), "AddHealth(): Invalid value of " + health);
                return;
            }

            //TODO: show added health as FX

            m_reactiveHealth.Value += health;
        }

        public void DealDamage(int damage, bool isCritical) {
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
            yield return new WaitForSeconds(m_stats.m_stunLength);
            m_reactiveIsHurt.Value = false;
        }

    }

}