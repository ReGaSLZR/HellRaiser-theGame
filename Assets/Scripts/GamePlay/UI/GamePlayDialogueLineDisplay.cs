using Scriptables;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI {

    public class GamePlayDialogueLineDisplay : MonoBehaviour
    {
        [Header("UI")]

        [SerializeField]
        private TextMeshProUGUI m_textName;

        [SerializeField]
        private RawImage m_rawImageAvatar;

        [SerializeField]
        private TextMeshProUGUI m_textLine;

        [SerializeField]
        private RawImage m_dialogueBackground;

        [Space]

        [SerializeField]
        private TextMeshProUGUI m_textButtonNext;

        [SerializeField]
        private string m_buttonTextNextLine = "Next";

        [SerializeField]
        private string m_buttonTextLastLine = "End";

        [Header("Configuration")]

        [SerializeField]
        private float m_lineLetterRevealDuration;

        private void ChangeButtonNextText(bool isLastLine)
        {
            m_textButtonNext.text = (isLastLine) ? m_buttonTextLastLine : m_buttonTextNextLine;
        }

        public void ConfigDisplay()
        {
            m_dialogueBackground.gameObject.SetActive(false);
        }

        public void DisplayLine(DialogueLine line, bool isLastLine)
        {
            ChangeButtonNextText(isLastLine);

            StopAllCoroutines();
            StartCoroutine(CorDisplayLine(line));
        }

        public void End()
        {
            StopAllCoroutines();
        }

        private IEnumerator CorDisplayLine(DialogueLine line)
        {
            m_textName.text = line.m_speaker.m_name;
            m_rawImageAvatar.texture = line.GetSpeakerAvatar();

            UpdateDialogueBackground(line.m_backgroundOption, line.m_background);

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

        private void UpdateDialogueBackground(int backgroundOption, Texture2D background)
        {
            switch (backgroundOption)
            {
                case DialogueLine.BG_OFF:
                    {
                        m_dialogueBackground.gameObject.SetActive(false);
                        break;
                    }
                case DialogueLine.BG_SHOW_NEW:
                    {
                        m_dialogueBackground.gameObject.SetActive(true);
                        m_dialogueBackground.texture = background;
                        break;
                    }
                    //case DialogueLine.BG_RETAIN: {
                    ////purposely do nothing
                    //break;
                    //}
            }
        }

    }

}