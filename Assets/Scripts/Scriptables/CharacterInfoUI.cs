using UnityEngine;
using NaughtyAttributes;

namespace Scriptables {

    [CreateAssetMenu(fileName = "New Character Info UI", menuName = "HellRaiser/Create Character Info UI")]
    public class CharacterInfoUI : ScriptableObject {

        public string m_name;

        [ShowAssetPreview]
        public Texture2D m_avatarMain;

        [ShowAssetPreview]
        public Texture2D m_avatarHappy;

        [ShowAssetPreview]
        public Texture2D m_avatarSad;

        [ShowAssetPreview]
        public Texture2D m_avatarAngry;

        [ShowAssetPreview]
        public Texture2D m_avatarSurprised;

    }

}