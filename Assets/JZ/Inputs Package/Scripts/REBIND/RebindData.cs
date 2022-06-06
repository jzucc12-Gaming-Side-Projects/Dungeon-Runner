using UnityEngine.InputSystem;

namespace JZ.INPUT.REBIND
{
    /// <summary>
    /// <para>Info for a given rebinding event</para>
    /// </summary>
    public struct RebindData
    {
        public InputAction action;
        public InputBinding oldBinding;
        public InputBinding newBinding;
        public int bindingIndex;
    }
}