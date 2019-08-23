using UnityEngine;

namespace Data.Storage {

    public class PlayerData
    {

        private const string KEY_INVENTORY_MONEY = "KEY_INVENTORY_MONEY";
        private const string KEY_INVENTORY_FOOD = "KEY_INVENTORY_FOOD";

        public static int GetInventoryMoney() {
            return PlayerPrefs.GetInt(KEY_INVENTORY_MONEY, 0);
        }

        public static int GetInventoryFood() {
            return PlayerPrefs.GetInt(KEY_INVENTORY_FOOD, 0);
        }

        public static void SaveInventory(int money, int food) {
            PlayerPrefs.SetInt(KEY_INVENTORY_MONEY, money);
            PlayerPrefs.SetInt(KEY_INVENTORY_FOOD, food);
            PlayerPrefs.Save();
        }

    }


}