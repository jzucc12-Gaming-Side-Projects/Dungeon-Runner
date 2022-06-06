using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JZ.INPUT.REBIND
{
    /// <summary>
    /// <para>Performs control rebinding</para>
    /// </summary>
    public class RebindManager : MonoBehaviour
    {
        #region //Events
        public static event Action<InputAction> RebindingStarted;
        public static event Action<RebindData> RebindingComplete;
        public static event Action RebindingFinished;
        public static event Action OnStartListening;
        public static event Action OnStopListening;
        #endregion

        private InputAction pauseAction => InputManager.generalInputs.Map.Pause;


        #region //Monobehaviour
        private void Start()
        {
            LoadRebindings();
        }
        #endregion

        #region //Rebinding
        //Public
        public void Rebind(InputAction action, int index)
        {
            if(action == null || index < 0) return;
            pauseAction.performed -= StopListening;
            RebindingStarted?.Invoke(action);

            //Set up rebind action
            var rebind = action.PerformInteractiveRebinding(index).
                                 WithMatchingEventsBeingSuppressed(true).
                                 WithMagnitudeHavingToBeGreaterThan(0.99f);

            RebindData data;
            data.oldBinding = action.bindings[index];
            data.bindingIndex = index;
            
            //Saves rebind and updates UI
            rebind.OnComplete(op => 
            {
                pauseAction.performed += StopListening;
                data.action = op.action;
                data.newBinding = op.action.bindings[index];
                RebindingComplete?.Invoke(data);
                RebindingFinished?.Invoke();
                op.Dispose();
                SaveRebind(op.action);
            });

            //Stops rebind
            rebind.OnCancel(op => 
            {
                pauseAction.performed += StopListening;
                RebindingFinished?.Invoke();
                op.Dispose();
            });

            //Input type specific restrictions
            if(action.bindings[index].groups.Contains("Keyboard"))
                SetRebindForPC(rebind);
            else
                SetRebindForGamepad(rebind);

            rebind.Start();
        }

        //Private
        private void SetRebindForGamepad(InputActionRebindingExtensions.RebindingOperation rebind)
        {
            rebind.
            WithControlsExcluding("Mouse").
            WithControlsExcluding("Keyboard").
            WithControlsExcluding("<Gamepad>/dpad").
            WithControlsExcluding("<Gamepad>/leftStick").
            WithControlsExcluding("<Gamepad>/rightStick").
            WithCancelingThrough("<Gamepad>/start");
        }

        private void SetRebindForPC(InputActionRebindingExtensions.RebindingOperation rebind)
        {
            rebind.
            WithControlsExcluding("Gamepad").
            WithControlsExcluding("<Keyboard>/leftArrow").
            WithControlsExcluding("<Keyboard>/rightArrow").
            WithControlsExcluding("<Keyboard>/upArrow").
            WithControlsExcluding("<Keyboard>/downArrow").
            WithControlsExcluding("<Keyboard>/anyKey").
            WithCancelingThrough("<Keyboard>/escape");
        }
        #endregion

        #region //Rebinding saving
        //Public
        public void ResetAllBindings()
        {
            foreach(var action in GetAllActions())
            {
                action.RemoveAllBindingOverrides();
                for(int ii = 0; ii < action.bindings.Count; ii++)
                {
                    PlayerPrefs.DeleteKey(GetPlayerPrefKey(action,ii));
                }
            }

            RebindingFinished?.Invoke();
        }

        public void SaveRebind(InputAction action)
        {
            for(int ii = 0; ii < action.bindings.Count; ii++)
            {
                PlayerPrefs.SetString(GetPlayerPrefKey(action,ii), action.bindings[ii].overridePath);
            }
        }

        //Private
        private void LoadRebindings()
        {
            foreach(var action in GetAllActions())
            {
                int count = 0;
                for(int ii = 0; ii < action.bindings.Count; ii++)
                {
                    string overridePath = PlayerPrefs.GetString(GetPlayerPrefKey(action,ii));
                    if(string.IsNullOrEmpty(overridePath)) continue;
                    action.ApplyBindingOverride(ii, overridePath);
                    count++;
                }
            }

            RebindingFinished?.Invoke();
        }

        private string GetPlayerPrefKey(InputAction action, int num)
        {
            return $"{action.actionMap} {action.name} {num}";
        }

        private IEnumerable<InputAction> GetAllActions()
        {
            //Get all assets from the input manager
            foreach(var asset in InputManager.GetAssets())
            {
                //Cycle through all action maps
                foreach(var map in asset.actionMaps)
                {
                    //Cycle through all actions
                    foreach(var action in map.actions)
                        yield return action;
                }
            }
        }
        #endregion
    
        #region //Rebinding Listening
        //Public

        public void StartListening()
        {
            //Game is now listening for rebinding inputs
            pauseAction.performed += StopListening;
            OnStartListening?.Invoke();
        }

        //Private
        private void StopListening(InputAction.CallbackContext context)
        {
            //Game is no longer listening for rebinding inputs
            OnStopListening?.Invoke();
        }
        #endregion
    }
}