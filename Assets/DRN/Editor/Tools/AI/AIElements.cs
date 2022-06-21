using System.Collections.Generic;
using DRN.STATS;
using JZ.UI;
using JZ.EDITOR.Extensions;
using UnityEngine.UIElements;

namespace DRN.TOOLS.AI
{
    /// <summary>
    /// Attacker Component display for AI Editors
    /// </summary>
    public class AttackerElement : VisualElement
    {
        #region //UI Elements
        private Foldout foldout = null;
        public TextField attackerName { get; private set; }
        private DropdownField logicScript = null;
        private StatElement stats = null;
        #endregion

        #region //Logic Elements
        private List<LogicElement> logics = new List<LogicElement>();
        #endregion


        #region //Constructor
        public AttackerElement(string startName, List<LogicElement> logics)
        {
            this.SetUpVisualElement($"{DRNToolGlobals.aiToolPath}/attackerElement.uxml");
            foldout = this.Q<Foldout>("Foldout");
            attackerName = this.Q<TextField>("attackerName");
            logicScript = this.Q<DropdownField>("logicScript");
            stats = new StatElement(this.Q<VisualElement>("statEntry"));

            foldout.text = startName;
            attackerName.value = startName;
            attackerName.RegisterValueChangedCallback(change => foldout.text = change.newValue);
            logicScript.choices.Clear();
            foreach(var logic in logics)
                AddScriptElement(logic);
        }
        #endregion

        #region //Getters
        public string GetAttackerName() => attackerName.value;
        public StatList GetStatList() => stats.GetStatList();
        public int GetLogicIndex() => logicScript.index;
        public string GetLogicValue() => logicScript.value;
        public LogicElement GetActiveElement() => (logicScript.index != -1 ? logics[logicScript.index] : null);
        public bool ElementIsSelected() => GetActiveElement() != null;
        #endregion

        #region //Logic Script Modification
        public void AddScriptElement(LogicElement logic)
        {
            logics.Add(logic);
            logic.logicName.RegisterValueChangedCallback(change => ChangeScriptName(change.previousValue, change.newValue));
            RegenerateList();
        }
        
        public void RemoveScriptElement(LogicElement logic)
        {
            logics.Remove(logic);
            if(logicScript.value == logic.logicName.value) logicScript.value = "";
            RegenerateList();
        }

        public void ChangeScriptName(string oldName, string newName)
        {
            if(logicScript.value == oldName) logicScript.value = newName;
            RegenerateList();
        }

        private void RegenerateList()
        {
            logicScript.choices.Clear();
            foreach(var logic in logics)
                logicScript.choices.Add(logic.logicName.value);
        }
        #endregion
    }

    public class AttackerElementList : VisualElementList<AttackerElement>
    {
        private List<LogicElement> logics = new List<LogicElement>();

        public AttackerElementList(VisualElement container) : base(container) { }

        protected override AttackerElement GenerateEntry()
        {
            return new AttackerElement($"Attacker {entries.Count + 1}", logics);
        }

        public void SetLogics(List<LogicElement> logics) => this.logics = logics;
    }


    /// <summary>
    /// Logic script display AI Editors
    /// </summary>
    public class LogicElement : VisualElement
    {
        public TextField logicName { get; private set; }


        public LogicElement(string startName)
        {
            this.SetUpVisualElement($"{DRNToolGlobals.aiToolPath}/logicElement.uxml");
            logicName = this.Q<TextField>("logicName");
            logicName.value = startName;
        }

        public string GetLogicName() => logicName.value;


        // TODO 
        // ONCE AI TUTORIALS FINISHED, DETERMINE IF I WANT TO INVERT HOW LOGICS AND ATTACKERS INTERACT! IF NOT, CHANGE TO USING THIS MASK FIELD
        // Mask field was not used initially because I didn't know how to use it at first. I made the currently implemented system before I knew I could do masks
        // By the time I learned about masks, someone on the GMTK Discord pointed out to me I should really consider looking into inverting the AIAttacker/AILogic relationship.

        //FIELDS
        // private List<AttackerElement> attackers = new List<AttackerElement>();
        // public MaskField attackerMask = null;

        //IN CONSTRUCTOR
        // attackerMask = this.Q<MaskField>("attackers");
        // foreach(var attacker in attackers)
        //     AddMaskEntry(attacker);

        //METHODS

        // public void AddMaskEntry(AttackerElement attacker)
        // {
        //     attackers.Add(attacker);
        //     attacker.attackerName.RegisterValueChangedCallback(change => RegenerateList());
        //     RegenerateList();
        // }

        // public void RemoveMaskEntry(AttackerElement attacker)
        // {
        //     attackers.Remove(attacker);
        //     RegenerateList();
        // }

        // private void RegenerateList()
        // {
        //     var list = new List<string>();
        //     foreach(var attacker in attackers)
        //         list.Add(attacker.GetAttackerName());

        //     attackerMask.choices = new List<string>(list);
        // }

        //AFTER
        //ADD THE ADD/REMOVE MASK ENTRIES TO CREATE NEW AI TOOL
        //REMOVE THE SIMILAR LOGIC FROM ATTACKER ELEMENT
        //CHANGE CREATION TO RELY ON MASK VALUES INSTEAD OF THE CURRENT ASSORTMENT OF LISTS IN HELD DATA CLASS.
    }

    public class LogicElementList : VisualElementList<LogicElement>
    {
        private List<AttackerElement> attackers = new List<AttackerElement>();


        public LogicElementList(VisualElement container) : base(container) { }

        protected override LogicElement GenerateEntry()
        {
            return new LogicElement($"Logic {entries.Count + 1}");
        }

        protected override void AddEntries(LogicElement entry)
        {
            base.AddEntries(entry);
            foreach(var attacker in attackers) attacker.AddScriptElement(entry);
        }

        protected override void RemoveEntries(LogicElement entry)
        {
            base.RemoveEntries(entry);
            foreach(var attacker in attackers) attacker.RemoveScriptElement(entry);
        }

        public void SetAttackers(List<AttackerElement> attackers) => this.attackers = attackers;
    }
}