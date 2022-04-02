using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SplineComponent))]
public class SplineComponentEditor : Editor
{
    //Basic method for creating custom inspector contains definition for buttons and helbox with info
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Hold Shift and click to append and insert curve points. Backspace to delete points.", MessageType.Info);
        var spline = target as SplineComponent;
        GUILayout.BeginHorizontal();
        var closed = GUILayout.Toggle(spline.closed, "Closed", "button");
        if (spline.closed != closed)
        {
            spline.closed = closed;
            spline.ResetIndex();
        }
        if(GUILayout.Button("Flatten Y Axis"))
        {
            Undo.RecordObject(target, "Flatten Y Axis");
            //TODO: Flatten(spline.points)
            spline.ResetIndex();
        }
        if (GUILayout.Button("Center around Origin")) 
        {
            Undo.RecordObject(target, "Center around Origin");
            //TODO: CenterArondOrigin(spline.points);
            spline.ResetIndex();
        }
        GUILayout.EndHorizontal();
    }
}
