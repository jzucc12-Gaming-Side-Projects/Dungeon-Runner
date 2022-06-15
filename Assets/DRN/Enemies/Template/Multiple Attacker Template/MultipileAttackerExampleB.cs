using DRN.COMBAT.TARGETING;
using DRN.STATS;

namespace DRN.COMBAT.ACTION
{
    public class MultipileAttackerExampleB : AIAttackLogic
    {
        protected override void AddActions()
        {
            var entry1 = new AttackEntry("Attack 3",
                                         new StatDamageAttack(WeaponStats.dexterity, 3, 2, 2, 0.1f),
                                         new RandomFoe(),
                                         false,
                                         false);


            var entry2 = new AttackEntry("Attack 4",
                                          new FleeAction(false),
                                          new TargetSelf(),
                                          true,
                                          false);

            AddAction(entry1);
            AddAction(entry2);
        }

        protected override AttackEntry ChooseEntry(int turnNumber)
        {
            if(attacker.GetHPPercentage() > 0.25f) return GetEntry("Attack 3");
            else return GetEntry("Attack 4");
        }
    }
}
