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
        public Dictionary<WeaponStats, float> weaponStats = new Dictionary<WeaponStats, float>();
        public Dictionary<BodyStats, float> bodyStats = new Dictionary<BodyStats, float>();


        public StatList()
        {
            foreach(WeaponStats stat in Enum.GetValues(typeof(WeaponStats)))
                weaponStats.Add(stat, 10);

            foreach(BodyStats stat in Enum.GetValues(typeof(BodyStats)))
                bodyStats.Add(stat, 10);
        }

        public StatList(StatList source)
        {
            this.weaponStats = source.weaponStats;
            this.bodyStats = source.bodyStats;
        }

        #region //Getters
        public int GetStat(WeaponStats stat)
        {
            return (int)weaponStats[stat];
        }

        public int GetStat(BodyStats stat)
        {
            return (int)bodyStats[stat];
        }
        #endregion
    }
}
