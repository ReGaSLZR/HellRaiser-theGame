using Audio;
using GamePlay.Base;
using GamePlay.Input;
using GamePlay.Mission;
using Utils;
using NaughtyAttributes;
using Scriptables;
using UniRx;
using UnityEngine;
using Zenject;

namespace GamePlay.Dialogue {

    public class DialogueTrigger : BaseTrigger
    {

        public enum TriggerBGM
        {
            NONE,
            BGM_TEMP_STOP,
            BGM_TEMP_PLAY_NEW,
            BGM_REPLACE_WITH_NEW_AND_PLAY
        }

        //INJECTABLES
        [Inject]
        private readonly MissionModel.TimerSetter m_modelTimer;
        [Inject]
        private readonly DialogueModel.Setter m_modelDialogueSetter;
        [Inject]
        private readonly DialogueModel.Getter m_modelDialogueGetter;
        [Inject]
        private readonly BaseInputModel m_modelInput;
        [Inject]
        private readonly AudioModel.BGMSetter m_modelBGM;

        [SerializeField]
        private Transform m_focusedObject;

        [SerializeField]
        private BaseTrigger m_chainedTriggerAfterDialogue;

        [Space]

        [SerializeField]
        [ReorderableList]
        private DialogueLine[] m_lines;

        [SerializeField]
        private TriggerBGM m_bgmType;

        [SerializeField]
        [EnableIf("IsBGMRequired")]
        private AudioClip m_clipDialogueBGM;

        private bool IsBGMRequired() {
            return (m_bgmType == TriggerBGM.BGM_TEMP_PLAY_NEW) ||
                (m_bgmType == TriggerBGM.BGM_REPLACE_WITH_NEW_AND_PLAY);
        }

        protected override void Start()
        {
            base.Start();

            m_modelDialogueGetter.IsInPlay()
                .Where(isInPlay =>  m_isTriggered && !isInPlay)
                .Subscribe(_ => {
                    m_modelTimer.StartTimer();
                    m_modelInput.EnableControls();

                    if (m_chainedTriggerAfterDialogue != null)
                    {
                        m_chainedTriggerAfterDialogue.ExecuteWithDelay();
                    }

                    m_modelBGM.PlayOriginalBGM(true);
                    Destroy(gameObject);
                })
                .AddTo(this);
        }

        public override void Execute()
        {
            m_modelTimer.PauseTimer();
            m_modelInput.DisableControls();

            this.gameObject.SetActive(false);

            PlayBGM();

            m_isTriggered = true;
            m_modelDialogueSetter.StartDialogue(m_lines, m_focusedObject);
        }

        private void PlayBGM() {
            LogUtil.PrintInfo(gameObject, GetType(), $"PlayBGM(): {m_bgmType}");

            switch (m_bgmType) {
                default: {
                        break;
                    }
                case TriggerBGM.BGM_TEMP_STOP: {
                        m_modelBGM.StopBGM();
                        break;
                    }
                case TriggerBGM.BGM_TEMP_PLAY_NEW:
                    {
                       m_modelBGM.PlayTemporaryBGM(m_clipDialogueBGM, true);
                        break;
                    }
                case TriggerBGM.BGM_REPLACE_WITH_NEW_AND_PLAY: {
                        m_modelBGM.ReplaceOriginalBGM(m_clipDialogueBGM);
                        m_modelBGM.PlayOriginalBGM(true);
                        break;
                    }
            }
        }

    }

}