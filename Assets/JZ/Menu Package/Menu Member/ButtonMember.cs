using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JZ.MENU
{
    /// <summary>
    /// <para>Attach to items that you want to be recognized by a menu</para>
    /// </summary>
    public class ButtonMember : MenuMember
    {
        private Button button = null;

        protected override void Awake()
        {
            base.Awake();
            button = GetComponent<Button>();
        }

        public override void Selected() 
        { 
            button.onClick?.Invoke();
        }
    }
}