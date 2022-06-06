using System;
using DRN.COMBAT.COMBATANT;
using JZ.INPUT;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DRN.COMBAT.TARGETING
{
    /// <summary>
    /// Gives the player the ability to change who they are targeting via inputs
    /// </summary>
    public class PlayerTargetingHub : MonoBehaviour
    {
        #region //Input
        public MenuingInputSystem inputSystem { get; private set; }
        #endregion

        #region //Targeting
        public static event Action<TargetData> TargetsChosen;
        private TargetingConfig activeConfig = null;
        private TargetData activeData = null;
        private MainTargeter mainTargeter = null;
        private SubTargeter subTargeter = null;
        private bool mainTargeterOpen => mainTargeter.HasData();
        private bool subTargeterOpen => subTargeter.HasData();
        #endregion

        #region //Active player
        private ActivePlayerManager playerManager = null;
        #endregion


        #region //Monobehaviour
        private void Awake()
        {
            inputSystem = FindObjectOfType<MenuingInputSystem>();
            playerManager = GetComponent<ActivePlayerManager>();
            mainTargeter = GetComponent<MainTargeter>();
            subTargeter = GetComponent<SubTargeter>();
        }
        #endregion

        #region //Activation
        public void StartTargeting(TargetingConfig config)
        {
            CombatBody.OnBodyDiedStatic += OnBodyDeath;
            playerManager.ActivePlayerChanged += OnActivePlayerChanged;
            inputSystem.select.started += OnSelect;
            inputSystem.back.started += OnBack;
            playerManager.DisableInputs();

            activeConfig = config;
            activeData = new TargetData(playerManager.activeCombatant, playerManager.activeAttacker, config.IsAOE());

            mainTargeter.StartTargeting(activeData, config);
            if(config.autoSelect) OnSelect(new InputAction.CallbackContext());
        }

        private void SendData(TargetData data)
        {
            StopTargeting();
            TargetsChosen?.Invoke(data);
        }

        private void StopTargeting()
        {
            CombatBody.OnBodyDiedStatic -= OnBodyDeath;
            playerManager.ActivePlayerChanged -= OnActivePlayerChanged;
            inputSystem.select.started -= OnSelect;
            inputSystem.back.started -= OnBack;
            playerManager.EnableInputs();

            activeData = null;
            activeConfig = null;

            if(subTargeterOpen) subTargeter.StopTargeting();
            if(mainTargeterOpen) mainTargeter.StopTargeting();
        }
        #endregion

        #region //Targeting events
        private void OnSelect(InputAction.CallbackContext context)
        {
            if(NeedSubTargeter())
            {
                subTargeter.StartTargeting(activeData);
                mainTargeter.DisableInputs();
            }
            else
            {
                SendData(activeData);
            }
        }

        private bool NeedSubTargeter()
        {
            if(subTargeterOpen) return false;
            if(!activeConfig.needSpecificAttacker) return false;
            if(activeData.isAOE) return false;
            return activeData.singleTarget.HasMultipleAttackers();
        }

        private void OnBack(InputAction.CallbackContext context)
        {
            if(subTargeterOpen) CancelSubTargeting();
            else SendData(null);
        }

        private void OnBodyDeath()
        {
            Combatant startingTarget = activeData.singleTarget;
            if(mainTargeter.ShouldCancel()) 
            {
                SendData(null);
            }
            else if(subTargeterOpen && startingTarget != activeData.singleTarget)
            {
                CancelSubTargeting();
            }
        }

        private void CancelSubTargeting()
        {
            subTargeter.StopTargeting();
            activeData.ccNumber = 0;
            mainTargeter.EnableInputs(activeConfig);
        }

        private void OnActivePlayerChanged()
        {
            if(playerManager.activeAttacker == activeData.attacker) return;
            SendData(null);
        }
        #endregion
    }
}