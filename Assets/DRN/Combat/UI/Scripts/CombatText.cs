using System.Collections;
using DRN.COMBAT.ACTION;
using TMPro;
using UnityEngine;

namespace DRN.COMBAT.UI
{
    public class CombatText : MonoBehaviour
    {
        #region //Variables
        [SerializeField] private GameObject box = null;
        private TextMeshProUGUI combatText = null;
        #endregion


        #region //Monobehaviour
        private void Awake()
        {
            combatText = GetComponentInChildren<TextMeshProUGUI>();
            box.SetActive(false);
        }

        private void OnEnable()
        {
            SchedulerItem.DisplayText += DisplayText;
            FleeAction.FleeAttemptMessage += DisplayText;
        }

        private void OnDisable()
        {
            SchedulerItem.DisplayText -= DisplayText;
            FleeAction.FleeAttemptMessage -= DisplayText;
        }
        #endregion

        private void DisplayText(string text, float displayTime)
        {
            combatText.text = text;
            box.SetActive(true);
            StartCoroutine(ShutOffDelay(displayTime));
        }

        private IEnumerator ShutOffDelay(float time)
        {
            yield return new WaitForSeconds(time);
            box.SetActive(false);
        }
    }
}
