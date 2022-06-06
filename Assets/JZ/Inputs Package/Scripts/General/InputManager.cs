using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JZ.INPUT
{
    /// <summary>
    /// <para>Holds references to all available player inputs</para>
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        #region //Input Action Assets
        public static GeneralInputs generalInputs = null;
        public static MenuingInputs menuingInputs = null;
        private static Dictionary<string, InputActionAsset> actionAssets = new Dictionary<string, InputActionAsset>();
        #endregion


        #region //Monobehaviour
        public void Awake()
        {
            generalInputs = new GeneralInputs();
            actionAssets.Add(generalInputs.asset.name, generalInputs.asset);

            menuingInputs = new MenuingInputs();
            actionAssets.Add(menuingInputs.asset.name, menuingInputs.asset);
        }

        private void OnEnable()
        {
            menuingInputs.Menus.Enable();
            generalInputs.Map.Enable();
        }

        private void OnDisable()
        {
            menuingInputs.Menus.Disable();
            generalInputs.Map.Disable();
        }
        #endregion

        #region //Getters
        public static InputActionAsset GetAsset(string assetName)
        {
            return actionAssets[assetName];
        }

        public static IEnumerable<InputActionAsset> GetAssets()
        {
            foreach(var entry in actionAssets.Values)
                yield return entry;
        }
        #endregion
    }
}