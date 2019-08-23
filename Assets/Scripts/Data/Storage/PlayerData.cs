using UnityEngine;

namespace Data.Storage {

    //TODO: move saving repository from PlayerPrefs to RemoteSettings..?
    public class PlayerData
    {

        private const string KEY_INVENTORY_MONEY = "KEY_INVENTORY_MONEY";
        private const string KEY_INVENTORY_SCROLL = "KEY_INVENTORY_SCROLL";

        public static int GetInventoryMoney() {
            return PlayerPrefs.GetInt(KEY_INVENTORY_MONEY, 0);
        }

        public static int GetInventoryScroll() {
            return PlayerPrefs.GetInt(KEY_INVENTORY_SCROLL, 0);
        }

        public static void SaveInventory(int money, int scroll) {
            PlayerPrefs.SetInt(KEY_INVENTORY_MONEY, money);
            PlayerPrefs.SetInt(KEY_INVENTORY_SCROLL, scroll);
            PlayerPrefs.Save();
        }

    }


}