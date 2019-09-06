﻿using GamePlay.Base;
using NaughtyAttributes;
using UnityEngine;

namespace Character.AI {

    public class EnemyAI : BaseAI
    {

        [Space]

        [SerializeField]
        [Required]
        private BaseTrigger m_chainedTriggerOnDeath;

        protected override void OnDeath()
        {
            m_chainedTriggerOnDeath.Execute();
            base.OnDeath();
        }

    }


}