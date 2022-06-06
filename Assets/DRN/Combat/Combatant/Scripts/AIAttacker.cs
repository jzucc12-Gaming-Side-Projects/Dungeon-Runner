using System;
using DRN.COMBAT.ACTION;
using UnityEngine;

namespace DRN.COMBAT.COMBATANT
{
    /// <summary>
    /// AI specific combatant info
    /// </summary>
    public class AIAttacker : CombatAttacker
    {
        [SerializeField] private int weaponNo = 0;
        private CombatActionScheduler scheduler = null;
        public event Action<int> OnAIReady = null;


        protected override void Awake()
        {
            base.Awake();
            scheduler = FindObjectOfType<CombatActionScheduler>();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override int GetWeaponNumber() => weaponNo;

        protected override void OnBBFull()
        {
            OnAIReady?.Invoke(turnNo);
        }
    }
}