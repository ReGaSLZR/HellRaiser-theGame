using UnityEngine;
using NaughtyAttributes;

namespace Common.Debug {

    public class DebugButtons : MonoBehaviour
    {

        [Button]
        public void ClearPlayerPrefs() {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

    }

}