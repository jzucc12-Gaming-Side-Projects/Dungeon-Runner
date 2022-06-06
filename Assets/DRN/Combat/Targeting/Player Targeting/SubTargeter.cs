using System;
using DRN.COMBAT.COMBATANT;
using JZ.INPUT;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DRN.COMBAT.TARGETING
{
    public class SubTargeter : MonoBehaviour
    {
        #region //Cached components
        private MenuingInputSystem inputSystem = null;
        #endregion

        #region //Active data
        private TargetData activeData = null;
        private Combatant target => activeData.singleTarget;
        private CombatAttacker[] attackers => target.GetAttackers();
        private int currentIndex = 0;
        private CombatAttacker currentAttacker => attackers[currentIndex];
        #endregion

        #region //Events
        public event Action<Combatant> StartSubTargeter;
        public event Action StopSubTargeter;
        public event Action<CombatAttacker, int> TargetChanged;
        #endregion

        
        #region //Monobehaviour
        private void Awake()
        {
            inputSystem = FindObjectOfType<MenuingInputSystem>();
        }
        #endregion

        #region //Targeting
        public void StartTargeting(TargetData data)
        {
            activeData = data;
            currentIndex = 0;
            inputSystem.yNavigate.started += ShiftIndex;
            StartSubTargeter?.Invoke(target);
            TargetChanged?.Invoke(currentAttacker, currentIndex);
        }

        public void StopTargeting()
        {
            currentIndex = 0;
            inputSystem.yNavigate.started -= ShiftIndex;
            activeData = null;
            StopSubTargeter?.Invoke();
        }

        private void ShiftIndex(InputAction.CallbackContext context)
        {
            int input = (int)context.ReadValue<float>();
            currentIndex = JZMathUtils.Wrap(currentIndex + input, 0, attackers.Length - 1);
            activeData.ccNumber = currentIndex;
            TargetChanged?.Invoke(currentAttacker, currentIndex);
        }
        #endregion

        #region //Getters
        public bool HasData() { return activeData != null; }
        #endregion
    }
}