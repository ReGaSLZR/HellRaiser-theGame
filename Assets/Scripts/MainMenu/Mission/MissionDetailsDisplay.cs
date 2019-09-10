using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Scriptables;
using Zenject;
using static Scriptables.PlaySettings;
using NaughtyAttributes;
using UniRx;
using Audio;

namespace MainMenu.Mission {

    public class MissionDetailsDisplay : MonoBehaviour
    {

        //INJECTABLES
        [Inject]
        private readonly AudioModel.SFXSetter m_modelSFX;
        [Inject]
        private readonly AudioTheme m_audioTheme;
        [Inject]
        private readonly ColorScheme m_colorScheme;

        [SerializeField]
        [Required]
        private Image m_panelMissionDetails;

        [SerializeField]
        [Required]
        private Button m_buttonCloseDetails;

        [Header("Mission Details - Variables UI Holder")]

        [SerializeField]
        [Required]
        private RawImage m_rawImageLocationOnMap;

        [SerializeField]
        [Required]
        private RawImage m_rawImagePreview;

        [Space]

        [SerializeField]
        [Required]
        private TextMeshProUGUI m_textTitle;

        [SerializeField]
        [Required]
        private TextMeshProUGUI m_textMissionType;

        [SerializeField]
        [Required]
        private TextMeshProUGUI m_textObjective;

        [SerializeField]
        [Required]
        private TextMeshProUGUI m_textDescription;

        [Space]

        [SerializeField]
        [Required]
        private TextMeshProUGUI m_textMainLevelsRequired;

        [SerializeField]
        [Required]
        private TextMeshProUGUI m_textSideLevelsRequired;

        private void Start()
        {
            m_buttonCloseDetails.OnClickAsObservable()
                .Subscribe(_ => {
                    m_modelSFX.PlaySFX(m_audioTheme.m_sfxButtonClick);
                    m_panelMissionDetails.gameObject.SetActive(false);
                })
                .AddTo(this);

            m_panelMissionDetails.gameObject.SetActive(false);
        }

        public void Display(MissionInfo mission, bool isUnlocked) {
            m_panelMissionDetails.color = (mission.IsMainMission()) ?
                m_colorScheme.m_missionMainPanel : m_colorScheme.m_missionSidePanel;

            //set texts
            m_textTitle.text = mission.m_title;
            m_textMissionType.text = mission.m_missionType;

            m_textObjective.text = isUnlocked ? mission.m_objective : " - ";
            m_textDescription.text = isUnlocked ? mission.m_description : " - ";
            
            m_textMainLevelsRequired.text = mission.m_reqMainLevelsCleared.ToString();
            m_textSideLevelsRequired.text = mission.m_reqSideLevelsCleared.ToString();

            //set images
            m_rawImagePreview.texture = mission.m_preview;
            m_rawImageLocationOnMap.texture = mission.m_map;

            //activate panel
            if (!m_panelMissionDetails.gameObject.activeInHierarchy) {
                m_panelMissionDetails.gameObject.SetActive(true);
            }

        }

    }

}