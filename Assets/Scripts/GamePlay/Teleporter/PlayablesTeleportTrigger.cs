using GamePlay.Base;
using System.Collections;
using UnityEngine;
using Utils;
using GamePlay.Playable;
using NaughtyAttributes;

namespace GamePlay.Teleport
{

    public class PlayablesTeleportTrigger : BaseTrigger
    {
        private PlayablesManager m_playablesManager;

        [SerializeField]
        [Required]
        private Transform m_teleportFX;

        [SerializeField]
        [Required]
        private TeleportTail m_destination;

        [Range(0f, 5f)]
        [SerializeField] private float m_executionDelay = 0f;

        [SerializeField]
        private bool m_isOneTime = true;

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
            StopAllCoroutines();
            StartCoroutine(CorExecuteTeleport());
        }

        private IEnumerator CorExecuteTeleport()
        {   
            m_teleportFX.gameObject.SetActive(true);
            m_destination.ShowTeleportFX();

            yield return new WaitForSeconds(m_executionDelay);

            m_playablesManager.MassTeleportPlayables(
                m_destination.gameObject.transform);

            m_teleportFX.gameObject.SetActive(false);

            if (m_isOneTime)
            {
                Destroy(gameObject);
            }
        }

    }


}