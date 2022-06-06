using UnityEngine.InputSystem;

namespace JZ.INPUT
{
    /// <summary>
    /// <para>Parent class for input systems</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MyInputSystem<T> where T : IInputActionCollection
    {
        protected T inputs = default(T);
        private bool active = false;

        #region//Constructor
        public MyInputSystem(T _inputs) { }
        #endregion

        #region//Startup shutdown
        public void Activate()
        {
            if (active) return;
            EnableActions();
            active = true;
        }

        public void Deactivate()
        {
            if (!active) return;
            DisableActions();
            active = false;
        }

        protected abstract void EnableActions();
        protected abstract void DisableActions();
        #endregion
    }
}