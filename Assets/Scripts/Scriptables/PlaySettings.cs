using GamePlay.Input;
using UnityEngine;

namespace Scriptables {


    [CreateAssetMenu(fileName = "New PlaySettings", menuName = "HellRaiser/Create Settings")]
    public class PlaySettings : ScriptableObject
    {

        [SerializeField]
        public InputType m_gamePlayInput;

        //TODO code more play settings variables and usage

    }


}