using UnityEngine;
using NaughtyAttributes;

namespace Scriptables {

    [CreateAssetMenu(fileName = "Avatar-", menuName = "HellRaiser/Create Character Avatar")]
    public class CharacterAvatar : ScriptableObject {

        public string m_name;

        [ShowAssetPreview]
        [Tooltip("The difference between Main and Normal is that the former is for HUD.")]
        public Texture2D m_avatarMain;

        [Header("For Dialogue Cutscenes:")]

        [ShowAssetPreview]
        public Texture2D m_avatarNormal;

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