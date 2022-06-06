using System.Collections;
using System.Collections.Generic;
using DRN.COMBAT.COMBATANT;
using UnityEngine;

namespace DRN.COMBAT.ACTION
{
    /// <summary>
    /// Maintains a queue of all active combat actions and enacts them one at a time.
    /// </summary>
    public class CombatActionScheduler : MonoBehaviour
    {
        private bool isRunning = false;
        private List<SchedulerItem> queuedActions = new List<SchedulerItem>();
        private CombatManager manager = null;


        #region //Monobehaviour
        private void Awake()
        {
            manager = GetComponent<CombatManager>();
        }
        #endregion

        #region //Action management
        public void AddAction(SchedulerItem item)
        {
            if(item.HighPriority()) queuedActions.Insert(0, item);
            else queuedActions.Add(item);

            if(!isRunning)
                StartCoroutine(PerformActions());
        }

        private IEnumerator PerformActions()
        {
            isRunning = true;
            while(queuedActions.Count > 0)
            {
                var item = queuedActions[0];
                queuedActions.RemoveAt(0);
                yield return item.Perform();
                queuedActions.RemoveAll(item => !item.ItemValid());
                manager.EndGameCheck();
            }
            isRunning = false;
        }
        #endregion
    }
}