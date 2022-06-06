namespace DRN.STATS
{
    /// <summary>
    /// Enum containing all stats required for weapon attacks
    /// </summary>
    public enum WeaponStats
    {
        strength = 0, //Melee damage
        dexterity = 1, //Ranged damage
        techAffinity = 2, //Tech damage
        speed = 3, //BB speed and escape capability
        aim = 4, //Accuracy
        crit = 5 //Crit rate and damage
    }

    /// <summary>
    /// Enum containing all character body stats
    /// </summary>
    public enum BodyStats
    {
        maxHP = 0, //Max health points
        maxTP = 1, //Max tech points
        physicalRes = 2, //Phyiscal damage resistance
        techRes = 3, //Tech damage resistance
        dodge = 4, //Avoiding attacks
        fortune = 5 //item drop boosts, crit rates, attack missing
    }
}