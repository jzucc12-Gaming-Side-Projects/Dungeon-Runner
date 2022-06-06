using System.Collections.Generic;
using DRN.COMBAT.COMBATANT;
using UnityEngine;

namespace DRN.COMBAT.TARGETING
{

    public abstract class AITargeting
    {
        public abstract void GetTargets(TargetData data);
    }

    public class RandomFoe : AITargeting
    {
        public override void GetTargets(TargetData data)
        {
            var options = CombatTarget.players.GetLiving();
            var target = options[Random.Range(0, options.Count)];
            data.SetTargets(options, target);
        }
    }

    public class LowestHP : AITargeting
    {
        public override void GetTargets(TargetData data)
        {
            var options = CombatTarget.players.GetLiving();
            List<(Combatant, float)> list = new List<(Combatant, float)>();
            foreach(var option in options)
            {
                list.Add((option, option.GetHP()));
            }

            list.Sort((x, y) => 
            {
                if(x.Item2 > y.Item2) return 1;
                else if(x.Item2 < y.Item2) return -1;
                else return 0;
            });
            data.SetTargets(options, list[0].Item1);
        }
    }

    public class HighestBB : AITargeting
    {
        public override void GetTargets(TargetData data)
        {
            var options = CombatTarget.players.GetLiving();
            List<(Combatant, CombatAttacker, int)> list = new List<(Combatant, CombatAttacker, int)>();
            foreach(var option in options)
            {
                int count = 0;
                foreach(var attack in option.GetAttackers())
                {
                    list.Add((option, attack, count));
                    count++;
                }
            }

            list.Sort((x, y) =>
            {
                if(x.Item2.GetBBPercentage() > y.Item2.GetBBPercentage()) return -1;
                else if(x.Item2.GetBBPercentage() < y.Item2.GetBBPercentage()) return 1;
                else return 0;
            });

            data.SetTargets(options, list[0].Item1);
            data.ccNumber = list[0].Item3;
        }
    }

    public class AOEAttack : AITargeting
    {
        private bool targetPlayers = false;
        public AOEAttack(bool targetFoes)
        {
            this.targetPlayers = targetFoes;
        }
        
        public override void GetTargets(TargetData data)
        {
            var options = targetPlayers ? CombatTarget.players.GetLiving() : CombatTarget.enemies;
            data.SetTargets(options);
            data.ToggleAOE();
        }
    }

    public class TargetSelf : AITargeting
    {
        public override void GetTargets(TargetData data)
        {
            var options = CombatTarget.enemies;
            data.SetTargets(options, data.attackerCombatant);
        }
    }
}

//TODO
//Stop options from being the first line of every child
//Find a more convenient way to do sorting