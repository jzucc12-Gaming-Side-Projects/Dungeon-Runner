using UnityEngine.InputSystem;

namespace JZ.INPUT
{
    /// <summary>
    /// <para>Information relating to a specific player control</para>
    /// </summary>
    public interface IControlInfo
    {
        InputActionAsset GetAsset();
        InputAction GetAction();
        InputBinding GetBinding();
        int GetBindingIndex();
    }
}
