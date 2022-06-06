using System.Collections;
using DRN.COMBAT.TARGETING;
using UnityEngine;

namespace DRN.COMBAT.ACTION
{
    public class DrainTPAction : IAction
    {
        private int minDrain = 0;
        private int maxDrain = 0;

        public DrainTPAction(int minDrain, int maxDrain)
        {
            this.minDrain = minDrain;
            this.maxDrain = maxDrain;
        }

        public IEnumerator Perform(TargetData data)
        {
            yield return data.attacker.AttackAnimation(AnimKeys.startAttack);
            foreach(var target in data.GetLivingTargets())
            {
                if(target.tech == null)
                {
                    target.SendCombatText("Immune");
                    continue;
                }

                (bool hit, bool crit) = DamageCalculator.RollHit(STATS.WeaponStats.techAffinity, data.attacker, target.body);
                if(!hit)
                {
                    target.SendCombatText("Miss!");
                    continue;
                }

                int amount = Random.Range(minDrain, maxDrain + 1);
                target.tech.SpendTP(amount);
                target.SendDamageText(amount," TP");
                data.attackerCombatant.tech.RestoreTP(amount);
                data.attackerCombatant.SendDamageText(-amount, " TP");
            }
            yield return data.attacker.AttackAnimation(AnimKeys.stopAttack);
        }
    }
}