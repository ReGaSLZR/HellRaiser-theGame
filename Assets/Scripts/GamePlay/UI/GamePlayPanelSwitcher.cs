﻿using Data.Storage;
using GamePlay.Dialogue;
using GamePlay.Input;
using GamePlay.Mission;
using GamePlay.Stats;
using GamePlay.Timer;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GamePlay.UI {

    public class GamePlayPanelSwitcher : MonoBehaviour
    {
        //INJECTIBLES
        [Inject]
        private readonly BaseInputModel m_modelInput;
        [Inject]
        private readonly GamePlayTimerModel.Getter m_modelTimer;
        [Inject]
        private readonly GamePlayStatsModel.Getter m_modelStats;
        [Inject]
        private readonly GamePlayDialogueModel.Getter m_modelDialogue;

        [Header("Buttons")]

        [SerializeField]
        private Button[] m_buttonResume;

        [Space]

        [SerializeField]
        private Button[] m_buttonPause;
        [SerializeField]
        private Button[] m_buttonPauseSettings;
        [SerializeField]
        private Button[] m_buttonPauseQuit;
        [SerializeField]
        private Button[] m_buttonPauseBackToMainContent;

        [Space]

        [SerializeField]
        private Button[] m_buttonReloadLevel;
        [SerializeField]
        private Button[] m_buttonMainMenu;

        [Header("Panels")]

        [SerializeField]
        private Image m_panelOnScreenInput;

        [SerializeField]
        private Image m_panelHUD;

        [SerializeField]
        private Image m_panelDialogue;

        [Space]

        [SerializeField]
        private Image m_panelPause;
        [SerializeField]
        private Image m_panelPauseContentMain;
        [SerializeField]
        private Image m_panelPauseContentSettings;
        [SerializeField]
        private Image m_panelPauseContentConfirmQuit;

        [Space]

        [SerializeField]
        private Image m_panelGameOver;
        [SerializeField]
        private Image m_panelGameOverContentClear;
        [SerializeField]
        private Image m_panelGameOverContentFail;

        [Space]

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

        //TODO panel for input (on-screen buttons)
        //TODO vignette changing
        //TODO mission objectives
        //TODO dialogue

        private void Awake()
        {
            DeactivateAllPanels();
        }

        private void Start()
        {
            m_modelDialogue.IsInPlay()
                .Subscribe(isInPlay => {
                    OnlyActivatePanel(isInPlay ? m_panelDialogue : m_panelHUD);
                })
                .AddTo(this);

            InitObserverPauseButtons();

            InitObserverButtons(m_buttonResume, m_panelHUD, null, 1, 0, true);
            InitObserverButtons(m_buttonReloadLevel, m_panelLoading, null, 1, SceneData.SCENE_SELF, false);
            InitObserverButtons(m_buttonMainMenu, m_panelLoading, null, 1, SceneData.SCENE_MAIN_MENU, false);

            InitGameOverObservers();

            ShowDefaultPanels();
        }

        private void ShowDefaultPanels() {
            OnlyActivatePanel(m_panelHUD);
            m_panelOnScreenInput.gameObject.SetActive((InputType.OnScreenButtons == m_modelInput.m_inputType));
        }

        private void InitGameOverObservers() {
            m_modelTimer.GetTimer()
                .Where(countdown => (countdown <= 0))
                .Subscribe(_ => {
                    SetGameOverPanel(false, m_spielGameOverFailTime);
                })
                .AddTo(this);

            m_modelStats.GetMissionStatus()
               .Where(status => (status != MissionStatus.IS_STARTED))
               .Subscribe(status => {
                   SetGameOverPanel((MissionStatus.IS_CLEARED == status), 
                       (MissionStatus.IS_CLEARED == status) ? m_spielGameOverClear : m_spielGameOverFailNormal);
               })
               .AddTo(this);
        }

        private void SetGameOverPanel(bool isCleared, string failSpiel) {
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

            m_textGameOverTitle.text = failSpiel;
        }

        private void InitObserverButtons(Button[] buttons, Image panelMain, Image panelContent, 
          int timeScale, int sceneToLoad, bool shouldShowOnScreenInput) {
            foreach (Button button in buttons)
            {
                button.OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        if (panelMain != null) { 
                            OnlyActivatePanel(panelMain);
                        }

                        if (panelContent != null) {
                            panelContent.gameObject.SetActive(true);
                        }

                        if (InputType.OnScreenButtons == m_modelInput.m_inputType) {
                            m_panelOnScreenInput.gameObject.SetActive(shouldShowOnScreenInput);
                        }

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

            if (InputType.OnScreenButtons == m_modelInput.m_inputType) {
                m_panelOnScreenInput.gameObject.SetActive(false);
            }

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