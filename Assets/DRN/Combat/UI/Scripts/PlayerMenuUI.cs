using DRN.COMBAT.COMBATANT;
using DRN.COMBAT.TARGETING;
using JZ.MENU.UI;
using TMPro;
using UnityEngine;

namespace DRN.COMBAT.UI
{
    /// <summary>
    /// UI for top level the player combat menu
    /// </summary>
    public class PlayerMenuUI : MonoBehaviour
    {
        #region //UI Components
        [SerializeField] private GameObject uiContainer = null;
        [SerializeField] private TextMeshProUGUI nameText = null;
        [SerializeField] private ListMenuUI listUI = null;
        #endregion

        #region //Player info
        private MainTargeter targeter = null;
        private ActivePlayerManager playerManager = null;
        private bool menuIsActive => playerManager.activeAttacker != null;
        #endregion


        #region //Monobehaviour
        private void Awake()
        {
            targeter = FindObjectOfType<MainTargeter>();
            playerManager = FindObjectOfType<ActivePlayerManager>();
        }

        private void Start()
        {
            uiContainer.SetActive(menuIsActive);
        }

        private void OnEnable()
        {
            playerManager.ActivePlayerChanged += ChangeActivePlayer;
            targeter.TargetingStarted += listUI.LockMenu;
            targeter.TargetingEnded += listUI.UnlockMenu;
        }

        private void OnDisable()
        {
            playerManager.ActivePlayerChanged -= ChangeActivePlayer;
            targeter.TargetingStarted -= listUI.LockMenu;
            targeter.TargetingEnded -= listUI.UnlockMenu;
        }
        #endregion

        #region //Updating UI
        private void ChangeActivePlayer()
        {
            uiContainer.SetActive(menuIsActive);

            if (menuIsActive)
                nameText.text = playerManager.activeCombatant.GetName();
        }
        #endregion
    }
}

//TODO
//Replace player name with weapon name (or get rid of weapon name altogether)
//Add arrow to point to active BB