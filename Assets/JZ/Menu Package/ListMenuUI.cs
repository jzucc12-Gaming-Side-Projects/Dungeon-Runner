using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JZ.INPUT;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JZ.MENU.UI
{
    /// <summary>
    /// UI for 1D menus consisting of multiple items
    /// </summary>
    public class ListMenuUI : MenuUI
    {
        #region //Menu components
        [Header("Menu Components")]
        [Tooltip("Parent GO for list items")] [SerializeField] protected GameObject memberContainer = null;
        [SerializeField] protected Transform selectionArrow = null;
        protected List<MenuMember> members = new List<MenuMember>();
        protected int activeIndex = 0;
        protected MenuMember activeMember => members[activeIndex];
        protected MenuingInputSystem inputSystem = null;
        #endregion

        #region //Menu options
        [Header("Menu Options")]
        [Tooltip("Does the menu navigate vertically?")] [SerializeField] private bool verticalMenu = true;
        [Tooltip("Does the menu loop past the last item?")] [SerializeField] private bool menuLoops = false;
        [Tooltip("Does the menu cursor reset when he menu closes?")] [SerializeField] private bool resetOnDisable = false;
        #endregion


        #region //Monobehaviour
        protected override void Awake()
        {
            base.Awake();
            inputSystem = FindObjectOfType<MenuingInputSystem>();
            members = memberContainer.GetComponentsInChildren<MenuMember>().ToList();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            EnableControls();
            StartCoroutine(SetUI());
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            DisableControls();

            if(resetOnDisable)
                activeIndex = 0;
        }
        #endregion

        #region //Activation
        public override void LockMenu()
        {
            base.LockMenu();
            DisableControls();
        }

        public override void UnlockMenu()
        {
            base.UnlockMenu();
            EnableControls();
        }

        private void EnableControls()
        {
            EnableNavigation();
            inputSystem.select.started += Select;
        }

        private void DisableControls()
        {
            DisableNavigation();
            inputSystem.select.started -= Select;
        }

        private void EnableNavigation()
        {
            if(verticalMenu)
            {
                inputSystem.yNavigate.started += MainNavigate;
                inputSystem.xNavigate.started += OffNavigate;
            }
            else
            {
                inputSystem.xNavigate.started += MainNavigate;
                inputSystem.yNavigate.started += OffNavigate;
            }
        }

        private void DisableNavigation()
        {
            if(verticalMenu)
            {
                inputSystem.yNavigate.started -= MainNavigate;
                inputSystem.xNavigate.started -= OffNavigate;
            }
            else
            {
                inputSystem.xNavigate.started -= MainNavigate;
                inputSystem.yNavigate.started -= OffNavigate;
            }
        }
        
        private IEnumerator SetUI()
        {
            yield return new WaitForEndOfFrame();
            ChangeIndex(activeIndex);
        }
        #endregion

        #region //Menu navigation
        private void MainNavigate(InputAction.CallbackContext context)
        {
            int direction = (int)context.ReadValue<float>();
            int newIndex = activeIndex + direction;

            if(menuLoops)
            {
                newIndex = JZMathUtils.Wrap(newIndex, 0, members.Count - 1);
            }
            else
            {
                newIndex = Mathf.Clamp(newIndex, 0, members.Count - 1);
            }

            ChangeIndex(newIndex);
        }

        private void OffNavigate(InputAction.CallbackContext context)
        {
            //Checks to see if this item has an off axis reader

            // int direction = (int)context.ReadValue<float>();
            // int newIndex = activeIndex + direction;

            //Tells the off axis reader the input
        }

        protected virtual void ChangeIndex(int newIndex)
        {
            //Exit last position
            activeMember.Hover(false);

            //Enter new position
            activeIndex = newIndex;
            activeMember.Hover(true);
            MoveArrow();
        }

        private void MoveArrow()
        {
            //Store current arrow position
            Vector2 newPostion = selectionArrow.position;

            //Set new arrow position
            int index = verticalMenu ? 1 : 0;
            newPostion[index] = activeMember.transform.position[index];
            selectionArrow.position = newPostion;
        }
        #endregion

        #region //Menu selection
        private void Select(InputAction.CallbackContext context)
        {
            if(activeMember == null) return;
            activeMember.Selected();
        }
        #endregion
    }
}