using UnityEngine;
using static Data.Storage.PlayerData;
using Data.Storage;
using Scriptables;
using NaughtyAttributes;

namespace MainMenu.Mission {


    public class MissionModel : MonoBehaviour, MissionModel.Getter, MissionModel.Setter
    {

        #region Interfaces
        public interface Getter {
            bool IsMissionUnlocked(MissionInfo mission);
        }

        public interface Setter {
            void DisplayMission(MissionInfo mission);
        }
        #endregion

        [SerializeField]
        [Required]
        private MissionDetailsDisplay m_detailsDisplay;

        [SerializeField]
        [Required]
        private MissionLoader m_loader;

        private MissionProgression m_missionProgression;

        private void Awake()
        {
            m_missionProgression = PlayerData.LoadMissionProgression();
        }

        public bool IsMissionUnlocked(MissionInfo mission)
        {
            return (mission.m_lastMainCleared <= m_missionProgression.m_mainCleared) &&
                (mission.m_lastSideCleared <= m_missionProgression.m_sideCleared);
        }

        public void DisplayMission(MissionInfo mission)
        {
            bool isUnlocked = IsMissionUnlocked(mission);

            m_detailsDisplay.Display(mission, isUnlocked);
            m_loader.SetMission(mission, isUnlocked);
        }

    }

}