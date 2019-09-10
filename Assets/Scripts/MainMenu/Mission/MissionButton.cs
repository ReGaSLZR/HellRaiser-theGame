using UnityEngine;
using UnityEngine.UI;
using Scriptables;
using Zenject;
using static Scriptables.PlaySettings;
using Audio;
using UniRx;
using TMPro;
using NaughtyAttributes;
using Data.Storage;

namespace MainMenu.Mission {

    public class MissionButton : MonoBehaviour
    {

        [Inject]
        private readonly ColorScheme m_colorScheme;
        [Inject]
        private readonly AudioTheme m_audioTheme;
        [Inject]
        private readonly AudioModel.SFXSetter m_modelSFX;
        [Inject]
        private readonly MissionModel.Getter m_modelMissionGetter;
        [Inject]
        private readonly MissionModel.Setter m_modelMissionSetter;

        [SerializeField]
        [Required]
        private Button m_button;

        [SerializeField]
        [Required]
        private TextMeshProUGUI m_textButton;

        private MissionInfo m_mission;

        private void Start()
        {
            m_button.OnClickAsObservable()
                .Subscribe(_ => {
                    m_modelSFX.PlaySFX(m_audioTheme.m_sfxButtonClick);
                    m_modelMissionSetter.DisplayMission(m_mission);
                })
                .AddTo(this);
        }

        public void Hide() {
            m_button.gameObject.SetActive(false);
        }

        public void SetMission(MissionInfo mission) {
            m_mission = mission;

            SetUpButtonColor();
            SetUpTitle();

            m_button.gameObject.SetActive(true);
        }

        private void SetUpButtonColor() {
            Color buttonColor = m_mission.IsMainMission() ?
                m_colorScheme.m_missionMainPanel : m_colorScheme.m_missionSidePanel;

            buttonColor.a = m_modelMissionGetter.IsMissionUnlocked(m_mission) ? 0.5f : 1f;

            m_button.image.color = buttonColor;
        }

        private void SetUpTitle() {
            string prefix = m_mission.IsMainMission() ? "M" : "S";

            m_textButton.text =
                prefix + (m_mission.m_buildIndex - SceneData.SCENE_MAIN_MENU) + " - "
                + m_mission.m_title; //sample: "S1 - Tutorial"
        }

    }


}