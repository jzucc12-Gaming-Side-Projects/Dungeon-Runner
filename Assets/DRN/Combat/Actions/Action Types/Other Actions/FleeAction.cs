using System.Collections;
using System.Collections.Generic;
using DRN.COMBAT.COMBATANT;
using DRN.COMBAT.TARGETING;
using DRN.STATS;
using UnityEngine;

namespace DRN.COMBAT.ACTION
{
    public class FleeAction : IAction
    {
        private bool isPlayer = false;
        public static event System.Action<string, float> FleeAttemptMessage;
        private float messageTime = 1f;


        public FleeAction(bool isPlayer)
        {
            this.isPlayer = isPlayer;
        }

        public IEnumerator Perform(TargetData data)
        {
            yield return new WaitForSeconds(0.5f);
            var targets = data.GetLivingTargets();

            float speed = 0;
            foreach(var target in targets)
                speed += target.GetStat(WeaponStats.speed);

            var opposition = isPlayer ? CombatTarget.enemies.GetLiving() : CombatTarget.players.GetLiving();
            float pressure = 0;
            foreach(var opposer in opposition)
                pressure += opposer.GetStat(WeaponStats.speed);

            int roll = Random.Range(0, 50 + 1);
            if(roll + speed >= pressure/10000)
            {
                if(!isPlayer)
                    EnemyFleeMessaging(targets);
                else
                    FleeAttemptMessage?.Invoke("You fled from combat", messageTime);

                yield return new WaitForSeconds(messageTime/2);
                foreach(var target in targets)
                    target.gameObject.SetActive(false);
                yield return new WaitForSeconds(messageTime/2);
            }
            else if(isPlayer)
            {
                FleeAttemptMessage?.Invoke("You couldn't flee", messageTime);
                yield return new WaitForSeconds(messageTime);
            }

        }

        private void EnemyFleeMessaging(List<Combatant> targets)
        {
            if(targets.Count > 0)
                FleeAttemptMessage?.Invoke($"{targets[0].GetName()} fled", messageTime);
            else
                FleeAttemptMessage?.Invoke("Enemies fled", messageTime);
        }
    }
}
