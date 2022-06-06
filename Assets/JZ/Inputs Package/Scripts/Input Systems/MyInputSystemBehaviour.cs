using UnityEngine.InputSystem;
using UnityEngine;

namespace JZ.INPUT
{
    /// <summary>
    /// <para>Parent class for nput systems placed onto a Monobehaviour</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MyInputSystemBehaviour<T> : MonoBehaviour where T : IInputActionCollection
    {
        protected T inputs = default(T);

        #region //Monobehaviour
        protected virtual void Awake() { }
        protected virtual void OnEnable()
        {
            EnableAllActions();
        }
        protected virtual void OnDisable()
        {
            DisableAllActions();
        }
        protected virtual void Start() { }
        protected virtual void Update() { }
        protected virtual void FixedUpdate() { }
        #endregion

        #region//Startup shutdown
        protected abstract void EnableAllActions();
        protected abstract void DisableAllActions();
        #endregion
    }
}