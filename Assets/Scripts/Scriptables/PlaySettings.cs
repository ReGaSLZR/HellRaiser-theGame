using GamePlay.Input;
using UnityEngine;

namespace Scriptables {

    [CreateAssetMenu(fileName = "New PlaySettings", menuName = "HellRaiser/Create Settings")]
    public class PlaySettings : ScriptableObject
    {

        [System.Serializable]
        public class ColorScheme {

            public Color m_healthGain;
            public Color m_healthLoss;

            [Space]

            public Color m_staminaGain;
            public Color m_staminaLoss;

            [Space]

            public Color m_moneyGain;
            public Color m_moneyLoss;

            [Space]

            public Color m_time;

        } //end of ColorScheme class

        public InputType m_gamePlayInput;

        public ColorScheme m_colorScheme;

        //TODO code more play settings variables and usage

    }


}