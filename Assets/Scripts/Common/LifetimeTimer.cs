using UnityEngine;
using Utils;
using NaughtyAttributes;
using Pooling;

namespace Common
{

    public class LifetimeTimer : MonoBehaviour
    {

        [SerializeField]
        [Range(0.25f, 30f)]
        private float m_lifetime = 1f;

        [SerializeField]
        private bool m_isPooled;

        [SerializeField]
        [EnableIf("m_isPooled")]
        private ObjectInPool m_poolItemReference;

        private void Awake()
        {
            if (m_isPooled && (m_poolItemReference == null))
            {
                LogUtil.PrintError(this, GetType(), "Awake(): " +
                    "No ObjectInPool reference. Switching to default destroy.");
                m_isPooled = false;
            }
        }

        private void OnEnable()
        {
            if (m_isPooled)
            {
                m_poolItemReference.PutBackToPool(m_lifetime);
            }

            else {
                Destroy(gameObject, m_lifetime);
            }
            
        }

    }

}
