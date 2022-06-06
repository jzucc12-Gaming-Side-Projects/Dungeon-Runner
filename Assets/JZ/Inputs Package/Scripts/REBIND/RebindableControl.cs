using UnityEngine;
using UnityEngine.InputSystem;

namespace JZ.INPUT.REBIND
{
    /// <summary>
    /// <para>Game objects with this script can have their controls rebinded</para>
    /// </summary>
    public class RebindableControl : MonoBehaviour
    {
        #region //Input info
        private InputAction onInput = null;
        private IControlInfo controlInfo = null;
        private InputAction action = null;
        private int bindingIndex = 0;
        private RebindManager rebindManager = null;
        #endregion


        #region //Monobehaviour
        private void Awake()
        {
            onInput = new InputAction(binding: "/*/<button>");
            rebindManager = FindObjectOfType<RebindManager>();
        }

        private void OnEnable()
        {
            RebindManager.OnStartListening += onInput.Enable;
            RebindManager.OnStopListening += onInput.Disable;
            RebindManager.RebindingComplete += CheckRebindSwap;
            onInput.performed += TryRebind;
        }

        private void OnDisable()
        {
            RebindManager.OnStartListening -= onInput.Enable;
            RebindManager.OnStopListening -= onInput.Disable;
            RebindManager.RebindingComplete -= CheckRebindSwap;
            onInput.performed -= TryRebind;
            onInput.Disable();
        }

        private void Start()
        {
            controlInfo = GetComponent<IControlInfo>();
            action = controlInfo.GetAction();
            bindingIndex = controlInfo.GetBindingIndex();
        }
        #endregion

        #region //Rebinding
        private void TryRebind(InputAction.CallbackContext context)
        {
            //Abort if there is not enough actuation
            //Needed because gamepad triggers are weird
            if(context.control.device is Gamepad && context.ReadValue<float>() < 1f) return;

            //Abort if this action does not contain this binding
            var binding = action.GetBindingForControl(context.control);
            if(binding == null) return;

            //Abort if this binding does not apply to this specific control
            var bind = (InputBinding)binding;
            if(bind.id != controlInfo.GetBinding().id) return;
            rebindManager.Rebind(action, bindingIndex);
        }

        //Swaps the binding of this control if it conflicts with a rebind that just took place
        private void CheckRebindSwap(RebindData data)
        {
            //Abort this is the binding that was rebinded
            if(data.action == action && data.bindingIndex == bindingIndex) return;

            //Abort if this action does not match the newly rebinded path
            if(data.newBinding.overridePath != controlInfo.GetBinding().effectivePath) return;

            //Swap paths
            action.ApplyBindingOverride(bindingIndex, data.oldBinding.effectivePath);
        }
        #endregion
    }
}