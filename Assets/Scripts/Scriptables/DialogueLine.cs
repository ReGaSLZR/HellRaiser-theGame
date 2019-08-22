using NaughtyAttributes;
using UnityEngine;

namespace Scriptables {

    [CreateAssetMenu(fileName = "New DialogueLine", menuName = "HellRaiser/Create DialogueLine")]
    public class DialogueLine : ScriptableObject
    {

        public const int BG_OFF = 0;
        public const int BG_SHOW_NEW = 1;
        public const int BG_RETAIN = 2;
        private readonly DropdownList<int> m_dropdownOptionsBackground = new DropdownList<int>() {
            {"No Background", BG_OFF},
            {"Show New Background", BG_SHOW_NEW},
            {"Retain Previous", BG_RETAIN}
        };

        public CharacterInfoUI m_speaker;
        //TODO add dropdown option for character avatar expression

        [TextArea]
        public string m_line;

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
            return m_speaker.m_avatarMain; //TODO for now... default to main avatar, while the other expression-specific avatars are not ready
        }

    }

}
