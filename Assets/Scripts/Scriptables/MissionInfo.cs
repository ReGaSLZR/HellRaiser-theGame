using NaughtyAttributes;
using UnityEngine;
using Data.Storage;

namespace Scriptables {

    [CreateAssetMenu(fileName = "New MissionInfo", menuName = "HellRaiser/Create MissionInfo")]
    public class MissionInfo : ScriptableObject
    {

        public enum MissionType
        {
            MAIN,
            SIDE
        }

        [Range(SceneData.SCENE_MISSION_FIRST, 50)]
        public int m_buildIndex = SceneData.SCENE_MISSION_FIRST;

        public Texture2D m_preview;

        public Texture2D m_map;

        [Space]

        public string m_title;

        [TextArea]
        public string m_objective;

        [TextArea]
        public string m_description;

        [Space]

        public MissionType m_missionType;

        [Tooltip("Refers to the previous levels' latest build index.")]
        [Range(0, 50)]
        public int m_lastMainCleared = SceneData.SCENE_MISSION_FIRST;

        [Tooltip("Refers to the previous levels' latest build index.")]
        [Range(0, 50)]
        public int m_lastSideCleared = SceneData.SCENE_MISSION_FIRST;

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
            return MissionType.MAIN == m_missionType;
        }
       
    }

}