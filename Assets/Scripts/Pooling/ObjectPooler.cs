namespace Pooling
{

    using System.Collections.Generic;
    using Utils;
    using UnityEngine;

    public class ObjectPooler : MonoBehaviour
    {

        [SerializeField] private List<ObjectInPool> m_listObjectsInPool;

        private int m_itemIndex = 0;

        private void Awake()
        {
            DeactivateAllInPool();
        }

        private void OnDestroy()
        {
            m_listObjectsInPool.Clear();
        }

        private void DeactivateAllInPool()
        {
            for (int x = (m_listObjectsInPool.Count-1); x >= 0; x--)
            {
                ObjectInPool inPool = m_listObjectsInPool[x];

                if (inPool == null)
                {
                    m_listObjectsInPool.RemoveAt(x);
                }
                else
                {
                    inPool.SetUp(x, this);
                    inPool.gameObject.SetActive(false);
                }   
            }
        }

        private void MoveToNextIndex()
        {
            m_itemIndex++;

            if (m_itemIndex >= m_listObjectsInPool.Count)
            {
                m_itemIndex = 0;
            }
        }

        public void RecycleInPool(int index)
        {
            GameObject poolItem = m_listObjectsInPool[index].gameObject;
            poolItem.transform.position = Vector3.zero;
            poolItem.SetActive(false);
        }

        public GameObject GetObjectFromPool(Vector3 objectPosition,
            Quaternion objectRotation)
        {
            int checkCount = 0;

            while (m_listObjectsInPool[m_itemIndex].gameObject.activeInHierarchy)
            {
                MoveToNextIndex();
                checkCount++;

                if (checkCount >= m_listObjectsInPool.Count)
                {
                    LogUtil.PrintError(this, GetType(), "GetObjectFromPool():" +
                        " No available pooled item. Returning null.");
                    return null;
                }
            }

            GameObject itemFromPool = m_listObjectsInPool[m_itemIndex].gameObject;
            itemFromPool.transform.position = objectPosition;
            itemFromPool.transform.rotation = objectRotation;
            itemFromPool.SetActive(true);

            return itemFromPool;
        }

    }

}