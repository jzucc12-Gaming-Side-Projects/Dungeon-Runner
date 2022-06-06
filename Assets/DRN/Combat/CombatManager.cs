using System;
using System.Collections.Generic;
using DRN.COMBAT.COMBATANT;
using UnityEngine;

namespace DRN.COMBAT
{
    public class CombatManager : MonoBehaviour
    {
        private List<Combatant> players => CombatTarget.players;
        private List<Combatant> enemies => CombatTarget.enemies;
        public event Action<string, Dictionary<string, int>> OnEndCombat;


        public void EndGameCheck()
        {
            if(players.Count == 0)
                EndCombat("You Fled!");
            else if(players.GetLiving().Count == 0)
                EndCombat("Combat Defeat!");
            else if(enemies.GetLiving().Count == 0)
                EndCombat("Combat Victory!");
        }

        private void EndCombat(string text)
        {
            Time.timeScale = 0;

            Dictionary<string, int> defeatedEnemies = new Dictionary<string, int>();
            foreach(var enemy in enemies)
            {
                if(enemy.IsAlive()) continue;
                if(defeatedEnemies.ContainsKey(enemy.GetName())) defeatedEnemies[enemy.GetName()]++;
                else defeatedEnemies.Add(enemy.GetName(), 1);
            }
            OnEndCombat?.Invoke(text, defeatedEnemies);
        }

    }
}