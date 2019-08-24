using GamePlay.Base;
using UnityEngine;

namespace GamePlay.Playable
{

    public class PlayablesTeleportTrigger : BaseTrigger
    {
        private PlayablesManager m_playablesManager;

        [SerializeField]
        private Transform m_teleportLocation;

        private void Awake()
        {
            m_playablesManager = GameObject.FindObjectOfType<PlayablesManager>();

            if (m_playablesManager == null) {
                LogUtil.PrintError(gameObject, GetType(), "No PlayablesManager found. Destroying...");
                Destroy(this);
            }
        }

        public override void Execute()
        {
            m_playablesManager.MassTeleportPlayables(m_teleportLocation);
            Destroy(gameObject);
        }
        
    }


}