using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SimpleIKSolver))]
public class SimpleIKSolverEditor : Editor
{
    //inspector customization
    static GUIStyle errorBox;

    private void OnEnable()
    {
        errorBox = new GUIStyle(EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).box);
        errorBox.normal.textColor = Color.red;
    }
    public override void OnInspectorGUI()
    {        
        var s = target as SimpleIKSolver;
        if(s.pivot == null || s.upper == null || s.lower == null | s.effector == null || s.tip == null)
        {
            EditorGUILayout.HelpBox("Please assign Pivot, Upper, Lower, Effector and Tip transforms.", MessageType.Error);
        }
        base.OnInspectorGUI();
    }
}
