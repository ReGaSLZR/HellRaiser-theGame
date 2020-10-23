using UnityEngine;
using System.Collections;
using TMPro;
using NaughtyAttributes;
using Utils;

namespace Loading {

    public class LoadingTextAnimator : MonoBehaviour
    {

        [SerializeField]
        [Required]
        private TextMeshProUGUI m_textLoading;

        [SerializeField]
        private string[] m_textContentToLoop;

        [SerializeField]
        private float m_loopDelay = 0.5f;

        private void Awake()
        {
            if (m_textContentToLoop.Length == 0) {
                LogUtil.PrintWarning(gameObject, GetType(), "Awake(): There's no text content to loop.");
                Destroy(this);
            }
        }

        private void OnEnable()
        {
            StartCoroutine(CorLoopContents());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator CorLoopContents() {
            while (true) {
                for (int x = 0; x < m_textContentToLoop.Length; x++) {
                    m_textLoading.text = m_textContentToLoop[x];
                    yield return new WaitForSeconds(m_loopDelay);
                }
            }
        }

    }

}