using Audio;
using GamePlay.Base;
using GamePlay.Input;
using GamePlay.Mission;
using NaughtyAttributes;
using Scriptables;
using UniRx;
using UnityEngine;
using Zenject;

namespace GamePlay.Dialogue {

    public class DialogueTrigger : BaseTrigger
    {

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
        private DialogueLine[] m_lines;

        [SerializeField]
        private bool m_shouldStopBGM = true;

        [SerializeField]
        [DisableIf("m_shouldStopBGM")]
        private AudioClip m_clipDialogueBGM;

        [Space]

        [SerializeField]
        private BaseTrigger m_chainedTriggerAfterDialogue;

        protected override void Start()
        {
            base.Start();

            m_modelDialogueGetter.IsInPlay()
                .Where(isInPlay => !isInPlay && m_isTriggered)
                .Subscribe(_ => {
                    m_modelTimer.StartTimer();
                    m_modelInput.EnableControls();

                    if (m_chainedTriggerAfterDialogue != null) {
                        m_chainedTriggerAfterDialogue.Execute();
                    }

                    m_modelBGM.PlayOriginal();
                    Destroy(gameObject);
                })
                .AddTo(this);
        }

        public override void Execute()
        {
            m_isTriggered = true;

            m_modelTimer.PauseTimer();
            m_modelInput.DisableControls();

            if (m_shouldStopBGM) {
                m_modelBGM.StopBGM();
            }
            else if (!m_shouldStopBGM && (m_clipDialogueBGM != null)) {
                m_modelBGM.PlayTemporary(m_clipDialogueBGM);
            }

            m_modelDialogueSetter.StartDialogue(m_lines);
        }

    }

}