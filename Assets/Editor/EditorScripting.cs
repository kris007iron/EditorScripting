using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorScripting : EditorWindow
{
    //Basic variables
    private string custString = "String here";
    private bool groupEnabled;
    private bool optionalSettings;
    private float jumMod = 1.0f;
    private float impactMod = 0.5f;

    //Adding new window named Test and its simple implementation in context menu
    [MenuItem(("Window / Test"))]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(EditorScripting));
    }

    private void OnGUI()
    {
        // OnGUI Method Contents
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        custString = EditorGUILayout.TextField("Text Field", custString);

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        optionalSettings = EditorGUILayout.Toggle("Double Jumping Enabled", optionalSettings);
        jumMod = EditorGUILayout.Slider("Jump Modifier", jumMod, -5, 5);
        impactMod = EditorGUILayout.Slider("Impact Modifier", impactMod, -5, 5);
        EditorGUILayout.EndToggleGroup();

        GUI.backgroundColor = Color.red;

        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Reset", GUILayout.Width(100), GUILayout.Height(30)))
        {
            custString = "String Here";
            optionalSettings = false;
            jumMod = 1.0f;
            impactMod = .5f;
        }
        EditorGUILayout.EndHorizontal();
    }
}
