using UnityEngine;

namespace DRN.COMBAT.ACTION
{
    [CreateAssetMenu(fileName = "Flee", menuName = "DRN/Action Factory/Flee Action Factory", order = 0)]
    public class FleeActionFactory : ActionFactory
    {
        [SerializeField] private bool isPlayer = false;


        public override IAction GetAction()
        {
            return new FleeAction(isPlayer);
        }
    }
}
