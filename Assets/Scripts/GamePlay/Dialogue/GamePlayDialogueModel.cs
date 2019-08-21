using UnityEngine;
using UniRx;
using Scriptables;
using TMPro;
using UnityEngine.UI;
using System.Collections;

namespace GamePlay.Dialogue {

    public class GamePlayDialogueModel : MonoBehaviour, GamePlayDialogueModel.Getter, GamePlayDialogueModel.Setter
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

        [Header("UI")]

        [SerializeField]
        private TextMeshProUGUI m_textName;

        [SerializeField]
        private RawImage m_rawImageAvatar;

        [SerializeField]
        private TextMeshProUGUI m_textLine;

        [SerializeField]
        private Button m_buttonSkipAll;

        [SerializeField]
        private Button m_buttonNext;

        [SerializeField]
        private TextMeshProUGUI m_textButtonNext;
 
        [Space]

        [SerializeField]
        private string m_buttonTextNextLine = "Next";

        [SerializeField]
        private string m_buttonTextLastLine = "End";

        [Header("Configuration")]

        [SerializeField]
        [Range(0.01f, 3f)]
        private float m_lineLetterRevealDuration = 0.01f;

        private ReactiveProperty<bool> m_isInPlay = new ReactiveProperty<bool>(false);
        private DialogueLine[] m_dialogueLines;

        private int m_currentLineIndex = 0;

        private void Start()
        {
            m_buttonNext.OnClickAsObservable()
                .Where(_ => (m_currentLineIndex < (m_dialogueLines.Length)))
                .Subscribe(_ => {
                    m_currentLineIndex++;
                    ChangeButtonNextText();

                    if (m_currentLineIndex == m_dialogueLines.Length)
                    {
                        EndDialogue();
                    }
                    else
                    {
                        StopAllCoroutines();
                        StartCoroutine(CorStartDialogue());
                    }
                })
                .AddTo(this);

            m_buttonSkipAll.OnClickAsObservable()
                .Subscribe(_ => EndDialogue())
                .AddTo(this);
        }

        private void ChangeButtonNextText() {
            m_textButtonNext.text = (m_currentLineIndex == (m_dialogueLines.Length - 1)) ?
                        m_buttonTextLastLine : m_buttonTextNextLine;
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
            ChangeButtonNextText();

            StopAllCoroutines();
            StartCoroutine(CorStartDialogue());
        }

        public void EndDialogue() {
            m_isInPlay.Value = false;
            StopAllCoroutines();
        }

        private IEnumerator CorStartDialogue() {
            DialogueLine line = m_dialogueLines[m_currentLineIndex];

            m_textName.text = line.m_speaker.m_name;
            m_rawImageAvatar.texture = line.GetSpeakerAvatar();

            //display the line letter by letter
            char[] charArray = line.m_line.ToCharArray();
            string displayText = "";

            for (int x = 0; x < charArray.Length; x++)
            {
                displayText += charArray[x];
                m_textLine.SetText(displayText);
                yield return new WaitForSeconds(m_lineLetterRevealDuration);
            }
        }

    }

}
