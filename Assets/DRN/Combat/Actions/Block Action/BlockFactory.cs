using UnityEngine;

namespace DRN.COMBAT.ACTION
{
    [CreateAssetMenu(fileName = "Block", menuName = "DRN/Action Factory/Block Factory", order = 0)]
    public class BlockFactory : ActionFactory
    {
        public override IAction GetAction()
        {
            return new BlockAction();
        }
    }
}