using Audio;
using Data.Storage;
using GamePlay.Dialogue;
using GamePlay.Input;
using GamePlay.Mission;
using GamePlay.Stats;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using static Scriptables.PlaySettings;

namespace GamePlay.UI {

    public class GamePlayPanelSwitcher : MonoBehaviour
    {
        //INJECTIBLES
        [Inject]
        private readonly BaseInputModel m_modelInput;
        [Inject]
        private readonly MissionModel.TimerGetter m_modelTimer;
        [Inject]
        private readonly MissionModel.MissionGetter m_modelMission;
        [Inject]
        private readonly GamePlayStatsModel.Getter m_modelStats;
        [Inject]
        private readonly DialogueModel.Getter m_modelDialogue;
        [Inject]
        private readonly MerchantModel.Getter m_modelMerchant;
        [Inject]
        private readonly AudioModel.BGMSetter m_modelBGM;
        [Inject]
        private readonly AudioModel.SFXSetter m_modelSFX;
        [Inject]
        private readonly AudioTheme m_audioTheme;

        [SerializeField]
        private Canvas m_canvas;

        [Header("--- Buttons ---")]

        [SerializeField]
        private Button[] m_buttonResume;

        [Header("Pause Buttons")]

        [SerializeField]
        private Button[] m_buttonPause;
        [SerializeField]
        private Button[] m_buttonPauseSettings;
        [SerializeField]
        private Button[] m_buttonPauseQuit;
        [SerializeField]
        private Button[] m_buttonPauseBackToMainContent;

        [Header("Scene Change Buttons")]

        [SerializeField]
        private Button[] m_buttonReloadLevel;
        [SerializeField]
        private Button[] m_buttonMainMenu;

        [Header("--- Panels ---")]

        [SerializeField]
        private Image m_panelOnScreenInput;

        [SerializeField]
        private Image m_panelMerchantGoods;

        [SerializeField]
        private Image m_panelHUD;

        [SerializeField]
        private Image m_panelMissionObjective;

        [SerializeField]
        private Image m_panelDialogue;

        [Header("Pause Panel and Contents")]

        [SerializeField]
        private Image m_panelPause;
        [SerializeField]
        private Image m_panelPauseContentMain;
        [SerializeField]
        private Image m_panelPauseContentSettings;
        [SerializeField]
        private Image m_panelPauseContentConfirmQuit;

        [Header("Game Over Contents")]

        [SerializeField]
        private Image m_panelGameOver;
        [SerializeField]
        private Image m_panelGameOverContentClear;
        [SerializeField]
        private Image m_panelGameOverContentFail;

        [Header("Game Over Spiels")]

        [SerializeField]
        private TextMeshProUGUI m_textGameOverTitle;
        [SerializeField]
        private string m_spielGameOverFailTime = "Time's up!";
        [SerializeField]
        private string m_spielGameOverFailNormal = "Mission Failed";
        [SerializeField]
        private string m_spielGameOverClear = "Mission Accomplished";

        [Space]

        [SerializeField]
        private Image m_panelLoading;

        private void Awake()
        {
            DeactivateAllPanels();
            Config();
        }

        private void Config()
        {
            m_modelDialogue.IsInPlay()
                .Subscribe(isInPlay => {
                    OnlyActivatePanel(isInPlay ? m_panelDialogue : m_panelHUD);
                    SetOnScreenInputActive(!isInPlay);
                })
                .AddTo(this);

            InitObserverPauseButtons();

            InitObserverButtons(m_buttonResume, m_panelHUD, null, 1, 0, true);
            InitObserverButtons(m_buttonReloadLevel, m_panelLoading, null, 1, SceneData.GetCurrentSceneIndex(), false);
            InitObserverButtons(m_buttonMainMenu, m_panelLoading, null, 1, SceneData.SCENE_MAIN_MENU, false);

            InitGameOverObservers();
            InitMerchantObserver();

            m_canvas.gameObject.SetActive(true);
            ShowDefaultPanels();
        }

        private void SetOnScreenInputActive(bool isActive)
        {
            if ((InputType.OnScreenButtons == m_modelInput.m_inputType))
            {
                m_panelOnScreenInput.gameObject.SetActive(isActive);
            }
        }

        private void ShowDefaultPanels() {
            OnlyActivatePanel(m_panelHUD);
            m_panelMissionObjective.gameObject.SetActive(false);
            m_panelMerchantGoods.gameObject.SetActive(false);

            SetOnScreenInputActive(InputType.OnScreenButtons == m_modelInput.m_inputType);
        }

        private void InitMerchantObserver() {
            m_modelMerchant.IsViewingMerchantGoods()
                .Subscribe(isViewing => {
                    m_panelMerchantGoods.gameObject.SetActive(isViewing);
                    Time.timeScale = isViewing ? 0 : 1;

                    //disable/enable all pause buttons when merchant goods are shown/hidden
                    for (int x = 0; x < m_buttonPause.Length; x++)
                    {
                        m_buttonPause[x].gameObject.SetActive(!isViewing);
                    }

                    SetOnScreenInputActive(!isViewing);

                    if (isViewing)
                    {
                        m_modelInput.DisableControls();
                    }
                    else
                    {
                        m_modelInput.EnableControls();
                    }

                })
                .AddTo(this);
        }

        private void InitGameOverObservers() {
            m_modelMission.GetMissionStatus()
                .Subscribe(status => {
                    switch (status) {
                        default:
                        case MissionStatus.ONGOING:
                        case MissionStatus.SHOWN: {
                                m_panelMissionObjective.gameObject.SetActive((MissionStatus.SHOWN == status));
                                break;
                            }
                        case MissionStatus.CLEARED:
                        case MissionStatus.FAILED: {
                                m_modelBGM.ReplaceOriginalBGM((status == MissionStatus.CLEARED) ? 
                                    m_audioTheme.m_bgmMissionAccomplished : m_audioTheme.m_bgmMissionFailure);
                                m_modelBGM.PlayOriginalBGM(false);

                                m_panelMissionObjective.gameObject.SetActive(false);

                                SetGameOverPanel((MissionStatus.CLEARED == status),
                                    (MissionStatus.CLEARED == status) ? m_spielGameOverClear : 
                                    ((m_modelTimer.GetTimer().Value == 0) ? m_spielGameOverFailTime : m_spielGameOverFailNormal));
                                break;
                            }
                    }
                })
                .AddTo(this);
        }

        private void SetGameOverPanel(bool isCleared, string title) {
            OnlyActivatePanel(m_panelGameOver);
            Time.timeScale = 0;

            if (isCleared)
            {
                m_panelGameOverContentClear.gameObject.SetActive(true);
            }
            else
            {
                m_panelGameOverContentFail.gameObject.SetActive(true);
            }

            m_textGameOverTitle.text = title;
        }

        private void InitObserverButtons(Button[] buttons, Image panelMain, Image panelContent, 
          int timeScale, int sceneToLoad, bool shouldShowOnScreenInput) {
            foreach (Button button in buttons)
            {
                button.OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        m_modelSFX.PlaySFX(m_audioTheme.m_sfxButtonClick);

                        if (panelMain != null) { 
                            OnlyActivatePanel(panelMain);
                        }

                        if (panelContent != null) {
                            panelContent.gameObject.SetActive(true);
                        }

                        SetOnScreenInputActive(shouldShowOnScreenInput);

                        Time.timeScale = timeScale;
                        SceneData.StoreLevelThenLoad(sceneToLoad);
                    })
                    .AddTo(this);
            }
        }

        private void InitObserverPauseButtons() {
            InitObserverButtons(m_buttonPause, m_panelPause, m_panelPauseContentMain, 0, 0, false);

            InitObserverButtons(m_buttonPauseSettings, m_panelPause, m_panelPauseContentSettings, 0, 0, false);
            InitObserverButtons(m_buttonPauseQuit, m_panelPause, m_panelPauseContentConfirmQuit, 0, 0, false);
            InitObserverButtons(m_buttonPauseBackToMainContent, m_panelPause, m_panelPauseContentMain, 0, 0, false);
        }

        private void InitObserverPauseContentButtons(Button[] buttonsPauseContent, Image panelPauseContent) {
            foreach (Button buttonPauseContent in buttonsPauseContent)
            {
                buttonPauseContent.OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        panelPauseContent.gameObject.SetActive(true);
                    })
                    .AddTo(this);
            }
        }

        private void OnlyActivatePanel(Image panel)
        {
            DeactivateAllPanels();
            panel.gameObject.SetActive(true);
        }

        private void DeactivateAllPanels() {
            m_panelHUD.gameObject.SetActive(false);
            m_panelDialogue.gameObject.SetActive(false);

            SetOnScreenInputActive(false);

            m_panelPause.gameObject.SetActive(false);
            m_panelPauseContentMain.gameObject.SetActive(false);
            m_panelPauseContentSettings.gameObject.SetActive(false);
            m_panelPauseContentConfirmQuit.gameObject.SetActive(false);

            m_panelGameOver.gameObject.SetActive(false);
            m_panelGameOverContentClear.gameObject.SetActive(false);
            m_panelGameOverContentFail.gameObject.SetActive(false);

            m_panelLoading.gameObject.SetActive(false);
        }

    }


}