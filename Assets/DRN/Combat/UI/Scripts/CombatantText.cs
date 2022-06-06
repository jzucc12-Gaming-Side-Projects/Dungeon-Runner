using TMPro;
using UnityEngine;

namespace DRN.COMBAT.UI
{
    public class CombatantText : MonoBehaviour
    {
        private TextMeshProUGUI combatText = null;


        private void Awake()
        {
            combatText = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            Deactivate();
        }

        public void Activate(string text, Color color)
        {
            combatText.color = color;
            Activate(text);
        }

        public void Activate(string text)
        {
            gameObject.SetActive(true);
            combatText.text = text;
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}