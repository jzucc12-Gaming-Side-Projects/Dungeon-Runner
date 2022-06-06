using DRN.COMBAT.TARGETING;
using UnityEngine;

namespace DRN.COMBAT.ACTION
{
    /// <summary>
    /// Class that contains basic logic for combat actions. Children classes required to specific actions.
    /// </summary>
    public abstract class ActionFactory : ScriptableObject
    {
        
        public bool highPriority = false;
        public TargetingConfig targetingConfig = new TargetingConfig(); 
        private IAction action;

        protected virtual void Awake() { }
        protected virtual void OnEnable() { }
        protected virtual void OnStart() { }

        public abstract IAction GetAction();
    }
}