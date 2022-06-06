using System.Collections;
using System.Collections.Generic;
using DRN.COMBAT.COMBATANT;
using DRN.COMBAT.TARGETING;
using UnityEngine;

namespace DRN.COMBAT.UI
{
    public abstract class CombatantUI : MonoBehaviour
    {
        #region //Cached components
        protected Combatant myCombatant = null;
        #endregion

        #region //Main Targeting
        [Header("Main Targeting")]
        [SerializeField] private GameObject targetingArrowPrefab = null;
        [SerializeField] private Vector2 targetingArrowOffset = Vector2.zero;
        private GameObject targetingArrow = null;
        protected MainTargeter targeter = null;
        #endregion

        #region //Sub-Targeting
        [Header("Sub-Targeting")]
        [SerializeField] GameObject subTargetArrowPrefab = null;
        [SerializeField] private Vector2 subTargetArrowOffset = Vector2.zero;
        private List<GameObject> subTargetingArrows = new List<GameObject>();
        private SubTargeter subTargeter = null;
        #endregion

        #region //Text display
        [Header("Text Display")]
        [SerializeField] private CombatantText damageDisplay = null;
        [SerializeField] private CombatantText messageDisplay = null;
        #endregion


        #region //Monobehaviour
        protected virtual void Awake()
        {
            myCombatant = GetComponentInParent<Combatant>();
            targeter = FindObjectOfType<MainTargeter>();
            subTargeter = FindObjectOfType<SubTargeter>();
            InitializeMainTargetingArrow();
            InitializeSubTargetingArrows();
        }

        protected virtual void OnEnable()
        {
            myCombatant.ShowMessage += ShowMessage;
            myCombatant.ShowDamage += ShowDamageText;

            if(myCombatant.HasMultipleAttackers()) 
            {
                subTargeter.TargetChanged += SetSubTargeting;
                subTargeter.StopSubTargeter += ResetSubTargeting;
            }
        }

        protected virtual void OnDisable()
        {
            myCombatant.ShowMessage -= ShowMessage;
            myCombatant.ShowDamage -= ShowDamageText;

            if(myCombatant.HasMultipleAttackers()) 
            {
                subTargeter.TargetChanged -= SetSubTargeting;
                subTargeter.StopSubTargeter -= ResetSubTargeting;
            }
        }

        protected virtual void Start() 
        { 
            myCombatant.body.OnBodyDied += (() => StartCoroutine(TurnOff()));
            myCombatant.body.OnBodyRevived += (() => enabled = true);
        }
        #endregion

        #region //Initialization
        private void InitializeMainTargetingArrow()
        {
            targetingArrow = Instantiate(targetingArrowPrefab, transform);
            targetingArrow.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            targetingArrow.transform.position += (Vector3)targetingArrowOffset;
            ResetTargeting();
        }

        private void InitializeSubTargetingArrows()
        {
            if(!myCombatant.HasMultipleAttackers()) return;
            foreach (var attacker in myCombatant.GetAttackers())
            {
                var arrow = Instantiate(subTargetArrowPrefab, transform);
                arrow.transform.position = (Vector2)attacker.transform.position + subTargetArrowOffset;
                subTargetingArrows.Add(arrow);
            }
            ResetSubTargeting();
        }
        
        private IEnumerator TurnOff() 
        { 
            yield return new WaitForEndOfFrame();
            enabled = false; 
        }
        #endregion

        #region //Targeting
        protected void SetTargeting(List<Combatant> targets)
        {
            targetingArrow.SetActive(targets.Contains(myCombatant));
        }

        protected void ResetTargeting()
        {
            targetingArrow.SetActive(false);
        }

        private void SetSubTargeting(CombatAttacker attacker, int index)
        {
            if(!myCombatant.HasCC(attacker)) return;
            for(int ii = 0; ii < subTargetingArrows.Count; ii++)
                subTargetingArrows[ii].SetActive(index == ii);
        }

        private void ResetSubTargeting()
        {
            foreach(var arrow in subTargetingArrows)
                arrow.SetActive(false);
        }
        #endregion

        #region //Message Display
        private void ShowDamageText(int amount, string suffix)
        {
            var color = amount > 0 ? Color.white : Color.green;
            damageDisplay.Activate($"{Mathf.Abs(amount).ToString()}{suffix}", color);
        }

        private void ShowMessage(string message)
        {
            messageDisplay.Activate(message);
        }
        #endregion
    }
}