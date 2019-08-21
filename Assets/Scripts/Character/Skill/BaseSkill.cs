using Scriptables;
using System.Collections;
using UniRx;
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
        [Tooltip("Necessary for skills with 'scratch' animations. Leave BLANK if unused.")]
        protected string m_animTriggerStopper;
        
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

        protected StatOffense m_statOffense;

        public ReactiveProperty<bool> m_isExecutionFinished { private set; get; }

        private bool m_tempStopRepeatingSkill;

        protected abstract void ExecuteUseSkill();

        protected virtual void Awake() {
            m_compAnimator = GetComponent<Animator>();
            m_isExecutionFinished = new ReactiveProperty<bool>(true);
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

            if (m_isExecutionFinished.Value)
            {
                StopAllCoroutines();
                StartCoroutine(CorChargeSkill());
            }
        }

        public void StopSkill(bool shouldResetCooldown) {
            if (shouldResetCooldown)
            {
                StopAllCoroutines();
                m_isExecutionFinished.Value = true;
            }
            else
            {
                m_tempStopRepeatingSkill = true;
            }

            AnimateSkill(false);
        }

        private IEnumerator CorChargeSkill() {
            m_isExecutionFinished.Value = false;

            yield return new WaitForSeconds(m_skillDelay);

            ExecuteUseSkill();
            SetChildFXActive(true);
            AnimateSkill(true);

            yield return new WaitForSeconds(m_skillDuration);

            OnSkillFinish();

            yield return new WaitForSeconds(m_skillCooldown);
            m_isExecutionFinished.Value = true;

            if (m_isRepeating && !m_tempStopRepeatingSkill) {
                UseSkill();
            }
        }

        protected virtual void OnSkillFinish() {
            SetChildFXActive(false);
            AnimateSkill(false);
        }

        private void AnimateSkill(bool shouldAnimate) {
            if (m_compAnimator.runtimeAnimatorController == null) {
                return;
            }

            m_compAnimator.SetBool(m_animSkill, shouldAnimate);

            if (!m_animTriggerStopper.Equals("")) {
                m_compAnimator.SetTrigger(m_animTriggerStopper);
            }
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