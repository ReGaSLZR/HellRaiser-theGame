using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "New InputSettings",
        menuName = "HellRaiser/Create InputSettings")]
    public class InputSettings : ScriptableObject
    {
        [Header("Movement Keys")]
        public KeyCode m_keyJump = KeyCode.I;
        public KeyCode m_keyMoveRight = KeyCode.D;
        public KeyCode m_keyMoveLeft = KeyCode.A;

        [Header("Skill Keys")]
        public KeyCode m_keySkillMain = KeyCode.J;
        public KeyCode m_keySkill2 = KeyCode.K;
        public KeyCode m_keySkill3 = KeyCode.L;

        [Header("Change Playable Character Key")]
        public KeyCode m_keyChangeChar = KeyCode.O;

        [Header("Camera Pan Keys")]
        public KeyCode m_keyCameraUp = KeyCode.UpArrow;
        public KeyCode m_keyCameraDown = KeyCode.DownArrow;
        public KeyCode m_keyCameraLeft = KeyCode.LeftArrow;
        public KeyCode m_keyCameraRight = KeyCode.RightArrow;
    }
}