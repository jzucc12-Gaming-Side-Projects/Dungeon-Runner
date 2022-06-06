using UnityEngine;

namespace JZ.INPUT.REBIND
{
    /// <summary>
    /// <para>Activates the Rebind Manager's public functions</para>
    /// <para>This is used because the Rebind Manager is in a different scene than the settings menu</para>
    /// </summary>
    public class RebindActivator : MonoBehaviour
    {
        #region //Input modification
        private RebindManager rebindManager = null;
        #endregion


        private void Awake()
        {
            rebindManager = FindObjectOfType<RebindManager>();
        }

        public void ResetBindings()
        {
            rebindManager.ResetAllBindings();
        }

        public void StartListening()
        {
            rebindManager.StartListening();
        }
    }
}