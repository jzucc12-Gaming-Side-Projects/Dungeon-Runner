using DRN.COMBAT.COMBATANT;
using DRN.COMBAT.TARGETING;
using DRN.STATS;

namespace DRN.COMBAT.ACTION
{
    public class SlimeLogic : AIAttackLogic
    {
        protected override void AddActions()
        {
            var entry1 = new AttackEntry("Slap",
                                         new DamageRangeAttack(WeaponStats.strength, 0, 10, 20, 0, 0),
                                         new RandomFoe(),
                                         false,
                                         false);

            var entry2 = new AttackEntry("Sludge",
                                         new StatDamageAttack(WeaponStats.strength, 5, 2, 0, 0),
                                         new RandomFoe(),
                                         false,
                                         true);

            AddAction(entry1);
            AddAction(entry2);
        }

        protected override AttackEntry ChooseEntry(AIAttacker attacker, int turnNumber)
        {
            if(turnNumber % 4 != 0) return GetEntry("Slap");
            else return GetEntry("Sludge");
        }
    }
}
