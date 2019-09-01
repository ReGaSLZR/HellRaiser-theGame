using Scriptables;
using UnityEngine;

namespace Character.Stats {

    public class StatsUtil : MonoBehaviour
    {

        public static int GetRawDamageDealt(int skillDamage, StatOffense statOffense, StatInflictionType type) {
            int attackStat = (type == StatInflictionType.PHYSICAL) ? statOffense.m_physicalAttack : statOffense.m_magickAttack;
            return Mathf.RoundToInt(skillDamage * (attackStat * 0.01f));
        }

        public static int GetCritDamage(int rawDamageDealt, StatOffense statOffense) {
            if ((statOffense.m_critChance > 0) &&
                (Random.Range(1, StatOffense.MAX_CRIT_CHANCE) <= statOffense.m_critChance))
            {
                return Mathf.RoundToInt(rawDamageDealt * (statOffense.m_critDamage * 0.01f));
            }

            return 0;
        }

        public static int GetDamageReceived(int damageDealt, StatDefense statDefense, StatInflictionType type) {
            int defenseStat = (type == StatInflictionType.PHYSICAL) ? statDefense.m_physicalDefense : statDefense.m_magickDefense;
            return Mathf.Clamp(Mathf.RoundToInt(damageDealt / (defenseStat * 0.01f)), 1, 9999);
        }
        
    }

}