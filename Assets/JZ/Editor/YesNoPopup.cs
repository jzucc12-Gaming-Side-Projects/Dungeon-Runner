using System;
using JZ.EDITOR.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Used for pop up notifications in a tool
/// </summary>
namespace JZ.EDITOR
{
    public class YesNoPopup : EditorWindow
    {
        #region //UI Elements
        private VisualElement container = null;
        private Label titleLabel = null;
        private Label contentLabel = null;
        private Button yesButton = null;
        private Button noButton = null;
        #endregion

        #region //Window variables
        private static string editorPath = JZGlobals.editorPath;
        private static Vector2 defaultWindowSize = new Vector2(350, 300);
        public event Action<bool> Closed; //Returning true continues execution. Returning false aborts current operation.
        private bool yesContinues = true; //If true, the "YES" button continues operation. If false, it aborts the operation.
        #endregion


        #region //Initialization
        [MenuItem("Tools/Test Popup")]
        public static YesNoPopup CreateAtOrigin()
        {
            return CreateWindow(Vector2.zero, false, true);
        } 

        public static YesNoPopup CreateWindow(float xPos, float yPos, float xSize, float ySize, bool center = true, bool show = true)
        {
            return CreateWindow(new Rect(xPos, yPos, xSize, ySize), center, show);
        }

        public static YesNoPopup CreateWindow(float xPos, float yPos, Vector2 size, bool center = true, bool show = true)
        {
            return CreateWindow(new Rect(new Vector2(xPos, yPos), size), center, show);
        }

        public static YesNoPopup CreateWindow(Vector2 position, Vector2 size, bool center = true, bool show = true)
        {
            return CreateWindow(new Rect(position, size), center, show);
        }

        public static YesNoPopup CreateWindow(Vector2 position, bool center = true, bool show = true)
        {
            return CreateWindow(new Rect(position, defaultWindowSize), center, show);
        }

        public static YesNoPopup CreateWindow(Rect rect, bool center = true, bool show = true)
        {
            YesNoPopup window = ScriptableObject.CreateInstance<YesNoPopup>();
            if(center)
            {
                rect.x -= defaultWindowSize.x/2;
                rect.y -= defaultWindowSize.y/2;
            }

            window.position = rect;
            if(show) window.ShowPopup();
            return window;
        }

        public void CreateGUI() => SetUpGUI();

        public void SetUp(string titleText, string content, bool doesYesContinue, string yesText = "YES", string NoText = "NO")
        {
            SetUpGUI();
            yesContinues = doesYesContinue;
            titleLabel.text = titleText;
            contentLabel.text = content;
            yesButton.text = yesText;
            noButton.text = NoText;
        }

        private void SetUpGUI()
        {
            if(container != null) return;
            container = this.SetUpWindow($"{editorPath}/YesNoPopUp.uxml", $"{editorPath}/JZBaseEditorUSS.uss");
            titleLabel = container.Q<Label>("title");
            contentLabel = container.Q<Label>("content");
            yesButton = container.Q<Button>("yesButton");
            noButton = container.Q<Button>("noButton");
            yesButton.clicked += (() => CloseMenu(true));
            noButton.clicked += (() => CloseMenu(false));
        }
        #endregion

        #region //Closing window
        private void CloseMenu(bool yes)
        {
            Closed?.Invoke(!(yes ^ yesContinues));
            this.Close();
        }
        #endregion
    }
}