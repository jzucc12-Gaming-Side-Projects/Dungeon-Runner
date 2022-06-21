using DRN.COMBAT.COMBATANT;
using DRN.COMBAT.TARGETING;
using DRN.STATS;

namespace DRN.COMBAT.ACTION
{
    public class GiantRatLogic : AIAttackLogic
    {
        protected override void AddActions()
        {
            var entry1 = new AttackEntry("Bite",
                                         new StatDamageAttack(WeaponStats.dexterity, 3, 2, 2, 0.1f),
                                         new RandomFoe(),
                                         false,
                                         false);


            var entry2 = new AttackEntry("Flee",
                                          new FleeAction(false),
                                          new TargetSelf(),
                                          true,
                                          false);

            AddAction(entry1);
            AddAction(entry2);
        }

        protected override AttackEntry ChooseEntry(AIAttacker attacker, int turnNumber)
        {
            if(attacker.GetHPPercentage() > 0.25f) return GetEntry("Bite");
            else if(turnNumber % 2 == 0) return GetEntry("Bite");
            else return GetEntry("Flee");
        }
    }
}
