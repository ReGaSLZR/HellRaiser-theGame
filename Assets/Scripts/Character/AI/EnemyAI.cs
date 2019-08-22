using Character.AI;
using GamePlay.Base;
using NaughtyAttributes;
using UnityEngine;

namespace GamePlay.Stats {

    public class EnemyAI : BaseAI
    {

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