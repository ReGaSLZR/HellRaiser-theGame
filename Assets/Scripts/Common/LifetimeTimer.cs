using UnityEngine;

namespace Common
{

    public class LifetimeTimer : MonoBehaviour
    {

        [SerializeField]
        [Range(0.25f, 30f)]
        private float m_lifetime = 1f;

        private void Start()
        {
            Destroy(gameObject, m_lifetime);
        }

    }

}
