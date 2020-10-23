using Audio;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using static Scriptables.PlaySettings;

namespace MainMenu.UI {

    public class MainMenuPanelSwitcher : MonoBehaviour
    {

        //INJECTABLES
        [Inject]
        private readonly AudioModel.SFXSetter m_modelSFX;
        [Inject]
        private readonly AudioTheme m_audioTheme;

        [Header("Panels")]

        [SerializeField]
        [Required]
        private Image m_panelHome;

        [SerializeField]
        [Required]
        private Image m_panelMissions;

        [SerializeField]
        [Required]
        private Image m_panelSettings;

        [SerializeField]
        [Required]
        private Image m_panelAbout;

        [Header("Buttons")]

        [SerializeField]
        private Button[] m_buttonsBackToHome;

        [SerializeField]
        [Required]
        private Button m_buttonMission;

        [SerializeField]
        [Required]
        private Button m_buttonSettings;

        [SerializeField]
        [Required]
        private Button m_buttonAbout;

        [Space]

        [SerializeField]
        [Required]
        private Button m_buttonExitGame;

        private void Start()
        {
            InitObservers();

            OnlyEnablePanel(m_panelHome);
        }

        private void InitObservers() {
            m_buttonExitGame.OnClickAsObservable()
                .Subscribe(_ => {
                        DisableAllButtons();
                        Application.Quit();
                    })
                .AddTo(this);

            SetButtonClickTriggerPanelObserver(m_buttonAbout, m_panelAbout);
            SetButtonClickTriggerPanelObserver(m_buttonMission, m_panelMissions);
            SetButtonClickTriggerPanelObserver(m_buttonSettings, m_panelSettings);
            SetButtonClickTriggerPanelObserver(m_buttonAbout, m_panelAbout);

            InitBackToHomeObserver();
        }

        private void InitBackToHomeObserver() {
            for (int x=0; x<m_buttonsBackToHome.Length; x++) {
                SetButtonClickTriggerPanelObserver(m_buttonsBackToHome[x], m_panelHome);
            }
        }

        private void OnlyEnablePanel(Image panel) {
            DisableAllPanels();
            panel.gameObject.SetActive(true);
        }

        private void DisableAllPanels() {
            m_panelAbout.gameObject.SetActive(false);
            m_panelHome.gameObject.SetActive(false);
            m_panelMissions.gameObject.SetActive(false);
            m_panelSettings.gameObject.SetActive(false);
        }

        private void DisableAllButtons() {
            m_buttonAbout.interactable = false;
            m_buttonExitGame.interactable = false;
            m_buttonMission.interactable = false;
            m_buttonSettings.interactable = false;

            for (int x=0; x<m_buttonsBackToHome.Length; x++) {
                m_buttonsBackToHome[x].interactable = false;
            }
        }

        private void SetButtonClickTriggerPanelObserver(Button button, Image panel) {
            button.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    m_modelSFX.PlaySFX(m_audioTheme.m_sfxButtonClick);
                    OnlyEnablePanel(panel);
                })
                .AddTo(this);
        }

    }

}