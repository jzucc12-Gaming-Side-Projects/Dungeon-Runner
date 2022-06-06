using System.Collections.Generic;
using DRN.COMBAT.COMBATANT;
using JZ.POOL;
using TMPro;
using UnityEngine;

namespace DRN.COMBAT.UI
{
    public class EndCombatUI : MonoBehaviour
    {
        private CombatManager combatManager = null;

        #region //UI elements
        [SerializeField] private GameObject uiBox = null;
        [SerializeField] private TextMeshProUGUI header = null;
        [SerializeField] private ComponentPool<TextMeshProUGUI> defeatedList;
        #endregion


        private void Awake()
        {
            combatManager = FindObjectOfType<CombatManager>();
            defeatedList.CreatePool();
            uiBox.SetActive(false);
        }

        private void OnEnable()
        {
            combatManager.OnEndCombat += InitiateUI;
        }

        private void OnDisable()
        {
            combatManager.OnEndCombat -= InitiateUI;
        }

        private void InitiateUI(string text, Dictionary<string, int> defeatedEnemies)
        {
            header.text = text;
            defeatedList.DeactivateAll();
            uiBox.SetActive(true);

            if(defeatedEnemies.Count == 0) defeatedList.GetObject().text = "None";
            foreach(var dictEntry in defeatedEnemies)
            {
                var entry = defeatedList.GetObject();
                entry.text = $"{dictEntry.Value}x {dictEntry.Key}";
            }
        }
    }
}
