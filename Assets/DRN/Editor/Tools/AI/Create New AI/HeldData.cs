using System;
using System.Collections.Generic;
using DRN.COMBAT.COMBATANT;

namespace DRN.TOOLS.AI
{
    [Serializable]
    public class HeldData
    {
        public Combatant baseAI { get; private set; }
        public List<string> scriptNames { get; private set; }
        private List<int> indices = new List<int>();
        private  List<AIAttacker> attackers = new List<AIAttacker>();

        public HeldData(Combatant aiBase)
        {
            this.baseAI = aiBase;
            this.scriptNames = new List<string>();
        }

        public void AddScriptName(string scriptName)
        {
            scriptNames.Add(scriptName);
        }

        public void AddAtkr(int index, AIAttacker attacker)
        {
            indices.Add(index);
            attackers.Add(attacker);
        }

        public IEnumerable<AIAttacker> GetAttackers(int index)
        {
            for(int ii = 0; ii < attackers.Count; ii++)
            {
                if(indices[ii] == index) yield return attackers[ii];
            }
        }
    }
}
