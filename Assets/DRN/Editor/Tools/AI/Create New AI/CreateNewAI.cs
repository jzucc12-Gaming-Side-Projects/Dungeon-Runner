using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Unity.EditorCoroutines.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections;
using System.Reflection;
using DRN.CHARACTER;
using DRN.COMBAT.COMBATANT;
using DRN.COMBAT.ACTION;
using JZExtensions.TRANSFORM;
using JZExtensions.STRING;
using JZ.EDITOR;
using JZ.EDITOR.Extensions;

namespace DRN.TOOLS.AI
{
    public class CreateNewAI : EditorWindow
    {
        #region //File paths
        private static string templatePath => $"Assets/{DRNGlobals.aiAssetPath}/Template";
        private static string savePath => $"Assets/{DRNGlobals.aiCharacterPath}";
        #endregion

        #region //Core Elements
        private VisualElement container = null;
        private Label notification = null;
        private Button createButton = null;
        private YesNoPopup currentPopup = null;
        #endregion

        #region //Base AI variables
        private TextField aiName = null;
        private ObjectField basePrefab = null;
        private StatElement baseStatEntry = null;
        #endregion

        #region //Logic script variables
        private Foldout logicFoldout = null;
        private ObjectField logicTemplate = null;
        private IntegerField logicCount = null;
        private LogicElementList logics = null;
        #endregion

        #region //Attacker variables
        private Foldout attackerFoldout = null;
        private ObjectField attackerPrefab = null;
        private IntegerField attackerCount = null;
        private AttackerElementList attackers = null;
        #endregion

        #region //Creation data
        private EditorCoroutine creationRoutine = null;
        private HeldData held = null;
        private bool holdingData => held != null && held.baseAI != null;
        #endregion


        #region //Monobehaviour
        private void Update() 
        {
            if(holdingData) TrySavePrefab();
        }

        private void OnDestroy() 
        {
            if(creationRoutine != null) EditorCoroutineUtility.StopCoroutine(creationRoutine);
            if(currentPopup != null) currentPopup.Close();
        }
        #endregion

        #region //Creating a window
        [MenuItem("AI Creation/Create AI")]
        public static void ShowWindow()
        {
            var window = GetWindow<CreateNewAI>();
            window.titleContent = new GUIContent("Create AI");
            window.minSize = new Vector2(350, 300);
            window.maxSize = new Vector2(700, 1000);
            EditorUtility.SetDirty(window);
        }

        public void CreateGUI()
        {
            //Core UI
            container = this.SetUpWindow($"{DRNToolGlobals.aiToolPath}/Create New AI/CreateNewAIUXML.uxml", $"{JZGlobals.editorPath}/JZBaseEditorUSS.uss");
            notification = container.Q<Label>("notification");
            createButton = container.Q<Button>("createButton");
            notification.text = " ";
            createButton.clicked += () => creationRoutine = EditorCoroutineUtility.StartCoroutine(CreateEnemy(), this);

            //Base specific
            aiName = container.Q<TextField>("aiName");
            basePrefab = container.Q<ObjectField>("basePrefab");
            baseStatEntry = new StatElement(container.Q<VisualElement>("baseStats"));
            basePrefab.objectType = typeof(Combatant);
            basePrefab.value = AssetDatabase.LoadAssetAtPath<Combatant>($"{templatePath}/AI Combatant.prefab");

            //Logic Specific
            logicFoldout = container.Q<Foldout>("logicNames");
            logicTemplate = container.Q<ObjectField>("logicTemplate");
            logicCount = container.Q<IntegerField>("logicCount");
            logicTemplate.objectType = typeof(MonoScript);
            logicTemplate.value = AssetDatabase.LoadAssetAtPath<MonoScript>($"{templatePath}/TemplateExampleLogic.cs");
            logics = new LogicElementList(logicFoldout);
            logicCount.RegisterValueChangedCallback(change => logics.ChangeCount(change.previousValue, change.newValue));

            //Attacker specific
            attackerFoldout = container.Q<Foldout>("attackerFoldout");
            attackerPrefab = container.Q<ObjectField>("attackerPrefab");
            attackerCount = container.Q<IntegerField>("attackerCount");
            attackerPrefab.objectType = typeof(AIAttacker);
            attackerPrefab.value = AssetDatabase.LoadAssetAtPath<GameObject>($"{templatePath}/AI Attacker.prefab");
            attackers = new AttackerElementList(attackerFoldout);
            attackerCount.RegisterValueChangedCallback(change => attackers.ChangeCount(change.previousValue, change.newValue));

            //List set up
            logics.SetAttackers(attackers.entries);
            attackers.SetLogics(logics.entries);
            logics.ChangeCount(0, 1);
            attackers.ChangeCount(0, 1);
        }
        #endregion

        #region //Enemy creation
        private IEnumerator CreateEnemy()
        {
            //Check for process abortion
            if(CheckAutoAbort()) yield break;
            foreach(var popup in GetAbortPopups())
            {
                container.visible = false;
                currentPopup = popup;
                currentPopup.ShowPopup();
                bool abort = false;
                popup.Closed += ((bool continueOperation) => abort = !continueOperation);
                while(currentPopup != null) yield return null;
                container.visible = true;
                if(abort) yield break;
            }

            //Base AI generation
            AssetDatabase.CreateFolder(savePath, aiName.value);
            Combatant ai = (Combatant)basePrefab.value;
            Combatant aiBase = Instantiate(ai);
            aiBase.name = aiName.value;
            aiBase.GetComponent<AIDataDistributor>().SetUp(aiName.value);
            held = new HeldData(aiBase);

            //Logic script creation
            MonoScript monoScript = (MonoScript)logicTemplate.value;
            foreach(var logic in logics.entries)
            {
                string scriptName = $"{aiName.value.Replace(" ","")}{logic.logicName.value.Replace(" ","")}Logic";
                string text = monoScript.text.Replace(monoScript.name, scriptName);

                TextAsset newAsset = new TextAsset(monoScript.text.Replace(monoScript.name, scriptName));
                File.WriteAllText(DRNGlobals.GetAIDataPath(aiName.value, $"{scriptName}.cs"), monoScript.text.Replace(monoScript.name, scriptName));
                AssetDatabase.ImportAsset($"{savePath}/{aiName.value}/{scriptName}.cs");
                held.AddScriptName(scriptName);
            }

            //Attacker creation
            var charWeapons = new CharacterWeapon[attackers.entries.Count];            
            for(int ii = 0; ii < attackers.entries.Count; ii++)
            {
                var weapon = new CharacterWeapon();
                var attacker = attackers.entries[ii];
                weapon.weaponName = attacker.GetAttackerName();
                weapon.stats = attacker.GetStatList();
                charWeapons[ii] = weapon;

                AIAttacker atkPrefab = Instantiate((AIAttacker)attackerPrefab.value, aiBase.transform.FindDeepChild("Attackers"));
                atkPrefab.name = attacker.GetAttackerName();
                atkPrefab.SetUp(ii, aiBase.GetComponent<CombatBody>());
                if(attacker.ElementIsSelected()) held.AddAtkr(attacker.GetLogicIndex(), atkPrefab);
            }            

            //Create character data
            CharacterData characterData = new CharacterData(aiName.value);
            characterData.baseStats = baseStatEntry.GetStatList();
            characterData.armorStats = baseStatEntry.GetStatList();
            characterData.weapons = charWeapons;
            characterData.currentHP = characterData.GetTotalStat(STATS.BodyStats.maxHP);
            characterData.currentTP = characterData.GetTotalStat(STATS.BodyStats.maxTP);
            CharacterData.CreateCharacterData(DRNGlobals.GetAIDataPath(aiName.value, $"{aiName.value}.txt"), characterData);
            AssetDatabase.ImportAsset($"{savePath}/{aiName.value}/{aiName.value}.txt");

            notification.text = "Waiting on prefab creation.";
        }

        //Checks to see if the creation process should automatically abort
        private bool CheckAutoAbort()
        {
            string abortMessage = "";

            //Checking for invalid names
            if(!aiName.value.IsFullAlphanumeric())
                abortMessage = "Invalid AI name. Must be alphanumeric and can't be empty. Aborting";

            else if(AssetDatabase.IsValidFolder($"{savePath}/{aiName.value}"))
                abortMessage = "Enemy already exists. Aborting";

            else if(logics.entries.Any(logic => !logic.GetLogicName().IsFullAlphanumeric()))
                abortMessage = "Invalid logic name. Must be alphanumeric and can't be empty. Aborting";

            else if(attackers.entries.Any(attacker => string.IsNullOrEmpty(attacker.GetAttackerName()) || attacker.GetAttackerName().IsOnlySpaces()))
                abortMessage = "Invalid attacker name. Can't be empty. Aborting";

            //Checking for duplicate names
            if(string.IsNullOrEmpty(abortMessage))
            {
                foreach(var logic in logics.entries)
                {
                    if(logics.entries.Count(x => x.logicName.value.ToUpper() == logic.logicName.value.ToUpper()) > 1)
                    {
                        abortMessage = "Can't have duplicate logic names. Aborting";
                        break;
                    }
                }
            }
            
            if(string.IsNullOrEmpty(abortMessage))
            {
                foreach(var attacker in attackers.entries)
                {
                    if(attackers.entries.FindAll(x => x.GetAttackerName().ToUpper() == attacker.GetAttackerName().ToUpper()).Count > 1)
                    {
                        abortMessage = "Can't have duplicate attacker names. Aborting";
                        break;
                    }
                }
            }

            notification.text = abortMessage;
            return !string.IsNullOrEmpty(abortMessage);
        }

        //Checks to see if pop-up windows prompting abortion should appear
        private List<YesNoPopup> GetAbortPopups()
        {
            List<YesNoPopup> popups = new List<YesNoPopup>();

            //Check for attackers with no logic script
            foreach(var attacker in attackers.entries)
            {
                if(!attacker.ElementIsSelected()) 
                {
                    var popup = YesNoPopup.CreateWindow(this.position.center, new Vector2(350, 200), true, false);
                    popup.SetUp("Floating Attackers", "There are attackers not hooked up to logic scripts. Continue?", true);
                    popups.Add(popup);
                    break;
                }
            }

            //Check for unused logic scripts
            foreach(var logic in logics.entries)
            {
                for(int ii = 0; ii < attackers.entries.Count; ii++)
                {
                    if(attackers.entries[ii].GetActiveElement() == logic) break;
                    else if(ii == attackers.entries.Count - 1)
                    {
                        var popup = YesNoPopup.CreateWindow(this.position.center, new Vector2(350, 200), true, false);
                        popup.SetUp("Floating Logic Script", "There are logic scripts without attackers. Continue?", true);
                        popups.Add(popup);
                        break;
                    }
                }
            }

            return popups;
        }
        
        //Saves the enemy prefab once the created logic scripts load into the assets folder
        public void TrySavePrefab()
        {
            Assembly assembly = typeof(AIAttacker).Assembly;
            if(assembly.GetType($"DRN.COMBAT.ACTION.{held.scriptNames[0]}") == null) return;

            var baseAI = held.baseAI;
            for(int ii = 0; ii < held.scriptNames.Count; ii++)
            {
                Type type = assembly.GetType($"DRN.COMBAT.ACTION.{held.scriptNames[ii]}");
                baseAI.gameObject.AddComponent(type);
                var ai = (AIAttackLogic)baseAI.GetComponent(type);
                List<AIAttacker> atkToAdd = new List<AIAttacker>(held.GetAttackers(ii));
                ai.SetUp(baseAI, atkToAdd);
            }

            PrefabUtility.SaveAsPrefabAsset(baseAI.gameObject, DRNGlobals.GetAIDataPath(baseAI.name, $"{baseAI.name}.prefab"));
            notification.text = $"Finished creating new enemy '{baseAI.name}'.";
            DestroyImmediate(baseAI.gameObject);
            held = null;
        }
        #endregion
    }
}