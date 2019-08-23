using GamePlay.Base;
using UnityEngine;

namespace GamePlay.Playable
{

    public class PlayablesTeleportTrigger : BaseTrigger
    {

        [SerializeField]
        private PlayablesManager m_playablesManager;

        [SerializeField]
        private Transform m_teleportLocation;

        public override void Execute()
        {
            m_playablesManager.MassTeleportPlayables(m_teleportLocation);
            Destroy(gameObject);
        }
        
    }


}