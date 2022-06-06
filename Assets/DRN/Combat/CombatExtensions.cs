using System.Collections.Generic;

namespace DRN.COMBAT.COMBATANT
{
    public static class CombatExtensions
    {
        public static List<Combatant> GetLiving(this IEnumerable<Combatant> targets)
        {
            List<Combatant> list = new List<Combatant>();
            foreach(var target in targets)
            {
                if(!target.IsAlive()) continue;
                list.Add(target);
            }
            return list;
        }
    }
}
