using DRN.COMBAT.COMBATANT;
using DRN.COMBAT.TARGETING;
using DRN.STATS;
using UnityEngine;

namespace DRN.COMBAT.ACTION
{
    public class SkeletonSwordLogic : AIAttackLogic
    {
        protected override void AddActions()
        {
            var entry1 = new AttackEntry("Spin Attack",
                                         new DamageRangeAttack(WeaponStats.dexterity, -5, 50, 75, 0, 0),
                                         new AOEAttack(true),
                                         false,
                                         true);

            var entry2 = new AttackEntry("Slash",
                                         new StatDamageAttack(WeaponStats.strength, 5, 3, 0, 0),
                                         new RandomFoe(),
                                         false,
                                         false);
            AddAction(entry1);
            AddAction(entry2);
        }

        protected override AttackEntry ChooseEntry(AIAttacker attacker, int turnNumber)
        {
            int roll = Random.Range(0, 101);
            if(turnNumber % 3 == 1)
            {
                return GetEntry("Slash");
            }
            else if(turnNumber % 3 == 2)
            {
                if(roll > 75) return GetEntry("Spin Attack");
                else return GetEntry("Slash");
            }
            else
            {
                if(roll > 50) return GetEntry("Spin Attack");
                else return GetEntry("Slash");
            }
        }
    }
}