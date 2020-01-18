using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Utils;

namespace Data.Storage {

    public static class PlayerData
    {

        #region Serializable Classes

        [System.Serializable]
        public class Inventory {
            public static readonly string SAVE_PATH
                = Application.persistentDataPath + "/hellraiser-inventory.hellishfun";

            public int m_money;

            public Inventory(int money) {
                m_money = money;
            }
        }

        [System.Serializable]
        public class MissionProgression {
            public static readonly string SAVE_PATH
                = Application.persistentDataPath + "/hellraiser-missions.hellishfun";

            public int m_mainCleared;
            public int m_sideCleared;

            public MissionProgression(int mainCleared, int sideCleared) {
                m_mainCleared = mainCleared;
                m_sideCleared = sideCleared;
            }

        }

        [System.Serializable]
        public class Checkpoint
        {
            public static readonly string SAVE_PATH
                = Application.persistentDataPath + "/hellraiser-checkpoint.hellishfun";

            public int m_missionIndex;
            public int m_checkpointID;

            public float m_spawnPointX;
            public float m_spawnPointY;
            public float m_spawnPointZ;

            public Checkpoint(int missionIndex, int checkpointID,
                Vector3 spawnPoint)
            {
                m_missionIndex = missionIndex;
                m_checkpointID = checkpointID;

                m_spawnPointX = spawnPoint.x;
                m_spawnPointY = spawnPoint.y;
                m_spawnPointZ = spawnPoint.z;
            }

            public Vector3 GetSpawnPoint()
            {
                return new Vector3(m_spawnPointX, m_spawnPointY, m_spawnPointZ);
            }

        }

        #endregion

        private static void ExecuteSave(string path, object data) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

            formatter.Serialize(stream, data);
            stream.Close();
        }

        private static object ExecuteLoad(string path) {
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                object data = formatter.Deserialize(stream);

                stream.Close();
                LogUtil.PrintInfo("PlayerData.ExecuteLoad(): Save file from " + path + " successfully loaded.");
                return data;
            }
            else
            {
                LogUtil.PrintWarning("PlayerData.ExecuteLoad(): Could not load " + path + " save file.");
                return null;
            }
        }

        public static void Save(Inventory data)
        {
            ExecuteSave(Inventory.SAVE_PATH, data);
        }

        public static void Save(MissionProgression data)
        {
            ExecuteSave(MissionProgression.SAVE_PATH, data);
        }

        public static void Save(Checkpoint data)
        {
            ExecuteSave(Checkpoint.SAVE_PATH, data);
        }

        public static void Clear(string savePath)
        {
            File.Delete(savePath);
        }

        public static Inventory LoadInventory() {
            object loadedData = ExecuteLoad(Inventory.SAVE_PATH);

            if (loadedData == null)
            {
                LogUtil.PrintWarning("PlayerData.LoadInventory(): returning blank data instead.");
                return new Inventory(0);
            }
            
            return loadedData as Inventory;
                
        }

        public static MissionProgression LoadMissionProgression() {
            object loadedData = ExecuteLoad(MissionProgression.SAVE_PATH);

            if (loadedData == null)
            {
                LogUtil.PrintWarning("PlayerData.LoadMissionProgression(): returning blank data instead.");
                return new MissionProgression(0, 0);
            }
            
            return loadedData as MissionProgression;
        }

        public static Checkpoint LoadCheckpoint()
        {
            object loadedData = ExecuteLoad(Checkpoint.SAVE_PATH);

            if (loadedData == null)
            {
                LogUtil.PrintWarning("PlayerData.LoadCheckpoint(): returning blank data instead.");
                return new Checkpoint(0, 0, Vector3.zero);
            }

            return loadedData as Checkpoint;
        }

    }

}