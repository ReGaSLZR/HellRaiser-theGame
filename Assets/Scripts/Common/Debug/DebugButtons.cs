using UnityEngine;
using NaughtyAttributes;
using System.IO;
using Data.Storage;

namespace Common.Debug {

    public class DebugButtons : MonoBehaviour
    {

        [Button]
        public void ClearPlayerPrefs() {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        [Button]
        public void ClearBinaryFiles() {
            File.Delete(PlayerData.Inventory.SAVE_PATH);
            File.Delete(PlayerData.MissionProgression.SAVE_PATH);
            File.Delete(PlayerData.Checkpoint.SAVE_PATH);
        }

    }

}