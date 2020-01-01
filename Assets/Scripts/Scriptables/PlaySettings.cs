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
            public Color m_damageNull;

            [Space]

            public Color m_staminaGain;
            public Color m_staminaLoss;

            [Space]

            public Color m_moneyGain;
            public Color m_moneyLoss;

            [Space]

            public Color m_time;

            [Space]

            public Color m_missionMainPanel;
            public Color m_missionSidePanel;

        } //end of ColorScheme class

        [System.Serializable]
        public class AudioTheme {

            [Header("Sound FXs")]

            public AudioClip m_sfxButtonClick;

            [Space]

            public AudioClip m_sfxMissionUpdate;

            [Header("Background Music")]

            public AudioClip m_bgmMissionAccomplished;
            public AudioClip m_bgmMissionFailure;

        }//end of AudioTheme class

        public InputType m_gamePlayInput;

        public ColorScheme m_colorScheme;

        public AudioTheme m_audioTheme;

        //TODO code more play settings variables and usage

    }


}