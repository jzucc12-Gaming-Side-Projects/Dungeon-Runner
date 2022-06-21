

using System;
using System.Collections.Generic;
using DRN.STATS;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DRN.TOOLS
{
    /// <summary>
    /// Displays stats input fields for Tool Editors
    /// </summary>
    public class StatElement
    {
        #region //UI Elements
        private Dictionary<BodyStats, IntegerField> bodyStatsDict = new Dictionary<BodyStats, IntegerField>();
        private Dictionary<WeaponStats, IntegerField> weaponStatsDict = new Dictionary<WeaponStats, IntegerField>();
        #endregion

        #region //Constructor
        public StatElement(VisualElement container, int startValue = 0)
        {
            var bodyStatContainer = container.Q<VisualElement>("bodyStatContainer");
            foreach(BodyStats stat in Enum.GetValues(typeof(BodyStats)))
            {
                var field = SetUpField(StatDesc.GetStatName(stat), StatDesc.GetStatDesc(stat), startValue);
                bodyStatContainer.Add(field);
                bodyStatsDict.Add(stat, field);
            }

            var weaponStatContainer = container.Q<VisualElement>("weaponStatContainer");
            foreach(WeaponStats stat in Enum.GetValues(typeof(WeaponStats)))
            {
                var field = SetUpField(StatDesc.GetStatName(stat), StatDesc.GetStatDesc(stat), startValue);
                weaponStatContainer.Add(field);
                weaponStatsDict.Add(stat, field);
            }
        }
        #endregion

        #region //Initialization
        private IntegerField SetUpField(string labelText, string tooltipText, int startValue)
        {
            IntegerField field = new IntegerField();
            field.AddToClassList("fieldText");
            field.value = startValue;
            field.label = labelText;
            field.tooltip = tooltipText;
            return field;
        }
        #endregion

        #region //Getters
        public StatList GetStatList()
        {
            StatList list = new StatList();
            foreach(BodyStats stat in Enum.GetValues(typeof(BodyStats)))
                list.bodyStats[stat] = bodyStatsDict[stat].value; 

            foreach(WeaponStats stat in Enum.GetValues(typeof(WeaponStats)))
                list.weaponStats[stat] = weaponStatsDict[stat].value;

            return list;
        }
        #endregion
    }
}