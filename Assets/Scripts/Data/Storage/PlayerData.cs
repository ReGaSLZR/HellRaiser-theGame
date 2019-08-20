using UnityEngine;

namespace Data.Storage {

    public class PlayerData
    {

        private const string KEY_ALLIANCE_LEVEL = "KEY_ALLIANCE_LEVEL";
        private const string KEY_EXPERIENCE = "KEY_EXPERIENCE";

        private const string KEY_INVENTORY_MONEY = "KEY_INVENTORY_MONEY";
        private const string KEY_INVENTORY_FOOD = "KEY_INVENTORY_FOOD";

        public static int GetAllianceLevel() {
            return PlayerPrefs.GetInt(KEY_ALLIANCE_LEVEL, 1);
        }

        public static int GetExperience() {
            return PlayerPrefs.GetInt(KEY_EXPERIENCE, 0);
        }

        public static int GetInventoryMoney() {
            return PlayerPrefs.GetInt(KEY_INVENTORY_MONEY, 0);
        }

        public static int GetInventoryFood() {
            return PlayerPrefs.GetInt(KEY_INVENTORY_FOOD, 0);
        }

        public static void SaveExperience(int experience) {
            PlayerPrefs.SetInt(KEY_EXPERIENCE, experience);
            PlayerPrefs.Save();
        }

        public static void LevelUpAlliance() {
            PlayerPrefs.SetInt(KEY_ALLIANCE_LEVEL, (GetAllianceLevel()+1));
            PlayerPrefs.Save();
        }

        public static void SaveInventory(int money, int food) {
            PlayerPrefs.SetInt(KEY_INVENTORY_MONEY, money);
            PlayerPrefs.SetInt(KEY_INVENTORY_FOOD, food);
            PlayerPrefs.Save();
        }

    }


}