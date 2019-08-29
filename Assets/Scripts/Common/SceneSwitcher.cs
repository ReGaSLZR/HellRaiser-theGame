using UnityEngine;
using Data.Storage;
using NaughtyAttributes;
using Utils;

namespace Common {

    public class SceneSwitcher : MonoBehaviour
    {

        private const string MAIN_MENU = "MAIN_MENU";
        private const string STORED_LEVEL = "STORED_LEVEL";
        private readonly string[] m_dropdownOptionns = new string[] {
            "<Unset>",
            MAIN_MENU,
            STORED_LEVEL
        };

        [SerializeField]
        [Dropdown("m_dropdownOptionns")]
        private string m_loadOptions;

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
            else {
                LogUtil.PrintWarning(gameObject, GetType(), "Could not find level of your choice to load.");
            }
            
        }

    }

}