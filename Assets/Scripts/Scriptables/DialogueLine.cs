using UnityEngine;

namespace Scriptables {

    [CreateAssetMenu(fileName = "New DialogueLine", menuName = "HellRaiser/Create DialogueLine")]
    public class DialogueLine : ScriptableObject
    {

        public CharacterInfoUI m_speaker;
        //TODO add dropdown option for character avatar expression

        [TextArea]
        public string m_line;

        public Texture2D GetSpeakerAvatar()
        {
            return m_speaker.m_avatarMain; //TODO for now... default to main avatar, while the other expression-specific avatars are not ready
        }

    }

}
