using UnityEngine;
using UnityEngine.SceneManagement;

namespace Data.Storage {

    public class SceneData
    {

        private const string KEY_LEVEL_TO_LOAD = "KEY_LEVEL_TO_LOAD";

        public const int SCENE_SPLASH = 0;
        public const int SCENE_LOADING = 1;
        public const int SCENE_MAIN_MENU = 2;

        public static void LoadLevel(int levelIndex) {
            SceneManager.LoadSceneAsync(levelIndex);
        }

        public static void LoadStoredLevel() {
            SceneManager.LoadSceneAsync(PlayerPrefs.GetInt(KEY_LEVEL_TO_LOAD));
        }

        public static void StoreLevelThenLoad(int levelIndex) {
            if (levelIndex > SCENE_LOADING) {
                PlayerPrefs.SetInt(KEY_LEVEL_TO_LOAD, levelIndex);
                PlayerPrefs.Save();

                SceneManager.LoadSceneAsync(SCENE_LOADING);
            }
        }

        public static int GetCurrentSceneIndex()
        {
            return SceneManager.GetActiveScene().buildIndex;
        }

    }

}