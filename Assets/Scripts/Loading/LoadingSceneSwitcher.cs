using UnityEngine;
using Data.Storage;
using Utils;

namespace Loading {

    public class LoadingSceneSwitcher : MonoBehaviour
    {

        public enum SwitchMode
        {
            FORCE_SPLASH,
            FORCE_MAIN_MENU,
            STORED_LEVEL
        }

        [SerializeField]
        private SwitchMode m_loadOptions;

        [SerializeField]
        private bool m_shouldClearStoredLevel;

        private void Awake()
        {
            if (m_shouldClearStoredLevel)
            {
                SceneData.ClearStoredLevel();
            }
        }

        private void Start()
        {
            switch (m_loadOptions)
            {
                case SwitchMode.FORCE_MAIN_MENU:
                {
                        SceneData.LoadLevel(SceneData.SCENE_MAIN_MENU);
                        break;
                }
                case SwitchMode.STORED_LEVEL:
                {
                        SceneData.LoadStoredLevel();
                        break;
                }
                case SwitchMode.FORCE_SPLASH:
                {
                        SceneData.LoadLevel(SceneData.SCENE_SPLASH);
                        break;
                }
                default:
                    { 
                        LogUtil.PrintWarning(gameObject, GetType(), "Could not find level of your choice to load.");
                        break;
                }
            }
            
        }

    }

}