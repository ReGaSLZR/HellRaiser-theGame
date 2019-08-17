using NaughtyAttributes;
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
        [Range(0.25f, 10f)]
        protected float m_useInterval = 0.25f;
        [SerializeField]
        protected bool m_isRepeating;

        protected abstract void ExecuteUseSkill();

        protected virtual void Awake() {
            m_compAnimator = GetComponent<Animator>();
        }

        public void UseSkill() {
            if (!m_compAnimator.GetBool(m_animSkill) && m_useCount > 0)
            {
                m_useCount--;
                ExecuteUseSkill();
                StopAllCoroutines();
                StartCoroutine(CorChargeSkill());
            }
        }

        public void StopSkill() {
            StopAllCoroutines();
            AnimateSkill(false);
        }

        private IEnumerator CorChargeSkill() {
            AnimateSkill(true);
            yield return new WaitForSeconds(m_useInterval);
            AnimateSkill(false);

            if (m_isRepeating) {
                UseSkill();
            }
        }

        private void AnimateSkill(bool shouldAnimate) {
            m_compAnimator.SetBool(m_animSkill, shouldAnimate);
        }
        
    }

}