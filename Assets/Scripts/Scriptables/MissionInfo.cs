using NaughtyAttributes;
using UnityEngine;

namespace Scriptables {

    [CreateAssetMenu(fileName = "New MissionInfo", menuName = "HellRaiser/Create MissionInfo")]
    public class MissionInfo : ScriptableObject
    {

        [TextArea]
        public string m_objective;

        public bool m_isWithKey;

        [EnableIf("m_isWithKey")]
        [Range(1, 10)]
        public int m_keyCount = 1;

        public bool m_isTimed;

        [EnableIf("m_isTimed")]
        [Range(1, 1000)]
        public int m_timeLimit = 180;
       
    }

}