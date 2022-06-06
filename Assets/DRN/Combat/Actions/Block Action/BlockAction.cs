using System.Collections;
using DRN.COMBAT.TARGETING;
using UnityEngine;

namespace DRN.COMBAT.ACTION
{
    public class BlockAction : IAction
    {
        private static ActionVFX effect = null;

        public BlockAction()
        {
            if(effect != null) return;
            effect = Resources.Load<ActionVFX>("Block VFX");
        }

        public IEnumerator Perform(TargetData data)
        {
            yield return data.attacker.AttackAnimation(AnimKeys.startAttack);
            foreach(var target in data.GetLivingTargets())
            {
                if(target.body == null) continue;
                target.body.SetBlocking(true);
                GameObject.Instantiate(effect, target.transform.position, Quaternion.identity);
            }
            yield return data.attacker.AttackAnimation(AnimKeys.stopAttack);
        }
    }
}
