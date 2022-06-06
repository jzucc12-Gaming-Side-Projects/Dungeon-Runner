using System;
using System.Collections;
using DRN.COMBAT.TARGETING;
using UnityEngine;

namespace DRN.COMBAT.ACTION
{
    public struct SchedulerItem
    {
        #region //Variables
        private IAction action;
        private TargetData data;
        private bool highPriority;
        private string combatText;
        private float messageTime;
        public static event Action<string, float> DisplayText = null;
        #endregion


        #region //Constructors
        public SchedulerItem(TargetData data, IAction action, bool highPriority = false, string combatText = "")
        {
            this.data = data;
            this.action = action;
            this.highPriority = highPriority;
            this.combatText = combatText;
            messageTime = 1;
        }

        public SchedulerItem(TargetData data, IAction action, string combatText = "", bool highPriority = false)
        {
            this.data = data;
            this.action = action;
            this.highPriority = highPriority;
            this.combatText = combatText;
            messageTime = 1;
        }
        #endregion
        
        #region //Performing
        public IEnumerator Perform()
        {
            if(!string.IsNullOrEmpty(combatText))
            {
                DisplayText?.Invoke(combatText, messageTime);
                yield return new WaitForSeconds(messageTime / 2);
            }

            yield return action.Perform(data);
            data.attacker.ResetBB();
        }
        #endregion

        #region //Item state
        public bool ItemValid() => data.attackerCombatant.IsAlive() && data.attacker.GetBBPercentage() == 1;
        public bool HighPriority() => highPriority;
        #endregion
    }
}