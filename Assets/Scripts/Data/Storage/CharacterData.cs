using UnityEngine;

namespace Data.Storage {

    public class CharacterData : MonoBehaviour
    {

        private const string KEY_CHARACTER_LEVEL = "KEY_CHARACTER_LEVEL-";
        private const string KEY_CHARACTER_HEALTH = "KEY_CHARACTER_HEALTH-";
        private const string KEY_CHARACTER_STAMINA = "KEY_CHARACTER_STAMINA-";

        private static string m_currentCharacterName;

        private static string GetKey(string key) {
            return key + m_currentCharacterName;
        }

        public static string GetCharacterName() {
            return m_currentCharacterName;
        }

        public static int GetCharacterLevel()
        {
            return PlayerPrefs.GetInt(GetKey(KEY_CHARACTER_LEVEL), 1);
        }

        public static int GetCharacterHealth() {
            return PlayerPrefs.GetInt(GetKey(KEY_CHARACTER_HEALTH), Scriptables.CharacterInfo.HEALTH_MAX);
        }

        public static int GetCharacterStamina() {
            return PlayerPrefs.GetInt(GetKey(KEY_CHARACTER_STAMINA), Scriptables.CharacterInfo.STAMINA_MAX);
        }

        public static void SetCharacterName(string name)
        {
            m_currentCharacterName = name;
        }

        public static void LevelUpCharacter() {
            PlayerPrefs.SetInt(GetKey(KEY_CHARACTER_LEVEL), (GetCharacterLevel()+1));
            PlayerPrefs.Save();
        }

        public static void SaveCharacterStats(int health, int stamina) {
            PlayerPrefs.SetInt(GetKey(KEY_CHARACTER_HEALTH), health);
            PlayerPrefs.SetInt(GetKey(KEY_CHARACTER_STAMINA), stamina);
            PlayerPrefs.Save();
        }

    }
   
}