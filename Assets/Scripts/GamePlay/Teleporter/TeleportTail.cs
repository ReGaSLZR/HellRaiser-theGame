using NaughtyAttributes;
using System.Collections;
using UnityEngine;

namespace GamePlay.Teleport
{

    public class TeleportTail : MonoBehaviour
    {

        [Required]
        [SerializeField] private Transform m_teleportFX;

        [Range(0.25f, 3f)]
        [SerializeField] private float m_fXDuration = 0.25f;

        private void Awake()
        {
            m_teleportFX.gameObject.SetActive(false);
        }

        private IEnumerator CorShowFX()
        {
            m_teleportFX.gameObject.SetActive(true);
            yield return new WaitForSeconds(m_fXDuration);
            m_teleportFX.gameObject.SetActive(false);
        }

        public void ShowTeleportFX()
        {
            StopAllCoroutines();
            StartCoroutine(CorShowFX());
        }

    }

}