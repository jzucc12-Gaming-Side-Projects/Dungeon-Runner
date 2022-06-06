
using UnityEngine;
using UnityEditor;

namespace DRN.COMBAT.TARGETING
{
    [System.Serializable]
    /// <summary>
    /// Contains restraints for a given targeting instance
    /// </summary>
    public class TargetingConfig
    {
        #region //Overall
        [Tooltip("If true, the targeter selects with the targets instantly")] public bool autoSelect = false;
        [Tooltip("If true, the attacker can only target themselves")] public bool targetSelfOnly = false;
        [Tooltip("If true, the sub-targeting menu will open if the chosen target has multiple attackers")] public bool needSpecificAttacker = false;
        #endregion

        #region //Targeting navigation
        [Tooltip("If true, targeting starts on the allied side of the field")] public bool startOnAllies = false;
        [Tooltip("If true, targeter can swap sides of the field")] public bool canChangeSides = false;
        [SerializeField] [Tooltip("If true, targeting starts on dead targets")] private bool startOnDead = false;
        [SerializeField] [Tooltip("Sets targeting quanitity types")] private TargetingType type = TargetingType.Single;
        #endregion


        public bool IsAOE() => !targetSelfOnly && type == TargetingType.AOE;
        public bool CanToggleAOE() => type == TargetingType.Switchable;
        public bool StartOnDead() => startOnAllies && startOnDead;
    }

    /// <summary>
    /// How many can be targeted
    /// </summary>
    public enum TargetingType
    {
        Single = 0,
        AOE = 1,
        Switchable = 2
    }


    [CustomPropertyDrawer(typeof(TargetingConfig))]
    public class TargetingConfigEditor : PropertyDrawer
    {
        static int rectNo = 0;
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if(property.isExpanded) 
                return EditorGUIUtility.singleLineHeight * (rectNo + 1);
            else 
                return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            rectNo = 0;
            EditorGUI.BeginProperty(position, label, property);
            property.isExpanded = EditorGUI.Foldout(NextRect(position), property.isExpanded, label);
            if(!property.isExpanded) return;

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            EditorGUI.LabelField(NextRect(position), "Overall", EditorStyles.boldLabel);
            EditorGUI.PropertyField(NextRect(position), property.FindPropertyRelative("autoSelect"));
            var tso = property.FindPropertyRelative("targetSelfOnly");
            EditorGUI.PropertyField(NextRect(position), tso);
            

            if(!tso.boolValue)
            {
                ShowTargetNavigation(position, property, label);
            }
            else
            {
                ShowSubTargeting(position, property, label);
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        private Rect NextRect(Rect position)
        {
            var rect = new Rect(position.x, position.y + rectNo * EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight);
            rectNo++;
            return rect;
        }

        private void ShowTargetNavigation(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.LabelField(NextRect(position), " ");
            EditorGUI.LabelField(NextRect(position), "Targeting Navigation", EditorStyles.boldLabel);
            EditorGUI.PropertyField(NextRect(position), property.FindPropertyRelative("canChangeSides"));

            var soa = property.FindPropertyRelative("startOnAllies");
            EditorGUI.PropertyField(NextRect(position), soa);
            if(soa.boolValue) EditorGUI.PropertyField(NextRect(position), property.FindPropertyRelative("startOnDead"));

            var type = property.FindPropertyRelative("type");
            type.intValue = EditorGUI.Popup(NextRect(position), "Targeting Type", type.enumValueIndex, type.enumNames);

            if(type.intValue == 0) ShowSubTargeting(position, property, label);
        }

        private void ShowSubTargeting(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.LabelField(NextRect(position), " ");
            EditorGUI.LabelField(NextRect(position), "Sub Targeting", EditorStyles.boldLabel);
            EditorGUI.PropertyField(NextRect(position), property.FindPropertyRelative("needSpecificAttacker"));
        }
    }
}