using NaughtyAttributes;
using UnityEngine;

namespace Scriptables {

    [CreateAssetMenu(fileName = "New DialogueLine", menuName = "HellRaiser/Create DialogueLine")]
    public class DialogueLine : ScriptableObject
    {

        #region Dropdown options

        public enum BGOption
        {
            NO_BG,
            SHOW_NEW_BG,
            RETAIN_PREVIOUS
        }

        public enum AvatarExpression
        {
            NORMAL,
            HAPPY,
            SAD,
            ANGRY,
            SURPRISED
        }

        #endregion

        [OnValueChanged("ChangeExpressionPreview")]
        public CharacterAvatar  m_speaker;

        [OnValueChanged("ChangeExpressionPreview")]
        public AvatarExpression m_expression;

        [SerializeField]
        [ShowAssetPreview]
        [Tooltip("Only for Inspector Preview. Changing its value manually won't affect the expression shown on GamePlay.")]
        private Texture2D m_expressionPreview; //ONLY FOR INSPECTOR PREVIEW

        [Space]

        [Tooltip("Print '???' as the temporary Character's name?")]
        public bool m_isNameAnonymized;
        [Tooltip("Have a shadow and '?' overlay to the Character Avatar?")]
        public bool m_isAvatarAnonymized;

        [Space]

        [TextArea]
        public string m_line;

        public AudioClip m_lineSFX;

        public BGOption m_backgroundOption;

        [EnableIf("HasBackground")]
        [ShowAssetPreview]
        public Texture2D m_background;

        private void OnEnable()
        {
            ChangeExpressionPreview();
        }

        private void ChangeExpressionPreview()
        {
            m_expressionPreview = (m_speaker != null) ? GetSpeakerAvatar() : null;
        }

        private bool HasBackground() {
            return (m_backgroundOption == BGOption.SHOW_NEW_BG);
        }

        public Texture2D GetSpeakerAvatar()
        {
            switch (m_expression)
            {
                case AvatarExpression.HAPPY:
                    {
                        return m_speaker.m_avatarHappy;
                    }
                case AvatarExpression.SAD:
                    {
                        return m_speaker.m_avatarSad;
                    }
                case AvatarExpression.ANGRY:
                    {
                        return m_speaker.m_avatarAngry;
                    }
                case AvatarExpression.SURPRISED:
                    {
                        return m_speaker.m_avatarSurprised;
                    }
                case AvatarExpression.NORMAL:
                default:
                    {
                        return m_speaker.m_avatarNormal;
                    }
            }
        }

    }

}
