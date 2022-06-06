using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Users;
#if !UNITY_WEBGL
using UnityEngine.InputSystem.Switch;
#endif

namespace JZ.INPUT
{
    /// <summary>
    /// <para>Keeps track of currently used player input device</para>
    /// </summary>
    public class DeviceChecker : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput = null;
        public static string currentScheme;
        public static bool isUsingGamepad => currentScheme == "Gamepad";
        private static GamepadType lastType = GamepadType.xbox;


        #region //Monobehaviour
        private void OnEnable()
        {
            InputUser.onChange += OnUserChange;
            InputSystem.onDeviceChange += NewDevice;
        }

        private void OnDisable()
        {
            InputUser.onChange -= OnUserChange;
            InputSystem.onDeviceChange -= NewDevice;
        }

        private void Start()
        {
            currentScheme = playerInput.currentControlScheme;

            if(Gamepad.all.Count > 0)
                SetGamepad(Gamepad.all[0]);
        }
        #endregion

        public static GamepadType GetCurrentGamepadType()
        {
            if (!isUsingGamepad)
                return lastType;

            SetGamepad(Gamepad.current);

            return lastType;
        }

        private static void SetGamepad(Gamepad gamepad)
        {
            if (gamepad is DualShockGamepad)
                lastType = GamepadType.sony;
            #if !UNITY_WEBGL
            else if (gamepad is SwitchProControllerHID)
                lastType = GamepadType.nSwitch;
            #endif
            else
                lastType = GamepadType.xbox;
        }

        private void OnUserChange(InputUser user, InputUserChange change, InputDevice dvc)
        {
            if (change == InputUserChange.ControlSchemeChanged)
            {
                currentScheme = playerInput.currentControlScheme;
            }
        }

        private void NewDevice(InputDevice device, InputDeviceChange change)
        {
            switch(change)
            {
                case InputDeviceChange.Added:
                    if(device is Gamepad)
                        SetGamepad((Gamepad)device);
                        break;
                default:
                    return;
            }
        }
    }
}

