using System;
using DRN.STATS;
using UnityEngine;

namespace DRN.COMBAT.COMBATANT
{
    /// <summary>
    /// Contains all information for a combat unit, or combatant
    /// </summary>
    public class CombatBody : CombatComponent
    {
        #region //Combatant State
        protected bool isBlocking = false;
        [SerializeField] private bool useOverkill = false;
        public event Action OnBodyDied;
        public static event Action OnBodyDiedStatic;
        public event Action OnBodyRevived;
        #endregion


        #region //Getters
        public bool IsBlocking() { return isBlocking; }
        #endregion

        #region //Setters
        public void SetBlocking(bool block) { isBlocking = block; }

        public override int GetStat(BodyStats stat)
        {
            return characterData.GetTotalStat(stat);
        }

        public override int GetStat(WeaponStats stat)
        {
            return characterData.GetTotalStat(stat);
        }
        #endregion

        #region //Damage and healing
        public void Heal(int amount)
        {
            characterData.currentHP = (int)Mathf.MoveTowards(GetHP(), GetMaxHP(), amount);
        }

        public void Revive(int amount)
        {
            if(characterData.IsAlive()) return;
            Heal(amount);
            OnBodyRevived?.Invoke();
        }

        public void TakeDamage(int damage)
        {
            if(!IsAlive()) return;
            int startingHP = characterData.currentHP;
            characterData.currentHP -= damage;
            if(characterData.currentHP <= 0 && OverkillCheck(startingHP))
            {
                OnBodyDiedStatic?.Invoke();
                OnBodyDied?.Invoke();
                characterData.currentHP = 0;
                foreach(var sr in GetComponentsInChildren<SpriteRenderer>())
                    sr.enabled = false;
            }
        }

        private bool OverkillCheck(int startingHP)
        {
            if(!useOverkill) return true;
            if(startingHP == 1) return true;

            int overflow = Mathf.Abs(characterData.currentHP);
            if(overflow >= characterData.GetTotalStat(BodyStats.maxHP)/2)
            {
                return true;
            }
            else
            {
                characterData.currentHP = 1;
                return false;
            }
        }
        #endregion
    }
}

//TODO
//Improve sprite visibility when a player target dies