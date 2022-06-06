using System;
using System.Collections.Generic;
using System.Linq;
using JZ.INPUT;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DRN.COMBAT.COMBATANT
{
    /// <summary>
    /// Keeps track of all players who can act in combat and allows the player to switch between them
    /// </summary>
    public class ActivePlayerManager : MonoBehaviour
    {
        #region //Input
        private MenuingInputSystem inputSystem = null;
        #endregion

        #region //Players
        private Dictionary<PlayerAttacker, Combatant> players = new Dictionary<PlayerAttacker, Combatant>();
        private List<PlayerAttacker> readiedPlayers = new List<PlayerAttacker>();
        #endregion

        #region //Manager state
        private int activePlayerIndex = 0;
        public PlayerAttacker activeAttacker => readiedPlayers.Count > 0 ? readiedPlayers[activePlayerIndex] : null;
        public Combatant activeCombatant => activeAttacker != null ? players[activeAttacker] : null;
        public event Action ActivePlayerChanged;
        #endregion


        #region //Monobehaviour
        private void Awake()
        {
            inputSystem = FindObjectOfType<MenuingInputSystem>();
        }

        private void OnEnable()
        {
            EnableInputs();
            foreach(var player in players.Keys)
                player.OnPlayerReady += OnPlayerReady;
        }

        private void OnDisable()
        {
            DisableInputs();
            foreach(var player in players.Keys)
                player.OnPlayerReady -= OnPlayerReady;
        }

        private void Start()
        {
            foreach(var player in CombatTarget.players)
            {
                foreach(var attacker in player.GetAttackers())
                {
                    var atkr = (PlayerAttacker)attacker;
                    players.Add(atkr, player);
                    atkr.OnPlayerReady += OnPlayerReady;
                }
            }
        }
        #endregion

        #region //Input management
        public void EnableInputs()
        {
            inputSystem.tab.started += ShiftActivePlayer;
        }

        public void DisableInputs()
        {
            inputSystem.tab.started -= ShiftActivePlayer;
        }
        #endregion

        #region //Active player change
        private void ShiftActivePlayer(InputAction.CallbackContext context)
        {
            if(activeAttacker == null) return;
            int shiftAmount = Mathf.RoundToInt(context.ReadValue<float>());
            int newIndex = JZMathUtils.Wrap(activePlayerIndex + shiftAmount, 0, readiedPlayers.Count - 1);
            SetActivePlayer(newIndex);
        }

        private void OnPlayerReady(PlayerAttacker player)
        {
            var currentPlayer = activeAttacker;
            
            readiedPlayers.Add(player);
            readiedPlayers.Sort((p1, p2) =>
            {
                if(p1.GetDisplayPriority() > p2.GetDisplayPriority()) return 1;
                else if(p1.GetDisplayPriority() < p2.GetDisplayPriority()) return -1;
                return 0;
            });

            player.OnPlayerUnReadied += RemoveReadiedPlayer;
            if(readiedPlayers.Count == 1)
                SetActivePlayer(0);
            else
                SetActivePlayer(currentPlayer);
        }

        public void RemoveReadiedPlayer(PlayerAttacker player)
        {
            //Don't remove if not present in list
            if(!readiedPlayers.Contains(player)) return;

            //Remove player
            PlayerAttacker currentlyActive = activeAttacker;
            bool removingActive = currentlyActive == player;
            readiedPlayers.Remove(player);
            player.OnPlayerUnReadied -= RemoveReadiedPlayer;

            //If you removed the active player, set a new active player
            if(removingActive)
                SetActivePlayer(0);
            else
                SetActivePlayer(currentlyActive);
            
        }

        private void SetActivePlayer(PlayerAttacker player)
        {
            int index = readiedPlayers.FindIndex(x => x == player);
            SetActivePlayer(index);
        }

        private void SetActivePlayer(int newIndex)
        {
            activePlayerIndex = newIndex;
            ActivePlayerChanged?.Invoke();
        }
        #endregion
    
        #region //Getters
        public int GetActiveAttackerNumber()
        {
            return activeCombatant.GetAttackerNumber(activeAttacker);
        }
        #endregion
    }
}