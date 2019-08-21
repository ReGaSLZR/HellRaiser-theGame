using GamePlay.Input;
using GamePlay.Timer;
using Scriptables;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace GamePlay.Dialogue {

    [RequireComponent(typeof(Collider2D))]
    public class DialogueTrigger : MonoBehaviour
    {

        [Inject]
        private readonly GamePlayTimerModel.Setter m_modelTimer;

        [Inject]
        private readonly GamePlayDialogueModel.Setter m_modelDialogueSetter;

        [Inject]
        private readonly GamePlayDialogueModel.Getter m_modelDialogueGetter;

        [Inject]
        private readonly BaseInputModel m_modelInput;

        [SerializeField]
        private DialogueLine[] m_lines;

        private bool m_isTriggered;

        private void Start()
        {
            this.OnTriggerEnter2DAsObservable()
               .Where(otherCollider2D => (otherCollider2D.tag.Contains("Player")))
               .Subscribe(otherCollider2D => {
                   m_isTriggered = true;

                   m_modelTimer.PauseTimer();
                   m_modelInput.DisableControls();
                   m_modelDialogueSetter.StartDialogue(m_lines);
               })
               .AddTo(this);

            m_modelDialogueGetter.IsInPlay()
                .Where(isInPlay => !isInPlay && m_isTriggered)
                .Subscribe(_ => {
                    m_modelTimer.StartTimer();
                    m_modelInput.EnableControls();

                    Destroy(gameObject);
                })
                .AddTo(this);
        }

    }

}