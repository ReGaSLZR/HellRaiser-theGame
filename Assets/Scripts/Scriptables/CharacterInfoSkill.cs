﻿using NaughtyAttributes;
using UnityEngine;

namespace Scriptables
{

    [CreateAssetMenu(fileName = "New Character Info Skill", menuName = "HellRaiser/Create Character Info Skill")]
    public class CharacterInfoSkill : ScriptableObject
    {

        public string m_name;

        [TextArea]
        public string m_description;

        //[MinMaxSlider(0f, 150f)]
        //public Vector2 m_damageRange;

        [Range(0.1f, 10f)]
        public float m_duration;

        [Range(0, CharacterInfo.STAMINA_MAX)]
        public int m_cost;

        public Texture2D m_icon;
        
    }

}