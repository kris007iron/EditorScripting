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
    //For each state selected or not we have different types of gizmos low or high resolutions
    [DrawGizmo(GizmoType.NonSelected)]
    static void DrawGizmosLoRes(SplineComponent spline, GizmoType gizmoType)
    {
        Gizmos.color = Color.white;
        DrawGizmo(spline, 64);
    }

    [DrawGizmo(GizmoType.Selected)]
    static void DrawGizmosHiRes(SplineComponent spline, GizmoType gizmoType)
    {
        Gizmos.color = Color.white;
        DrawGizmo(spline, 1024);
    }
    static void DrawGizmo(SplineComponent spline, int stepCount)
    {
        if(spline.points.Count > 0)
        {
            var P = 0f;
            var start = spline.GetNonUniformPoint(0);
            var step = 1f / stepCount;
            do
            {
                P += step;
                var here = spline.GetNonUniformPoint(P);
                Gizmos.DrawLine(start, here);
                start = here;
            } while (P + step <= 1);
        }
    }
}
