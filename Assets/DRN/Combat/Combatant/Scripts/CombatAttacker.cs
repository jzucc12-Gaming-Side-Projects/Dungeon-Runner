using System.Collections;
using DRN.CHARACTER;
using DRN.STATS;
using UnityEngine;

namespace DRN.COMBAT.COMBATANT
{
    public abstract class CombatAttacker : CombatComponent
    {
        #region //CombatComponents
        [SerializeField] private CombatBody body = null;
        private Animator animator = null;
        private string weaponName = "";
        #endregion

        #region //Battle Bar (BB) variables
        private float currentBB = 0;
        private const int maxBB = 100;
        protected bool bbMaxed => currentBB == maxBB;
        protected int turnNo = 1;
        #endregion


        #region //Monobehaviour
        protected override void Start() 
        { 
            base.Start(); 
            body.OnBodyDied += OnBodyDeath;
            body.OnBodyRevived += OnBodyRevive;
        }
        
        protected virtual void FixedUpdate()
        {
            UpdateBB();
        }
        #endregion

        #region //Getters
        public float GetBBPercentage() { return currentBB / maxBB; }

        public override int GetStat(BodyStats stat)
        {
            return characterData.GetTotalStat(stat);
        }

        public override int GetStat(WeaponStats stat)
        {
            return characterData.GetWeaponStat(stat, GetWeaponNumber());
        }

        public string GetWeaponName()
        {
            return weaponName;
        }

        protected abstract int GetWeaponNumber();
        #endregion

        #region //Battle Bar
        public virtual void ResetBB()
        {
            currentBB = 0;
            turnNo++;
        }

        private void UpdateBB()
        {
            if(bbMaxed) return;
            currentBB = Mathf.Min(currentBB + GetBBIncrease(), maxBB);

            if(bbMaxed)
                OnBBFull();
        }

        protected virtual void OnBBFull()
        {
            body.SetBlocking(false);
        }

        private float GetBBIncrease()
        {
            return GetStat(WeaponStats.speed) * Time.deltaTime;
        }
        #endregion

        #region //Player events
        protected virtual void OnBodyDeath()
        {
            ResetBB();
            enabled = false;
        }

        protected virtual void OnBodyRevive()
        {
            enabled = true;
        }
        #endregion

        #region //Animation
        public void SetAnimator(Animator animator) => this.animator = animator;
        public IEnumerator AttackAnimation(string triggerName)
        {
            if(animator == null) yield break;
            if(animator.GetCurrentAnimatorClipInfoCount(0) == 0) yield break;
            animator.SetTrigger(triggerName);
            yield return null;
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        }
        #endregion

        #region //Override
        public override void ReceiveData(CharacterData data)
        {
            base.ReceiveData(data);
            weaponName = data.weapons[GetWeaponNumber()].weaponName;
        }
        #endregion
    }
}

//TODO
//More properly display weapon name once equipment is added