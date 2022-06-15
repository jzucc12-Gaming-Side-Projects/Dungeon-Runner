using System.Collections.Generic;
using DRN.COMBAT.COMBATANT;
using DRN.COMBAT.TARGETING;
using UnityEngine;

namespace DRN.COMBAT.ACTION
{
    public abstract class AIAttackLogic : MonoBehaviour
    {
        #region //Variables
        [SerializeField] protected Combatant combatant = null;
        [SerializeField] protected AIAttacker attacker = null;
        private Dictionary<string, AttackEntry> actions = new Dictionary<string, AttackEntry>();
        private CombatActionScheduler scheduler = null;
        #endregion


        #region //Monobehaviour
        protected virtual void Awake()
        {
            scheduler = FindObjectOfType<CombatActionScheduler>();
            AddActions();
        }

        protected virtual void OnEnable()
        {
            attacker.OnAIReady += QueueAction;
        }

        protected virtual void OnDisable()
        {
            attacker.OnAIReady -= QueueAction;
        }

        protected virtual void Start() 
        { 
            if(attacker.GetBBPercentage() == 1) QueueAction(0);
        }
        #endregion

        public void QueueAction(int turnNumber)
        {
            var entry = ChooseEntry(turnNumber);
            if(string.IsNullOrEmpty(entry.attackName)) return;
            TargetData data = new TargetData(combatant, attacker);
            entry.targeting.GetTargets(data);
            var text = entry.displayAttackName ? entry.attackName : "";
            var item = new SchedulerItem(data, entry.action, entry.highPriority, text);
            scheduler.AddAction(item);
        }

        protected abstract void AddActions();

        protected void AddAction(AttackEntry entry)
        {
            actions.Add(entry.attackName, entry);
        }

        protected AttackEntry GetEntry(string attackName)
        {
            return actions[attackName];
        }

        protected abstract AttackEntry ChooseEntry(int turnNumber);
    }

    public struct AttackEntry
    {
        public string attackName { get; private set; }
        public IAction action { get; private set; }
        public AITargeting targeting { get; private set; }
        public bool highPriority { get; private set; }
        public bool displayAttackName { get; private set; }

        public AttackEntry(string attackName, IAction action, AITargeting targeting, bool highPriority, bool displayAttackName)
        {
            this.attackName = attackName;
            this.action = action;
            this.targeting = targeting;
            this.highPriority = highPriority;
            this.displayAttackName = displayAttackName;
        }
    }
}

//TODO
//Clean up how attackers connect to their logic. Right now the weapon name is given by character data and then the attacker has to be dragged to the proper logic in the inspector
//Possibly make it a bit more automatic though there could be benefits to leaving it this way (different stats/names for the same logic on an enemy)

//Possibly find a way to move entry creation to a non-code method, like JSON.

//Possibly use string variables for attack names so that I'm not just throwing strings around that can easily have to be changed in a million spots or are prone to typos