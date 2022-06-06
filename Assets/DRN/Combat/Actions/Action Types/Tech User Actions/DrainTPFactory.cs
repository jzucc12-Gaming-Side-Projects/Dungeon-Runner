using UnityEngine;

namespace DRN.COMBAT.ACTION
{
    [CreateAssetMenu(fileName = "Drain TP", menuName = "DRN/Action Factory/Drain TP Factory", order = 0)]
    public class DrainTPFactory : ActionFactory
    {
        public override IAction GetAction()
        {
            return new DrainTPAction(5, 15);
        }
    }
}