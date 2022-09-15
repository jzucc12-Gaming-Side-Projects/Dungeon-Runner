using System;
using System.Collections.Generic;
using DRN.COMBAT.COMBATANT;
using JZ.INPUT;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DRN.COMBAT.TARGETING
{
    public class MainTargeter : MonoBehaviour
    {
        #region //Input
        public MenuingInputSystem inputSystem { get; private set; }
        #endregion

        #region //Targeting info
        private List<Combatant> players => CombatTarget.players;
        private List<Combatant> enemies => CombatTarget.enemies.GetLiving();
        private TargetData activeData = null;
        private int targetIndex = 0;
        #endregion

        #region //Targeting events
        public event Action TargetingStarted;
        public event Action<List<Combatant>> ChangeTarget;
        public event Action TargetingEnded;
        #endregion


        #region //Monobehaviour
        private void Awake()
        {
            inputSystem = FindObjectOfType<MenuingInputSystem>();
        }

        private void OnDisable()
        {
            DisableInputs();
        }
        #endregion

        #region //Start targeting
        public void StartTargeting(TargetData data, TargetingConfig config)
        {
            EnableInputs(config);
            TargetingStarted?.Invoke();
            SetUpTargetData(data, config);
            UpdateTargets();
        }

        public void EnableInputs(TargetingConfig config)
        {
            if(config.targetSelfOnly) return;
            inputSystem.yNavigate.started += ShiftTargets;

            if(config.canChangeSides)
                inputSystem.xNavigate.started += SwapSides;

            if(config.CanToggleAOE())
                inputSystem.tab.started += ToggleAOE;
        }

        private void SetUpTargetData(TargetData data, TargetingConfig config)
        {
            activeData = data;

            if(config.targetSelfOnly)
            {
                activeData.SetTargets(players, data.attackerCombatant);
            }
            else
            {
                var aoeTargets = new List<Combatant>(config.startOnAllies ? players : enemies);
                targetIndex = config.StartOnDead() ? aoeTargets.FindIndex(x => !x.IsAlive()) : 0;
                activeData.SetTargets(aoeTargets, targetIndex);
            }
        }
        #endregion

        #region //End targeting
        public void StopTargeting()
        {
            activeData = null;
            TargetingEnded?.Invoke();
            DisableInputs();
        }

        public void DisableInputs()
        {
            inputSystem.yNavigate.started -= ShiftTargets;
            inputSystem.xNavigate.started -= SwapSides;
            inputSystem.tab.started -= ToggleAOE;
        }
        #endregion

        #region //Change targets
        private void ShiftTargets(InputAction.CallbackContext context)
        {
            if(activeData.isAOE) return;
            int input = (int)context.ReadValue<float>();
            targetIndex = activeData.SetTargets(targetIndex + input);
            UpdateTargets();
        }

        private void SwapSides(InputAction.CallbackContext context)
        {
            int input = (int)context.ReadValue<float>();
            bool moveToPlayers = input > 0;
            activeData.SetTargets(moveToPlayers ? players : enemies);
            targetIndex = 0;
            UpdateTargets();
        }

        private void ToggleAOE(InputAction.CallbackContext context)
        {
            activeData.ToggleAOE();
            UpdateTargets();
        }

        private void UpdateTargets()
        {
            ChangeTarget?.Invoke(activeData.GetLivingTargets());
        } 
        #endregion

        public bool ShouldCancel()
        {
            if(activeData.IsOnAllies()) return false;

            int deadIndex = -1;
            for(int ii = 0; ii < activeData.GetTargets().Count; ii++)
            {
                if(activeData.GetTargets()[ii].IsAlive()) continue;
                deadIndex = ii;
            }

            if(deadIndex >= targetIndex)
                targetIndex = activeData.SetTargets(enemies, targetIndex);
            else
                targetIndex = activeData.SetTargets(enemies, targetIndex - 1);

            if(activeData.aoeTargets.Count == 0) return true;
            UpdateTargets();
            return false;
        }

        #region //Getters
        public bool HasData() => activeData != null;
        #endregion
    }
}