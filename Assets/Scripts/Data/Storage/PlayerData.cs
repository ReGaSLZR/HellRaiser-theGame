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
            public static readonly string SAVE_PATH = Application.persistentDataPath + "/hellraiser-inventory.hellishfun";

            public int m_money;

            public Inventory(int money) {
                m_money = money;
            }
        }

        [System.Serializable]
        public class MissionProgression {
            public static readonly string SAVE_PATH = Application.persistentDataPath + "/hellraiser-missions.hellishfun";

            public int m_mainCleared;
            public int m_sideCleared;

            public MissionProgression(int mainCleared, int sideCleared) {
                m_mainCleared = mainCleared;
                m_sideCleared = sideCleared;
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

        public static Inventory LoadInventory() {
            object loadedData = ExecuteLoad(Inventory.SAVE_PATH);

            if (loadedData == null)
            {
                LogUtil.PrintWarning("PlayerData.LoadInventory(): returning blank data instead.");
                return new Inventory(0);
            }
            else
            {
                return loadedData as Inventory;
            }
            
        }

        public static MissionProgression LoadMissionProgression() {
            object loadedData = ExecuteLoad(MissionProgression.SAVE_PATH);

            if (loadedData == null)
            {
                LogUtil.PrintWarning("PlayerData.LoadMissionProgression(): returning blank data instead.");
                return new MissionProgression(0, 0);
            }
            else
            {
                return loadedData as MissionProgression;
            }
        }

    }

}