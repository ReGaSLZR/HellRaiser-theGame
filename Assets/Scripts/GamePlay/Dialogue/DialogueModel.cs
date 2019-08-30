using UnityEngine;
using UniRx;
using Scriptables;
using UnityEngine.UI;
using NaughtyAttributes;
using Cinemachine;

namespace GamePlay.Dialogue {

    public class DialogueModel : MonoBehaviour, DialogueModel.Getter, DialogueModel.Setter
    {

        #region Interfaces 

        public interface Getter {
            ReactiveProperty<bool> IsInPlay();
        }

        public interface Setter {
            void StartDialogue(DialogueLine[] lines, Transform focusedObject);
            void EndDialogue();
        }

        #endregion

        [SerializeField]
        [Required]
        private DialogueLineDisplay m_display;

        [SerializeField]
        [Tooltip("A virtual camera with much higher priority than that of any Playable Character's unique vCam.")]
        [Required]
        private CinemachineVirtualCamera m_focusCamera;

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

            m_focusCamera.gameObject.SetActive(false);
        }

        private void ConfigForNewLines(Transform focusedObject)
        {
            m_isInPlay.Value = true;
            m_currentLineIndex = 0;

            if (focusedObject != null)
            {
                m_focusCamera.m_Follow = focusedObject;
                m_focusCamera.gameObject.SetActive(true);
            }
            else
            {
                m_focusCamera.gameObject.SetActive(false);
            }

            m_display.ConfigDisplay();
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

        public void StartDialogue(DialogueLine[] lines, Transform focusedObject)
        {
            m_dialogueLines = lines;
            ConfigForNewLines(focusedObject);

            DisplayNextLine();
        }

        public void EndDialogue() {
            m_focusCamera.gameObject.SetActive(false);

            m_display.End();
            m_isInPlay.Value = false;
        }

    }

}
