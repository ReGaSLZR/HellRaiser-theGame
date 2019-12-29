using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using GamePlay.Base;

namespace GamePlay.Teleport
{

    public class TeleportHeadTrigger : BaseTrigger
    {

        [Required]
        [SerializeField] private Transform m_teleportFX;

        [TagSelector]
        [SerializeField] private string[] m_targetTags;

        [Space]

        [Required]
        [SerializeField] private TeleportTail m_destination;

        [Range(0f, 5f)]
        [SerializeField] private float m_executionDelay = 0f;

        private void Awake()
        {
            m_teleportFX.gameObject.SetActive(false);
        }

        public override void Execute()
        {
            StopAllCoroutines();
            StartCoroutine(CorExecuteTeleport());
        }

        protected override bool IsTriggerable(GameObject collidedObject)
        {

            for (int x=0; x<m_targetTags.Length; x++)
            {
                if (collidedObject.tag.Equals(m_targetTags[x]))
                {
                    return true;
                }
            }

            return false; 
        }

        private IEnumerator CorExecuteTeleport()
        {
            m_triggerer.enabled = false;
            m_teleportFX.gameObject.SetActive(true);

            m_destination.ShowTeleportFX();

            yield return new WaitForSeconds(m_executionDelay);

            m_triggerer.transform.position =
                m_destination.gameObject.transform.position;

            m_teleportFX.gameObject.SetActive(false);
            m_triggerer.enabled = true;
        }

    }


}