using System.Collections;
using System.Collections.Generic;
using DRN.COMBAT.COMBATANT;
using UnityEngine;


namespace DRN.COMBAT.UI
{
    public class PlayerCombatantUI : CombatantUI
    {
        #region //Active player variables
        [Header("Active Player")]
        [SerializeField] GameObject activePlayerArrowPrefab = null;
        [SerializeField] private Vector2 activePlayerArrowOffset = Vector2.zero;
        private List<GameObject> activePlayerArrows = new List<GameObject>();
        private ActivePlayerManager playerManager = null;
        #endregion


        #region //Monobehaviour
        protected override void Awake()
        {
            base.Awake();
            playerManager = FindObjectOfType<ActivePlayerManager>();
            foreach (var attacker in myCombatant.GetAttackers())
            {
                var arrow = Instantiate(activePlayerArrowPrefab, transform);
                arrow.transform.position = (Vector2)attacker.transform.position + activePlayerArrowOffset;
                activePlayerArrows.Add(arrow);
                arrow.SetActive(false);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            playerManager.ActivePlayerChanged += SetActivePlayerArrows;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            playerManager.ActivePlayerChanged -= SetActivePlayerArrows;
        }

        protected override void Start()
        {
            base.Start();
            targeter.ChangeTarget += SetTargeting;
            targeter.TargetingEnded += ResetTargeting;
        }
        #endregion

        #region //Active Player
        private void SetActivePlayerArrows()
        {
            if(playerManager.activeCombatant != myCombatant)
            {
                foreach(var arrow in activePlayerArrows)
                    arrow.SetActive(false);
            }
            else
            {
                for(int ii = 0; ii < activePlayerArrows.Count; ii++)
                    activePlayerArrows[ii].SetActive(ii == playerManager.GetActiveAttackerNumber());
            }
        }
        #endregion
    }
}
