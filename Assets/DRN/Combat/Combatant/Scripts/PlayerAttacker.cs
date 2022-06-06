using System;
using UnityEngine;

namespace DRN.COMBAT.COMBATANT
{
    /// <summary>
    /// Player specific combatant info
    /// </summary>
    public class PlayerAttacker : CombatAttacker
    {
        [SerializeField] private int displayPriority = 0;
        [SerializeField] private bool isLeftHand = true;
        public event Action<PlayerAttacker> OnPlayerReady = null;
        public event Action<PlayerAttacker> OnPlayerUnReadied = null;


        protected override int GetWeaponNumber() => isLeftHand ? 0 : 1;

        protected override void OnBBFull()
        {
            base.OnBBFull();
            OnPlayerReady?.Invoke(this);
        }

        protected override void OnBodyDeath()
        {
            base.OnBodyDeath();
            OnPlayerUnReadied?.Invoke(this);
        }

        public override void ResetBB()
        {
            base.ResetBB();
            OnPlayerUnReadied?.Invoke(this);
        }

        public int GetDisplayPriority() { return displayPriority; }

    }
}
