using System;
using DRN.COMBAT.COMBATANT;
using DRN.COMBAT.TARGETING;
using JZ.UI;
using TMPro;
using UnityEngine;

namespace DRN.COMBAT.UI
{
    public class SubTargeterUI : MonoBehaviour
    {
        #region //Cached Components
        [Header("Cached Components")]
        [SerializeField] private Transform container = null;
        [SerializeField] private GameObject buttonPrefab = null;
        [SerializeField] private TextMeshProUGUI targetName = null;
        private SubTargeter subTargeter = null;
        private LayoutGroupRebuilder rebuilder = null;
        #endregion

        #region //Arrow variables
        [Header("Arrow variables")]
        [SerializeField] private Transform menuArrow = null;
        #endregion


        #region //Monobehaviour
        private void Awake()
        {
            subTargeter = FindObjectOfType<SubTargeter>();
            rebuilder = GetComponent<LayoutGroupRebuilder>();
        }

        private void Start()
        {
            subTargeter.StartSubTargeter += SetUpUI;
            subTargeter.TargetChanged += UpdateArrowPositions;
            subTargeter.StopSubTargeter += CloseUI;
            CloseUI();
        }
        #endregion

        #region //Targeting
        private void SetUpUI(Combatant target)
        {
            gameObject.SetActive(true);
            targetName.text = target.GetName();

            foreach(Transform entry in container)
                Destroy(entry.gameObject);
            container.DetachChildren();

            foreach(var attacker in target.GetAttackers())
            {
                var entry = Instantiate(buttonPrefab, container);
                entry.GetComponentInChildren<TextMeshProUGUI>().text = attacker.GetWeaponName();
            }
            
            rebuilder.RebuildLayoutGroups();
        }

        private void UpdateArrowPositions(CombatAttacker attacker, int index)
        {
            Vector2 menuPosition = container.GetChild(index).position;
            menuPosition.x = menuArrow.position.x;
            menuArrow.position = menuPosition;
        }

        private void CloseUI()
        {
            gameObject.SetActive(false);
        }
        #endregion
    }
}