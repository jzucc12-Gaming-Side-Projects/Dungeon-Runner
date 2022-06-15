using DRN.COMBAT.TARGETING;
using DRN.STATS;
using UnityEngine;

namespace DRN.COMBAT.ACTION
{
    public class GoblinShieldLogic : AIAttackLogic
    {
        protected override void AddActions()
        {
            var entry1 = new AttackEntry("Block",
                                          new BlockAction(),
                                          new TargetSelf(),
                                          false,
                                          false);

            var entry2 = new AttackEntry("Shield Bash",
                                         new FixedDamageAttack(WeaponStats.strength, 0, 10, 0, 0),
                                         new LowestHP(),
                                         false,
                                         true);

            AddAction(entry1);
            AddAction(entry2);
        }

        protected override AttackEntry ChooseEntry(int turnNumber)
        {
            if(attacker.GetHPPercentage() > 0.5f) return new AttackEntry();
            if(turnNumber % 3 == 1) return new AttackEntry();

            int roll = Random.Range(0, 101);
            if(roll > 50) return GetEntry("Block");
            else return GetEntry("Shield Bash");
        }
    }
}
