namespace Pooling
{

    using UnityEngine;
    using Utils;
    using System.Collections;

    public class ObjectInPool : MonoBehaviour
    {

        private int m_index;
        private ObjectPooler m_pooler;

        public void SetUp(int index, ObjectPooler pooler)
        {
            m_index = index;
            m_pooler = pooler;
        }

        public void PutBackToPool()
        {
            if (m_pooler == null)
            {
                LogUtil.PrintWarning(GetType(), "PutBackToPool():" +
                    " Missing ObjectPooler. Did you forget to call SetUp()?");
            }
            else
            {
                m_pooler.RecycleInPool(m_index);
            }
            
        }

        public void PutBackToPool(float delay)
        {
            StartCoroutine(CorBackToPool(delay));
        }

        private IEnumerator CorBackToPool(float delay)
        {
            yield return new WaitForSeconds(delay);
            PutBackToPool();
        }

    }

}