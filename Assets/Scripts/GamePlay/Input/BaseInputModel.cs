using GamePlay.Camera;
using UniRx;
using UnityEngine;
using Utils;

namespace GamePlay.Input {

    public abstract class BaseInputModel : MonoBehaviour
    {

        public InputType m_inputType { protected set; get; }

        protected float m_movementBaseSpeed = 1f;

        public bool m_jump { protected set; get; }
        public float m_run { protected set; get; }

        public bool m_skillMain { protected set; get; }
        public bool m_skill2 { protected set; get; } 
        public bool m_skill3 { protected set; get; } 

        public ReactiveProperty<bool> m_reactiveSkillMain_enabled { protected set; get; }
        public ReactiveProperty<bool> m_reactiveSkill2_enabled { protected set; get; }
        public ReactiveProperty<bool> m_reactiveSkill3_enabled { protected set; get; }

        /// <summary>
        /// Variable for handling Playable Character switching.
        /// </summary>
        public bool m_charChange { protected set; get; }

        public bool m_isEnabled { protected set; get; }

        public ReactiveProperty<CameraPanDirection> m_cameraPanDirection { protected set; get; }

        protected abstract void SetInputType();

        public BaseInputModel() {
            m_reactiveSkillMain_enabled = new ReactiveProperty<bool>(true);
            m_reactiveSkill2_enabled = new ReactiveProperty<bool>(true);
            m_reactiveSkill3_enabled = new ReactiveProperty<bool>(true);

            SetInputType();
        }

        private void Awake()
        {
            m_cameraPanDirection = new ReactiveProperty<CameraPanDirection>(CameraPanDirection.NO_PAN);
            m_isEnabled = true;
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

        public void SetSkillEnabled(int skillIndex, bool isEnabled)
        {
            ReactiveProperty<bool> property = (skillIndex == 0)
                ? m_reactiveSkillMain_enabled :
                    ((skillIndex == 1) ? m_reactiveSkill2_enabled :
                        m_reactiveSkill3_enabled);

            property.Value = isEnabled;
        }

    }

}