using UnityEngine;
using Data.Storage;
using NaughtyAttributes;
using Utils;

namespace Loading {

    public class LoadingSceneSwitcher : MonoBehaviour
    {

        private const string SPLASH = "SPLASH";
        private const string MAIN_MENU = "MAIN_MENU";
        private const string STORED_LEVEL = "STORED_LEVEL";
        private readonly string[] m_dropdownOptionns = new string[] {
            "<Unset>",
            SPLASH,
            MAIN_MENU,
            STORED_LEVEL,
        };

        [SerializeField]
        [Dropdown("m_dropdownOptionns")]
        private string m_loadOptions;

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
            if (MAIN_MENU.Equals(m_loadOptions))
            {
                SceneData.LoadLevel(SceneData.SCENE_MAIN_MENU);
            }
            else if (STORED_LEVEL.Equals(m_loadOptions))
            {
                SceneData.LoadStoredLevel();
            }
            else if (SPLASH.Equals(m_loadOptions))
            {
                SceneData.LoadLevel(SceneData.SCENE_SPLASH);
            }
            else {
                LogUtil.PrintWarning(gameObject, GetType(), "Could not find level of your choice to load.");
            }
            
        }

    }

}