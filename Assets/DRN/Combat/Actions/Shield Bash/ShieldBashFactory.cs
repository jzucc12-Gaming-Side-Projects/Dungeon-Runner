using UnityEngine;

namespace DRN.COMBAT.ACTION
{
    [CreateAssetMenu(fileName = "Shield Bash", menuName = "DRN/Action Factory/Shield Bash Factory", order = 0)]
    public class ShieldBashFactory : ActionFactory
    {
        public override IAction GetAction()
        {
            return new ShieldBash();
        }
    }
}