using UnityEngine.InputSystem;

namespace JZ.INPUT
{
    public class MenuingInputSystem : MyInputSystemBehaviour<MenuingInputs>
    {
        public InputAction xNavigate { get; private set; }
        public InputAction yNavigate { get; private set; }
        public InputAction select { get; private set; }
        public InputAction back { get; private set; }
        public InputAction tab { get; private set; }


        protected override void Awake()
        {
            base.Awake();
            inputs = InputManager.menuingInputs;
            xNavigate = inputs.Menus.XNavigate;
            yNavigate = inputs.Menus.YNavigate;
            select = inputs.Menus.Select;
            back = inputs.Menus.Back;
            tab = inputs.Menus.Tab;
        }

        protected override void EnableAllActions()
        {
            inputs.Enable();
        }

        protected override void DisableAllActions()
        {
            inputs.Disable();
        }
    }
}