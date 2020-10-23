using Audio;
using NaughtyAttributes;
using Scriptables;
using System.Collections;
using UniRx;
using UnityEngine;

namespace Character.Skill {

    public class BaseSkill : MonoBehaviour
    {

        [Header("----- Base variables -----")]

        //COMPONENTS
        [SerializeField]
        [Required]
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
        [Range(0.25f, 8f)]
        protected float m_skillDuration = 0.25f;
        [SerializeField]
        [Range(0.25f, 10f)]
        protected float m_skillCooldown = 0.25f;

        [Space]

        [SerializeField]
        protected bool m_isRepeating;

        [Header("Audio Settings")]

        [SerializeField]
        [Required]
        protected AudioSource m_audioSource;

        [SerializeField]
        protected AudioClip[] m_clipSkillUse;

        [Space]

        [SerializeField]
        protected Transform m_childFX;

        protected StatOffense m_statOffense;
        protected ReactiveProperty<bool> m_isExecutionFinished = new ReactiveProperty<bool>(true);

        protected bool m_tempStopRepeatingSkill;

        protected virtual void Awake() {
            SetChildFXActive(false);

            if (m_childFX != null) {
                PassStatOffenseTo(m_childFX.gameObject);
            }
        }

        public ReactiveProperty<bool> IsExecutionFinished() {
            return m_isExecutionFinished;
        }

        public void SetStatOffense(StatOffense statOffense)
        {
            m_statOffense = statOffense;
        }

        public void SetSkillDuration(float duration) {
            m_skillDuration = duration;
        }

        public virtual void UseSkill() {
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

            //this is to alllow scratch animations
            //to play first uninterrupted (esp. with the use of triggers)
            AnimateSkill(true);

            AudioUtil.SafelyPlayRandom(m_audioSource, m_clipSkillUse);

            yield return new WaitForSeconds(m_skillDelay);

            OnSkillStart();

            yield return new WaitForSeconds(m_skillDuration);

            OnSkillFinish();

            yield return new WaitForSeconds(m_skillCooldown);
            m_isExecutionFinished.Value = true;

            if (m_isRepeating && !m_tempStopRepeatingSkill) {
                UseSkill();
            }
        }

        protected virtual void OnSkillStart()
        {
            SetChildFXActive(true);
        }

        protected virtual void OnSkillFinish() {
            SetChildFXActive(false);
            AnimateSkill(false);
        }

        private void AnimateSkill(bool shouldAnimate) {
            if (m_compAnimator == null || m_compAnimator.runtimeAnimatorController == null || m_animSkill.Equals("")) {
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