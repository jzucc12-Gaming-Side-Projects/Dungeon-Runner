namespace DRN.COMBAT.UI
{
    public class AICombatantUI : CombatantUI
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            targeter.ChangeTarget += SetTargeting;
            targeter.TargetingEnded += ResetTargeting;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            targeter.ChangeTarget -= SetTargeting;
            targeter.TargetingEnded -= ResetTargeting;
        }
    }
}
