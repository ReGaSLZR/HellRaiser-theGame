using GamePlay.Base;
using GamePlay.Input;
using GamePlay.Mission;
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
        private readonly GamePlayDialogueModel.Setter m_modelDialogueSetter;

        [Inject]
        private readonly GamePlayDialogueModel.Getter m_modelDialogueGetter;

        [Inject]
        private readonly BaseInputModel m_modelInput;

        [SerializeField]
        private DialogueLine[] m_lines;

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

                    Destroy(gameObject);
                })
                .AddTo(this);
        }

        public override void Execute()
        {
            m_isTriggered = true;

            m_modelTimer.PauseTimer();
            m_modelInput.DisableControls();
            m_modelDialogueSetter.StartDialogue(m_lines);
        }

    }

}