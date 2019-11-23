using NaughtyAttributes;
using UnityEngine;

namespace Scriptables {

    [CreateAssetMenu(fileName = "Info-", menuName = "HellRaiser/Create Character Info")]
    public class CharacterInfo : ScriptableObject
    {

        public const int HEALTH_MAX = 100;
        public const int HEALTH_CRITICAL = 25;

        public const int STAMINA_MAX = 100;

        [SerializeField] private bool m_isMob;

        [HideIf("m_isMob")]
        public CharacterAvatar m_avatar;

        [HideIf("m_isMob")]
        [ResizableTextArea]
        public string m_bio;

        [HideIf("m_isMob")]
        public bool m_isPlayable;

        [ShowIf("m_isPlayable")]
        public CharacterSkill[] m_skillsInOrder = new CharacterSkill[3];

        [Header("Stats")]

        public CharacterRank m_rank;

        [HideIf("m_isPlayable")]
        [Range(0, HEALTH_MAX)]
        public int m_health = HEALTH_MAX;

        [HideIf("m_isPlayable")]
        [Range(0, STAMINA_MAX)]
        public int m_stamina = STAMINA_MAX;

        public StatMovement m_statMovement;
        public StatOffense m_statOffense;
        public StatDefense m_statDefense;

        public void SetToFullHealthStamina()
        {
            m_health = HEALTH_MAX;
            m_stamina = STAMINA_MAX;
        }

    }

    [System.Serializable]
    public class StatMovement {

        [Range(0.75f, 100f)]
        public float m_movSpeed = 1f;

        [Range(0.25f, 10f)]
        public float m_stunLength = 0.25f;

        [Range(0.25f, 10f)]
        public float m_deathLength = 0.25f;

        public StatMovement(float speed = 1f, float stunLength = 0.25f, float deathLength = 0.25f) {
            m_movSpeed = speed;
            m_stunLength = stunLength;
            m_deathLength = deathLength;
        }

    }

    [System.Serializable]
    public class StatOffense {

        public const int MAX_CRIT_CHANCE = 100;

        [Range(0, 150)]
        public int m_physicalAttack = 0;

        [Range(0, 150)]
        public int m_magickAttack = 0;

        [Range(0, MAX_CRIT_CHANCE)]
        public int m_critChance = 0;

        [Range(0f, 150f)]
        public float m_critDamage = 0f;

        public StatOffense(int attack = 1, int critChance = 0, float critDamage = 0f) {
            m_physicalAttack = attack;
            m_critChance = critChance;
            m_critDamage = critDamage;
        }

    }

    [System.Serializable]
    public class StatDefense {

        [Range(0, 150)]
        public int m_physicalDefense = 0;

        public bool m_isMagusBane;

        [Range(0, 150)]
        [HideIf("m_isMagusBane")]
        public int m_magickDefense = 0;

        public StatDefense(int pDef, int mDef, bool isMagusBane) {
            m_physicalDefense = pDef;
            m_magickDefense = mDef;
            m_isMagusBane = isMagusBane;
        }

    }

    public enum CharacterRank {
        F,
        E,
        D,
        C,
        B,
        A,
        S,
        SS
    }

}