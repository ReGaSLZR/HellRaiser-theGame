using Character.Stats;
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
        
        [Space]

        [SerializeField]
        [Range(0f, 5f)]
        protected float m_skillDelay = 0f;
        [SerializeField]
        [Range(0.25f, 5f)]
        protected float m_skillDuration = 0.25f;
        [SerializeField]
        [Range(0.25f, 10f)]
        protected float m_skillCooldown = 0.25f;

        [Space]

        [SerializeField]
        protected bool m_isRepeating;

        [Space]

        [SerializeField]
        protected Transform m_childFX;

        private bool m_tempStopRepeatingSkill;
        private bool m_isInCooldown;

        protected StatOffense m_statOffense;

        protected abstract void ExecuteUseSkill();

        protected virtual void Awake() {
            m_compAnimator = GetComponent<Animator>();
            SetChildFXActive(false);

            if (m_childFX != null) {
                PassStatOffenseTo(m_childFX.gameObject);
            }
        }

        public void SetStatOffense(StatOffense statOffense)
        {
            m_statOffense = statOffense;
        }

        public void UseSkill() {
            m_tempStopRepeatingSkill = false;

            if (!m_isInCooldown)
            {
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

            yield return new WaitForSeconds(m_skillDelay);

            ExecuteUseSkill();
            SetChildFXActive(true);
            AnimateSkill(true);

            yield return new WaitForSeconds(m_skillDuration);

            SetChildFXActive(false);
            AnimateSkill(false);

            yield return new WaitForSeconds(m_skillCooldown);
            m_isInCooldown = false;

            if (m_isRepeating && !m_tempStopRepeatingSkill) {
                UseSkill();
            }
        }

        private void AnimateSkill(bool shouldAnimate) {
            if (m_compAnimator.runtimeAnimatorController == null) {
                return;
            }

            m_compAnimator.SetBool(m_animSkill, shouldAnimate);
        }

        private void SetChildFXActive(bool isActive) {
            if (m_childFX != null) {
                m_childFX.gameObject.SetActive(isActive);
            }
        }

        protected void PassStatOffenseTo(GameObject statReceiver) {
            if (statReceiver != null)
            {
                BaseSkill childSkill = statReceiver.GetComponent<BaseSkill>();
                if (childSkill != null)
                {
                    childSkill.SetStatOffense(m_statOffense);
                }
            }
        }
        
    }

}