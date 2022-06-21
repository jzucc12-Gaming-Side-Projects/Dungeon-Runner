using System.Collections.Generic;
using System;

namespace DRN.STATS
{
    /// <summary>
    /// Holds a set of stats typically used on player and enemy characters in combat
    /// </summary>
    
    [Serializable]
    public class StatList
    {
        public Dictionary<BodyStats, float> bodyStats = new Dictionary<BodyStats, float>();
        public Dictionary<WeaponStats, float> weaponStats = new Dictionary<WeaponStats, float>();


        public StatList(int startValue = 0)
        {
            foreach(BodyStats stat in Enum.GetValues(typeof(BodyStats)))
                bodyStats.Add(stat, startValue);
    
            foreach(WeaponStats stat in Enum.GetValues(typeof(WeaponStats)))
                weaponStats.Add(stat, startValue);
        }

        public StatList(StatList source)
        {
            this.bodyStats = source.bodyStats;
            this.weaponStats = source.weaponStats;
        }

        #region //Getters
        public int GetStat(BodyStats stat)
        {
            return (int)bodyStats[stat];
        }

        public int GetStat(WeaponStats stat)
        {
            return (int)weaponStats[stat];
        }
        #endregion
    }
}
