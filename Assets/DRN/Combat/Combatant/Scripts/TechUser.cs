using DRN.STATS;
using UnityEngine;

namespace DRN.COMBAT.COMBATANT
{
    /// <summary>
    /// Contains all information for a combat unit, or combatant
    /// </summary>
    public class TechUser : CombatComponent
    {
        #region //Setters
        public override int GetStat(BodyStats stat)
        {
            return characterData.GetTotalStat(stat);
        }

        public override int GetStat(WeaponStats stat)
        {
            return characterData.GetTotalStat(stat);
        }
        #endregion

        #region //Damage and healing
        public void RestoreTP(int amount, bool message = false)
        {
            characterData.currentTP = (int)Mathf.MoveTowards(GetTP(), GetMaxTP(), amount);
        }

        public void SpendTP(int amount, bool message = false)
        {
            characterData.currentTP -= amount;
        }

        public bool TPCheck(int amount)
        {
            return characterData.currentTP >= amount;
        }
        #endregion
    }
}