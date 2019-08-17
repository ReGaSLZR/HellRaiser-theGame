using UnityEngine;

namespace Character.Stats {

    [CreateAssetMenu(fileName = "New CharacterStat", menuName = "HellRaiser/Create Character Stat")]
    public class CharacterStats : ScriptableObject
    {

        public const int HEALTH_MAX = 100;
        public const int HEALTH_CRITICAL = 25;

        [Range(1, 99)]
        public int m_level;

        [Space]

        [Range(1, HEALTH_MAX)]
        public int m_health;

        [Range(1f, 100f)]
        public float m_movementSpeed;

        [Space]

        [Range(1, 150)]
        public int m_attack;

        [Space]

        [Range(1, 150)]
        public int m_defense;

        [Range(1f, 10f)]
        public float m_stunLength;

        [Space]

        [Range(1f, 100f)]
        public float m_critChance;

        [Range(1f, 100f)]
        public float m_critDamage;

        [Space]

        [Range(0f, 100f)]
        public float m_deflectChance;

    }

}