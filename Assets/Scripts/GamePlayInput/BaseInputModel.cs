using UniRx;
using UnityEngine;

namespace GamePlayInput {

    public abstract class BaseInputModel : MonoBehaviour
    {

        [SerializeField]
        public InputType m_inputType { protected set; get; }

        [SerializeField]
        protected float movementBaseSpeed = 0.1f;

        public bool m_jump { protected set; get; }
        public float m_run { protected set; get; }

        public bool m_skillMain { protected set; get; }
        public bool m_skill2 { protected set; get; }
        public bool m_skill3 { protected set; get; }

        public bool m_isEnabled { protected set; get; }

        //public const int MAX_SKILL_GAUGE = 100;
        //public const int SKILL_1_COST = 0;
        //public const int SKILL_2_COST = MAX_SKILL_GAUGE/2;
        //public const int SKILL_3_COST = MAX_SKILL_GAUGE;

        //public ReactiveProperty<int> m_skillGauge { protected set; get; }

        protected abstract void SetInputType();

        private void Awake()
        {
            //m_skillGauge = new ReactiveProperty<int>(MAX_SKILL_GAUGE);
            m_isEnabled = true;
            SetInputType();
        }

        public void DisableControls()
        {
            LogUtil.PrintWarning(gameObject, GetType(), "Disabling Controls...");
            m_isEnabled = false;
            m_run = 0f;
        }

        public void EnableControls()
        {
            LogUtil.PrintInfo(gameObject, GetType(), "Enabling Controls...");
            m_isEnabled = true;
        }

    }

}