using NaughtyAttributes;
using UnityEngine;
using Data.Storage;

namespace Scriptables {

    [CreateAssetMenu(fileName = "New MissionInfo", menuName = "HellRaiser/Create MissionInfo")]
    public class MissionInfo : ScriptableObject
    {

        private const string MISSION_TYPE_MAIN = "MAIN";
        private const string MISSION_TYPE_SIDE = "SIDE";
        private readonly string[] m_dropdownOptionsMissionType = new string[] {
            "<Unset>",
            MISSION_TYPE_MAIN,
            MISSION_TYPE_SIDE
        };

        [Range(SceneData.SCENE_MISSION_FIRST, 50)]
        public int m_buildIndex;

        public Texture2D m_preview;

        public Texture2D m_map;

        [Space]

        public string m_title;

        [TextArea]
        public string m_objective;

        [Multiline]
        public string m_description;

        [Space]

        [Dropdown("m_dropdownOptionsMissionType")]
        public string m_missionType;

        public int m_reqMainLevelsCleared = 0;

        public int m_reqSideLevelsCleared = 0;

        [Space]

        public bool m_isWithKey;

        [EnableIf("m_isWithKey")]
        [Range(1, 10)]
        public int m_keyCount = 1;

        public bool m_isTimed;

        //TODO change lower time limit range (for now, 1 is for debug)
        [EnableIf("m_isTimed")]
        [Range(1, 1000)]
        public int m_timeLimit = 180;

        [Space]

        public bool m_allCharactersMustSurvive;

        public bool IsMainMission() {
            return MISSION_TYPE_MAIN.Equals(m_missionType);
        }
       
    }

}