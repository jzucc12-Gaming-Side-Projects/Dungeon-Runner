using System.Collections.Generic;
using UnityEngine;

namespace DRN.COMBAT.COMBATANT
{
    [RequireComponent(typeof(Combatant))]
    public class CombatTarget : MonoBehaviour
    {
        #region //Target list
        public static List<Combatant> players { get; private set; }
        public static List<Combatant> enemies { get; private set; }
        [SerializeField] private bool isPlayerSide = false;
        #endregion

        
        #region //Monobehaviour
        private void Awake()
        {
            if(players == null) players = new List<Combatant>();
            if(enemies == null) enemies = new List<Combatant>();
        }

        private void OnEnable()
        {
            if(isPlayerSide)
                AddCombatant(players, GetComponent<Combatant>());
            else
                AddCombatant(enemies, GetComponent<Combatant>());
        }

        private void OnDisable()
        {
            if(isPlayerSide)
                players.Remove(GetComponent<Combatant>());
            else
                enemies.Remove(GetComponent<Combatant>());
        }
        #endregion

        private void AddCombatant(List<Combatant> list, Combatant newCombatant)
        {
            for(int ii = 0; ii < list.Count; ii++)
            {
                if(newCombatant.transform.GetSiblingIndex() > list[ii].transform.GetSiblingIndex()) continue;
                list.Insert(ii, newCombatant);
                return;
            }
            list.Add(newCombatant);
        }
    }
}