using UnityEngine;

namespace Character.Stats {

    public class StatsUtil : MonoBehaviour
    {

        public static int GetDamageDealt(int skillDamage, int attackStat) {
            return Mathf.RoundToInt(skillDamage * (attackStat * 0.01f));
        }

        public static int GetDamageReceived(int damageDealt, int defenseStat) {
            return Mathf.RoundToInt(damageDealt / (defenseStat * 0.01f));
        }
        
    }

}