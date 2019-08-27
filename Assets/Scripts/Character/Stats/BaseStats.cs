using Scriptables;
using System.Collections;
using UniRx;
using UnityEngine;
using Utils;

namespace Character.Stats {

    public class BaseStats : MonoBehaviour
    {

        protected ReactiveProperty<int> m_reactiveHealth = new ReactiveProperty<int>();
        protected ReactiveProperty<bool> m_reactiveIsHurt = new ReactiveProperty<bool>(false);

        [SerializeField]
        protected Scriptables.CharacterInfo m_info;

        protected CompositeDisposable m_disposables = new CompositeDisposable();

        protected virtual void Awake()
        {
            m_info.ResetHealthStamina();
            m_reactiveHealth.Value = m_info.m_health;
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

            int damageReceived = StatsUtil.GetDamageReceived(damage, m_info.m_defense);

            //TODO: show damage received as FX
            //TODO: show critical FX

            m_reactiveHealth.Value = Mathf.Clamp(
                (m_reactiveHealth.Value - damageReceived), 0, m_reactiveHealth.Value);
            StopAllCoroutines();
            StartCoroutine(CorStun());
        }

        private IEnumerator CorStun() {
            m_reactiveIsHurt.Value = true;
            yield return new WaitForSeconds(m_info.m_statMovement.m_stunLength);
            m_reactiveIsHurt.Value = false;
        }

    }

}