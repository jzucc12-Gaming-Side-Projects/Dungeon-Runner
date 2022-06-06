using UnityEngine;
using JZ.MENU.BUTTON;
using DRN.COMBAT.TARGETING;
using DRN.COMBAT.COMBATANT;

namespace DRN.COMBAT.ACTION
{
    /// <summary>
    /// Player menu button for starting an action.
    /// </summary>
    public class ActionFactoryButton : ButtonFunction
    {
        [SerializeField] private ActionFactory factorySO;
        private CombatActionScheduler scheduler = null;
        protected PlayerTargetingHub targeter = null;
        protected ActivePlayerManager playerManager = null;


        protected override void Awake()
        {
            base.Awake();
            playerManager = FindObjectOfType<ActivePlayerManager>();
            targeter = FindObjectOfType<PlayerTargetingHub>();
            scheduler = FindObjectOfType<CombatActionScheduler>();
        }

        public override void OnClick()
        {
            PlayerTargetingHub.TargetsChosen += TargetingEnded;
            targeter.StartTargeting(factorySO.targetingConfig);
        }

        private void TargetingEnded(TargetData data)
        {
            PlayerTargetingHub.TargetsChosen -= TargetingEnded;
            if(data != null) QueueAction(data);
        }

        private void QueueAction(TargetData data)
        {
            scheduler.AddAction(new SchedulerItem(data, factorySO.GetAction(), factorySO.highPriority));
            playerManager.RemoveReadiedPlayer((PlayerAttacker)data.attacker);
        }
    }
}