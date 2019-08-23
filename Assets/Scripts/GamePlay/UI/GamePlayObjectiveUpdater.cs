using UnityEngine;
using UniRx;
using Zenject;
using GamePlay.Mission;
using TMPro;

namespace GamePlay.UI {

    public class GamePlayObjectiveUpdater : MonoBehaviour
    {

        [Inject]
        private readonly MissionModel.MissionGetter m_modelMission;

        [SerializeField]
        private TextMeshProUGUI[] m_textObjective;

        private void Start()
        {
            m_modelMission.GetMissionStatus()
                .Where(status => (MissionStatus.CLEARED != status))
                .Subscribe(status => {
                    UpdateObjective();
                })
                .AddTo(this);

            UpdateObjective();
        }

        private void UpdateObjective() {
            string objective = m_modelMission.GetMissionObjective();

            for (int x = 0; x < m_textObjective.Length; x++)
            {
                m_textObjective[x].text = objective;
            }
        }

    }


}