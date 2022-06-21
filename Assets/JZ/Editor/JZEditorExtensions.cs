using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace JZ.EDITOR.Extensions
{
    public static class JZEditorWindowExtentions
    {
        public static VisualElement SetUpWindow(this EditorWindow window, string uxmlPath, params string[] ussPaths)
        {
            var container = window.rootVisualElement;
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);
            container.Add(visualTree.Instantiate());

            foreach(var ussPath in ussPaths)
            {
                StyleSheet stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(ussPath);
                container.styleSheets.Add(stylesheet);
            }
            return container;
        }

        public static void SetUpVisualElement(this VisualElement container, string uxmlPath, params string[] ussPaths)
        {
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);
            container.Add(visualTree.Instantiate());

            foreach(var ussPath in ussPaths)
            {
                StyleSheet stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(ussPath);
                container.styleSheets.Add(stylesheet);
            }
        }

        public static IEnumerator<bool?> WaitForPopup(this VisualElement element, YesNoPopup popup)
        {
            element.visible = false;
            bool abort = false;
            popup.Closed += ((bool continueOperation) => abort = !continueOperation);
            while(popup != null) yield return null;
            yield return abort;
            element.visible = true;
        }
    }
}
