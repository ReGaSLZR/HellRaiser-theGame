using Scriptables;
using UnityEngine;

namespace Character.Stats {

    public class StatsUtil : MonoBehaviour
    {

        public const int MIN_DAMAGE = 1;
        public const int MAX_DAMAGE = 9999;

        public static int GetRawDamage(int skillDamage, StatOffense statOffense, StatInflictionType type) {
            int attackStat = (type == StatInflictionType.PHYSICAL) ? statOffense.m_physicalAttack : statOffense.m_magickAttack;
            return Mathf.Clamp(Mathf.RoundToInt(skillDamage * (attackStat * 0.01f)), MIN_DAMAGE, MAX_DAMAGE);
        }

        public static int GetCritDamage(int rawDamageDealt, StatOffense statOffense) {
            if ((statOffense.m_critChance > 0) &&
                (Random.Range(1, StatOffense.MAX_CRIT_CHANCE) <= statOffense.m_critChance))
            {
                return Mathf.RoundToInt(rawDamageDealt * (statOffense.m_critDamage * 0.01f)); //purposely no max value cap applied here
            }

            return 0;
        }

        public static bool IsInflictionReduceableByBane(bool isMagusBane, StatInflictionType type) {
            return (isMagusBane && (type == StatInflictionType.MAGICK));
        }

        public static int GetDamageReducedByDefense(int damageValue, StatDefense statDefense, StatInflictionType type, CharacterRank rank) {
            if (IsInflictionReduceableByBane(statDefense.m_isMagusBane, type)) {
                //special computation for MagusBanes hit with Magick-based attacks (M. Defense is ignored here)
                return GetMagickDamageReducedByMagusBane(rank, damageValue); 
            }

            //normal computation of damage reduced by defense stat
            int defenseStat = (type == StatInflictionType.PHYSICAL) ? statDefense.m_physicalDefense : statDefense.m_magickDefense;
            return Mathf.Clamp(Mathf.RoundToInt(damageValue / (defenseStat * 0.01f)), MIN_DAMAGE, MAX_DAMAGE);
        }

        public static int GetRecoveryValue(int value, StatInflictionType recoveryType, bool isMagusBane, CharacterRank rank) {
            if (IsInflictionReduceableByBane(isMagusBane, recoveryType))
            {
                return GetMagickDamageReducedByMagusBane(rank, value);
            }

            //Unlike GetDamageReducedByDefense(), in this method there's no recovery reduction by defense stat at all
            return value;
        }

        private static int GetMagickDamageReducedByMagusBane(CharacterRank rank, int rawDamageValue) {
            switch (rank) {
                default:
                case CharacterRank.F:
                    {
                        return rawDamageValue; //no damage reduction whatsoever
                    }
                case CharacterRank.E:
                    {
                        return Mathf.RoundToInt((rawDamageValue <= 15) ? 0 : (rawDamageValue * 0.9f));
                    }
                case CharacterRank.D:
                    {
                        return Mathf.RoundToInt((rawDamageValue <= 30) ? 0 : (rawDamageValue * 0.75f));
                    }
                case CharacterRank.C:
                    {
                        return Mathf.RoundToInt((rawDamageValue <= 45) ? 0 : (rawDamageValue * 0.6f));
                    }
                case CharacterRank.B:
                    {
                        return Mathf.RoundToInt((rawDamageValue <= 60) ? 0 : (rawDamageValue * 0.5f));
                    }
                case CharacterRank.A:
                    {
                        return Mathf.RoundToInt((rawDamageValue <= 75) ? 0 : (rawDamageValue * 0.35f));
                    }
                case CharacterRank.S:
                    {
                        return Mathf.RoundToInt((rawDamageValue <= 90) ? 0 : (rawDamageValue * 0.2f));
                    }
                case CharacterRank.SS:
                    {
                        return 0; //fully nullifies any value of magick damage received
                    }
            }
        }

        public static string GetMagickDamageFeedbackOnMagusBane(CharacterRank rank, int reducedDamageValue) {
            if(reducedDamageValue == 0)
            {
                return "NO DAMAGE";
            }

            switch (rank)
            {
                default:
                case CharacterRank.E:
                case CharacterRank.D:
                case CharacterRank.C:
                case CharacterRank.B:
                case CharacterRank.A:
                case CharacterRank.S:
                    {
                        return "REDUCED";
                    }
                case CharacterRank.F:
                    {
                        return ""; //no feedback on purpose
                    }
                    //case CharacterRank.SS: //no need for this case. The if block above handles it already
                    //    {
                    //        return "NULL DAMAGE";
                    //    }
            }
        }
        
    }

}