using NaughtyAttributes;
using UnityEngine;

namespace Scriptables {

    [CreateAssetMenu(fileName = "New Character Info", menuName = "HellRaiser/Create Character Info")]
    public class CharacterInfo : ScriptableObject
    {

        public const int HEALTH_MAX = 100;
        public const int HEALTH_CRITICAL = 25;

        public const int STAMINA_MAX = 100;

        public CharacterInfoUI m_infoUI;

        [ResizableTextArea]
        public string m_bio;

        [Header("Stats")]

        [HideInInspector]
        public int m_level;

        [Range(1, HEALTH_MAX)]
        public int m_health;

        [HideInInspector]
        public int m_stamina;

        public StatMovement m_statMovement;
        public StatOffense m_statOffense;
        
        [Space]

        [Range(1, 150)]
        public int m_defense = 1;

        [Range(0f, 100f)]
        public float m_deflectChance;

    }

    [System.Serializable]
    public class StatMovement {

        [Range(1f, 100f)]
        public float m_movSpeed = 1f;

        [Range(0.25f, 10f)]
        public float m_stunLength = 0.25f;

        public StatMovement(float speed = 1f, float stunLength = 0.25f) {
            m_movSpeed = speed;
            m_stunLength = stunLength;
        }

    }

    [System.Serializable]
    public class StatOffense {

        public const int MAX_CRIT_CHANCE = 100;

        [Range(1, 150)]
        public int m_attack = 1;

        [Range(0, MAX_CRIT_CHANCE)]
        public int m_critChance = 0;

        [Range(0f, 150f)]
        public float m_critDamage = 0f;

        public StatOffense(int attack = 1, int critChance = 0, float critDamage = 0f) {
            m_attack = attack;
            m_critChance = critChance;
            m_critDamage = critDamage;
        }

    }

}