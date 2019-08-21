using UnityEngine;
using NaughtyAttributes;

namespace Scriptables {

    [CreateAssetMenu(fileName = "New Character Info UI", menuName = "HellRaiser/Create Character Info UI")]
    public class CharacterInfoUI : ScriptableObject {

        public string m_name;
        [ShowAssetPreview]
        public Texture2D m_avatarMain;

        //TODO add more avatars specific for other expressions
        //TODO create a method for getting specific expression

    }

}