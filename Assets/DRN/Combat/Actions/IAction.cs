using System.Collections;
using DRN.COMBAT.TARGETING;

namespace DRN.COMBAT.ACTION
{
    public interface IAction
    {
        IEnumerator Perform(TargetData data);
    }
}