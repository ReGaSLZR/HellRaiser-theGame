using UnityEngine;
using UnityEngine.SceneManagement;

namespace Data.Storage {

    /// <summary>
    /// NOTE: All constant integer values here should reflect what's on the build settings.
    /// </summary>
    public class SceneData
    {

        private const string KEY_LEVEL_TO_LOAD = "KEY_LEVEL_TO_LOAD";

        public const int SCENE_SPLASH = 0;
        public const int SCENE_LOADING = 1;
        public const int SCENE_MAIN_MENU = 2;
        public const int SCENE_MISSION_FIRST = 3;

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