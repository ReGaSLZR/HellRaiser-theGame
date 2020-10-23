using UnityEngine;

namespace Injection {

    public interface Instantiator
    {
        void InjectPrefab(GameObject prefab);
        GameObject InstantiateInjectPrefab(GameObject prefab, Transform parent);
    }

}