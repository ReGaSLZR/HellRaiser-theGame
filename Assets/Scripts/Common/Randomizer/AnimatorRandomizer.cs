﻿namespace Common.Randomizer
{

    using UnityEngine;
    using Utils;

    [RequireComponent(typeof(Animator))]
    public class AnimatorRandomizer : MonoBehaviour
    {

        private Animator m_compAnimator;

        [SerializeField] private RuntimeAnimatorController[] m_controllers;

        private void Awake()
        {
            m_compAnimator = GetComponent<Animator>();

            if (m_controllers.Length == 0)
            {
                LogUtil.PrintInfo(this, GetType(),
                    "Awake(): No controllers defined. Destroying...");
                Destroy(this);
            }
        }

        private void Start()
        {
            m_compAnimator.runtimeAnimatorController =
                m_controllers[Random.Range(0, m_controllers.Length)];
            Destroy(this);
        }

    }

}