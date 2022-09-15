using System.Collections.Generic;
using DRN.COMBAT.COMBATANT;

namespace DRN.COMBAT.TARGETING
{
    public class TargetData
    {
        #region //Variables
        public Combatant attackerCombatant {get; private set; }
        public CombatAttacker attacker {get; private set; }
        public Combatant singleTarget { get; private set; }
        public List<Combatant> aoeTargets { get; private set; }
        public bool isAOE { get; private set; }
        public int ccNumber = 0;
        #endregion


        #region //Constructors
        public TargetData() 
        { 
            this.attackerCombatant = null;
            this.attacker = null;
            this.singleTarget = null;
            this.aoeTargets = new List<Combatant>();
            this.ccNumber = 0;
            this.isAOE = false;
        }

        public TargetData(Combatant attackerCombatant, CombatAttacker attacker, bool isAOE = false)
        {
            this.attackerCombatant = attackerCombatant;
            this.attacker = attacker;
            this.singleTarget = null;
            this.aoeTargets = new List<Combatant>();
            this.ccNumber = 0;
            this.isAOE = isAOE;
        }
        #endregion

        #region //Setters
        public int SetTargets(List<Combatant> targets, int index = 0)
        {
            aoeTargets = new List<Combatant>(targets);
            return SetTargets(index);
        }

        public void SetTargets(List<Combatant> targets, Combatant singleTarget)
        {
            aoeTargets = new List<Combatant>(targets);
            SetTargets(singleTarget);
        }

        public int SetTargets(int index)
        {
            index = JZMathUtils.Wrap(index, 0, aoeTargets.Count - 1);
            singleTarget = aoeTargets[index];
            return index;
        }

        public void SetTargets(Combatant target)
        {
            singleTarget = target;
        }

        public void ToggleAOE() => isAOE = !isAOE;
        #endregion

        #region //Get living targets
        public List<Combatant> GetLivingTargets()
        {
            if(isAOE)
            {
                return new List<Combatant>(aoeTargets.GetLiving());
            }
            else
            {
                return new List<Combatant>() { GetLivingFromSingleTarget() };
            }
        }

        private Combatant GetLivingFromSingleTarget()
        {
            if(!IsOnAllies() && !singleTarget.IsAlive())
                return aoeTargets.GetLiving()[0];

            return singleTarget;
        }

        public List<Combatant> GetTargets() => aoeTargets;
        #endregion

        #region //Targeting info
        public bool IsOnAllies()
        {
            return aoeTargets.Contains(attackerCombatant);
        }
        #endregion
    }
}