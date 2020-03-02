using Audio;
using Scriptables;
using Utils;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GamePlay.Dialogue {

    public class DialogueLineDisplay : MonoBehaviour
    {

        [Inject]
        private readonly AudioModel.SFXSetter m_modelSFX;

        [Header("UI")]

        [SerializeField]
        private TextMeshProUGUI m_textName;

        [SerializeField]
        private RawImage m_rawImageAvatar;

        [SerializeField]
        private RawImage m_rawImageAvatarAnonymized;

        [SerializeField]
        private TextMeshProUGUI m_textLine;

        [Space]

        [SerializeField]
        private RawImage m_backgroundBorder;

        [SerializeField]
        private RawImage m_background;

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

        private const string ANONYMIZED_NAME = "???";

        private void ChangeButtonNextText(bool isLastLine)
        {
            m_textButtonNext.text = (isLastLine) ? m_buttonTextLastLine : m_buttonTextNextLine;
        }

        public void ConfigDisplay()
        {
            m_backgroundBorder.gameObject.SetActive(false);
        }

        public void DisplayLine(DialogueLine line, bool isLastLine)
        {
            ChangeButtonNextText(isLastLine);
            m_modelSFX.PlaySFX(line.m_lineSFX);

            StopAllCoroutines();
            StartCoroutine(CorDisplayLine(line));
        }

        public void End()
        {
            StopAllCoroutines();
        }

        private IEnumerator CorDisplayLine(DialogueLine line)
        {
            m_textName.text = !line.m_isNameAnonymized
                ? line.m_speaker.m_name
                : ANONYMIZED_NAME;
            m_rawImageAvatar.texture = line.GetSpeakerAvatar();
            m_rawImageAvatarAnonymized
                .gameObject.SetActive(line.m_isAvatarAnonymized);

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

        private void UpdateDialogueBackground(DialogueLine.BGOption backgroundOption, Texture2D background)
        {
            switch (backgroundOption)
            {
                case DialogueLine.BGOption.NO_BG:
                    {
                        m_backgroundBorder.gameObject.SetActive(false);
                        break;
                    }
                case DialogueLine.BGOption.SHOW_NEW_BG:
                    {
                        if (background == null)
                        {
                            LogUtil.PrintWarning(this, GetType(),
                                "UpdateDialogueBackground(): BG_SHOW_NEW " +
                                "but missing actual BG content. Using BG_OFF mode instead.");
                            m_backgroundBorder.gameObject.SetActive(false);
                            return;
                        }

                        //turn it off first to activate any animations attached to the gameObject
                        m_backgroundBorder.gameObject.SetActive(false); 

                        m_background.texture = background;
                        m_backgroundBorder.gameObject.SetActive(true);
                        
                        break;
                    }
                //case DialogueLine.BGOption.RETAIN_PREVIOUS:
                //    {
                //        //purposely do nothing
                //        break;
                //    }
            }
        }

    }

}