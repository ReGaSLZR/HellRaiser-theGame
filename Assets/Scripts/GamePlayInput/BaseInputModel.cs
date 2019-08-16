﻿using UnityEngine;

namespace GamePlayInput {

    public abstract class BaseInputModel : MonoBehaviour
    {

        [SerializeField]
        public InputType m_inputType { protected set; get; }

        [SerializeField]
        protected float movementBaseSpeed = 0.1f;

        public bool m_jump { protected set; get; }
        public float m_run { protected set; get; }

        public bool m_attack { protected set; get; }
        public bool m_skill1 { protected set; get; }
        public bool m_skill2 { protected set; get; }

        public bool m_isEnabled { protected set; get; }

        protected abstract void SetInputType();

        private void Awake()
        {
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