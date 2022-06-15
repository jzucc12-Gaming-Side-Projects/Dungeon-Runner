using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class CreateNewEnemy : EditorWindow
{
    [MenuItem("Window/Create New Enemy")]
    public static void ShowEditorWindow()
    {
        GetWindow(typeof(CreateNewEnemy), false, "Create New Enemy");
    }
}