using DRN.COMBAT.COMBATANT;
using UnityEngine;

namespace DRN.COMBAT.UI
{
    public class BattleBarUI : MonoBehaviour
    {
        private CombatAttacker player = null;
        [SerializeField] Transform fillBar = null;
        [SerializeField] GameObject maxedBar = null;

 
        public void SetAttacker(CombatAttacker player) { this.player = player; }

        private void Update()
        {
            float fillAmount = player.GetBBPercentage();
            fillBar.localScale = new Vector3(fillAmount, 1, 1);
            maxedBar.SetActive(fillAmount == 1);
        }
    }
}
