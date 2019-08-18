using System.Collections;
using UnityEngine;

namespace Character.Skill {

    [RequireComponent(typeof(Animator))]
    public abstract class BaseSkill : MonoBehaviour
    {

        //COMPONENTS
        protected Animator m_compAnimator;

        [SerializeField]
        protected string m_animSkill;

        [SerializeField]
        [Range(1, 999)]
        protected int m_useCount = 999;
        [SerializeField]
        [Range(0.25f, 5f)]
        protected float m_useInterval = 0.25f;
        [SerializeField]
        [Range(0.25f, 10f)]
        protected float m_cooldown = 0.25f;
        [SerializeField]
        protected bool m_isRepeating;

        private bool m_tempStopRepeatingSkill;
        private bool m_isInCooldown;

        protected abstract void ExecuteUseSkill();

        protected virtual void Awake() {
            m_compAnimator = GetComponent<Animator>();
        }

        public void UseSkill() {
            m_tempStopRepeatingSkill = false;

            if (!m_isInCooldown && (m_useCount > 0))
            {
                m_useCount--;
                ExecuteUseSkill();
                StopAllCoroutines();
                StartCoroutine(CorChargeSkill());
            }
        }

        public void StopSkill(bool shouldResetCooldown) {
            if (shouldResetCooldown)
            {
                StopAllCoroutines();
                m_isInCooldown = false;
            }
            else
            {
                m_tempStopRepeatingSkill = true;
            }

            AnimateSkill(false);
        }

        private IEnumerator CorChargeSkill() {
            m_isInCooldown = true;

            AnimateSkill(true);
            yield return new WaitForSeconds(m_useInterval);
            AnimateSkill(false);

            yield return new WaitForSeconds(m_cooldown);
            m_isInCooldown = false;

            if (m_isRepeating && !m_tempStopRepeatingSkill) {
                UseSkill();
            }
        }

        private void AnimateSkill(bool shouldAnimate) {
            m_compAnimator.SetBool(m_animSkill, shouldAnimate);
        }
        
    }

}