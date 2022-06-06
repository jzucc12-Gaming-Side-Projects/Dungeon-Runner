using System;
using DRN.STATS;
using UnityEngine;

namespace DRN.COMBAT.COMBATANT
{
    public class Combatant : CombatComponent
    {
        #region //Combat Components
        public CombatBody body { get; private set; }
        public TechUser tech  { get; private set; }
        private CombatAttacker[] attackers  = new CombatAttacker[0];
        #endregion

        #region //Events
        public event Action<string> ShowMessage;
        public event Action<int, string> ShowDamage;
        #endregion


        #region //Monobehaviour
        protected override void Awake()
        {
            base.Awake();
            body = GetComponentInChildren<CombatBody>();
            tech = GetComponentInChildren<TechUser>();
            attackers = GetComponentsInChildren<CombatAttacker>();
        }
        #endregion

        #region //Getters
        public CombatAttacker[] GetAttackers()
        {
            return attackers;
        }

        public CombatAttacker GetAttacker(int index = 0) 
        { 
            index = Mathf.Clamp(index, 0, attackers.Length - 1);
            return attackers[index]; 
        }

        public int GetAttackerNumber(CombatAttacker attacker)
        {
            for(int ii = 0; ii < attackers.Length; ii++)
            {
                if(attacker != attackers[ii]) continue;
                return ii;
            }
            return -1;
        }

        public override int GetStat(BodyStats stat)
        {
            return characterData.GetTotalStat(stat);
        }

        public override int GetStat(WeaponStats stat)
        {
            return characterData.GetTotalStat(stat);
        }
        #endregion

        #region //Combatant Info Methods
        public bool HasCC(CombatComponent cc)
        {
            return cc.transform.IsChildOf(transform);
        }

        public bool HasAttackers() { return attackers.Length > 0; }

        public bool HasMultipleAttackers() { return attackers.Length > 1; }
        #endregion
    
        #region //Messaging
        public void SendCombatText(string text)
        {
            ShowMessage?.Invoke(text);
        }

        public void SendDamageText(int damage, string suffix = "")
        {
            ShowDamage?.Invoke(damage, suffix);
        }
        #endregion
    }
}