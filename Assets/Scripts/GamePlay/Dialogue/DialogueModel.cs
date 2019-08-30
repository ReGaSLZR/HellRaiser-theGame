using UnityEngine;
using UniRx;
using Scriptables;
using UnityEngine.UI;
using GamePlay.UI;
using NaughtyAttributes;

namespace GamePlay.Dialogue {

    public class DialogueModel : MonoBehaviour, DialogueModel.Getter, DialogueModel.Setter
    {

        #region Interfaces 

        public interface Getter {
            ReactiveProperty<bool> IsInPlay();
        }

        public interface Setter {
            void StartDialogue(DialogueLine[] lines);
            void EndDialogue();
        }

        #endregion

        [SerializeField]
        [Required]
        private GamePlayDialogueLineDisplay m_display;

        [Space]

        [SerializeField]
        private Button m_buttonSkipAll;

        [SerializeField]
        private Button m_buttonNext;

        private DialogueLine[] m_dialogueLines;

        private int m_currentLineIndex = 0;
        private ReactiveProperty<bool> m_isInPlay = new ReactiveProperty<bool>(false);

        private void Start()
        {
            m_buttonNext.OnClickAsObservable()
                .Where(_ => (m_currentLineIndex < (m_dialogueLines.Length)))
                .Subscribe(_ => {
                    m_currentLineIndex++;

                    if (m_currentLineIndex == m_dialogueLines.Length)
                    {
                        EndDialogue();
                    }
                    else
                    {
                        DisplayNextLine();
                    }
                })
                .AddTo(this);

            m_buttonSkipAll.OnClickAsObservable()
                .Subscribe(_ => EndDialogue())
                .AddTo(this);
        }

        private void DisplayNextLine()
        {
            m_display.DisplayLine(m_dialogueLines[m_currentLineIndex],
                            (m_currentLineIndex == (m_dialogueLines.Length - 1)));
        }

        public ReactiveProperty<bool> IsInPlay()
        {
            return m_isInPlay;
        }

        public void StartDialogue(DialogueLine[] lines)
        {
            m_dialogueLines = lines;
            m_isInPlay.Value = true;
            m_currentLineIndex = 0;
            m_display.ConfigDisplay();

            DisplayNextLine();
        }

        public void EndDialogue() {
            m_display.End();
            m_isInPlay.Value = false;
        }

    }

}
