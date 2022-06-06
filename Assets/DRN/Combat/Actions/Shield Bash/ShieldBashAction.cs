using System.Collections;
using DRN.COMBAT.TARGETING;
using UnityEngine;

namespace DRN.COMBAT.ACTION
{
    public class ShieldBash : IAction
    {
        private static ActionVFX effect = null;

        public ShieldBash()
        {
            if(effect != null) return;
            effect = Resources.Load<ActionVFX>("Shield Bash VFX");
        }

        public IEnumerator Perform(TargetData data)
        {
            yield return data.attacker.AttackAnimation(AnimKeys.startAttack);

            foreach(var target in data.GetLivingTargets())
            {
                if(!target.HasAttackers()) continue;
                target.GetAttacker(data.ccNumber).ResetBB();
                GameObject.Instantiate(effect, target.transform.position, Quaternion.identity);
            }
            
            yield return data.attacker.AttackAnimation(AnimKeys.stopAttack);
        }
    }
}
