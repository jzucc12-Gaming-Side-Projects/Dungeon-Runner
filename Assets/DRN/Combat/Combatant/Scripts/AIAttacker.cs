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
        [SerializeField] private int weaponNo = -1;
        private CombatActionScheduler scheduler = null;
        public event Action<AIAttacker, int> OnAIReady = null;


        protected override void Awake()
        {
            base.Awake();
            scheduler = FindObjectOfType<CombatActionScheduler>();
            if(weaponNo == -1)
            {
                int count = 0;
                foreach(var attacker in transform.parent.GetComponentsInChildren<AIAttacker>())
                {
                    attacker.weaponNo = count;
                    count++;
                }
            }
        }

        protected override int GetWeaponNumber() => weaponNo;

        protected override void OnBBFull()
        {
            OnAIReady?.Invoke(this, turnNo);
        }

        public void SetUp(int number, CombatBody body)
        {
            weaponNo = number;
            this.body = body;
        }
    }
}