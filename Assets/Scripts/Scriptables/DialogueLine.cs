using NaughtyAttributes;
using UnityEngine;

namespace Scriptables {

    [CreateAssetMenu(fileName = "New DialogueLine", menuName = "HellRaiser/Create DialogueLine")]
    public class DialogueLine : ScriptableObject
    {

        #region Dropdown options

        public const int BG_OFF = 0;
        public const int BG_SHOW_NEW = 1;
        public const int BG_RETAIN = 2;

        private readonly DropdownList<int> m_dropdownOptionsBackground = new DropdownList<int>() {
            {"No Background", BG_OFF},
            {"Show New Background", BG_SHOW_NEW},
            {"Retain Previous", BG_RETAIN}
        };

        private const int EXPRESSION_NORMAL = 0;
        private const int EXPRESSION_HAPPY = 1;
        private const int EXPRESSION_SAD = 2;
        private const int EXPRESSION_ANGRY = 3;
        private const int EXPRESSION_SURPRISED = 4;

        private readonly DropdownList<int> m_dropdownOptionsExpressions = new DropdownList<int>() {
            {"Normal", EXPRESSION_NORMAL},
            {"Happy", EXPRESSION_HAPPY},
            {"Sad", EXPRESSION_SAD},
            {"Angry", EXPRESSION_ANGRY},
            {"Surprised", EXPRESSION_SURPRISED},
        };

        #endregion

        public CharacterAvatar  m_speaker;
        [Dropdown("m_dropdownOptionsExpressions")]
        public int m_expression;

        [TextArea]
        public string m_line;

        public AudioClip m_lineSFX;

        [Dropdown("m_dropdownOptionsBackground")]
        public int m_backgroundOption;

        [EnableIf("HasBackground")]
        [ShowAssetPreview]
        public Texture2D m_background;

        private bool HasBackground() {
            return (m_backgroundOption == BG_SHOW_NEW);
        }

        public Texture2D GetSpeakerAvatar()
        {
            switch (m_expression)
            {
                case EXPRESSION_HAPPY:
                    {
                        return m_speaker.m_avatarHappy;
                    }
                case EXPRESSION_SAD:
                    {
                        return m_speaker.m_avatarSad;
                    }
                case EXPRESSION_ANGRY:
                    {
                        return m_speaker.m_avatarAngry;
                    }
                case EXPRESSION_SURPRISED:
                    {
                        return m_speaker.m_avatarSurprised;
                    }
                case EXPRESSION_NORMAL:
                default:
                    {
                        return m_speaker.m_avatarNormal;
                    }
            }
        }

    }

}
