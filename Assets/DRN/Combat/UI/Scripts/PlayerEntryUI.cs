using DRN.COMBAT.COMBATANT;
using TMPro;
using UnityEngine;

namespace DRN.COMBAT.UI
{
    /// <summary>
    /// Parent UI for player health, BB's, and other combat display info
    /// </summary>
    public class PlayerEntryUI : MonoBehaviour
    {
        #region //Player info
        [SerializeField, Range(1,2)] int playerNumber = 1;
        private Combatant combatant = null;
        private CombatAttacker[] attackers = new CombatAttacker[0];
        #endregion

        #region //UI components
        [SerializeField] private TextMeshProUGUI nameText = null;
        [SerializeField] private TextMeshProUGUI hpText = null;
        [SerializeField] private TextMeshProUGUI tpText = null;
        [SerializeField] private BattleBarUI leftHandBB = null;
        [SerializeField] private BattleBarUI rightHandBB = null;
        #endregion

        #region //UI Colors
        [Tooltip("HP/TP color if value is 50% or more")] [SerializeField] private Color highColor = Color.white;
        [Tooltip("HP/TP color if value is between 25% and 50%")] [SerializeField] private Color midColor = Color.yellow;
        [Tooltip("HP/TP color if value is below 25%")] [SerializeField] private Color lowColor = Color.red;
        #endregion

    
        #region //Monobehaviour
        private void Awake()
        {
            var playerGO = GameObject.FindGameObjectWithTag($"Player {playerNumber}");
            combatant = playerGO.GetComponent<Combatant>();
            attackers = playerGO.GetComponentsInChildren<CombatAttacker>();
            leftHandBB.SetAttacker(attackers[0]);
            rightHandBB.SetAttacker(attackers[1]);
        }

        private void Start()
        {
            nameText.text = combatant.GetName();
            UpdateHPText();
            UpdateTPText();
        }

        private void Update()
        {
            UpdateHPText();
            UpdateTPText();
        }
        #endregion

        #region //Updating UI
        private void UpdateHPText()
        {
            hpText.color = GetTextColor(combatant.GetHPPercentage());
            hpText.text = $"<color=#FFFFFF>HP:</color> {combatant.GetHP()} / {combatant.GetMaxHP()}";
        }

        private void UpdateTPText()
        {
            tpText.color = GetTextColor(combatant.GetTPPercentage());
            tpText.text = $"<color=#FFFFFF>TP:</color> {combatant.GetTP()} / {combatant.GetMaxTP()}";
        }

        private Color GetTextColor(float percentage)
        {
            if(percentage >= 0.50f) return highColor;
            else if(percentage >= 0.25f) return midColor;
            else return lowColor;
        }
        #endregion
    }
}

//TODO
//Add weapon name fields once equipment is added