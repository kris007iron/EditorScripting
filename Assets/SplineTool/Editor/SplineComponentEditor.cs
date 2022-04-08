using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SplineComponent))]
public class SplineComponentEditor : Editor
{
    int hotIndex = -1;
    //We need to set it to -1 otherwise in OnSceneGUI we will delete point every frame we dont want do that
    int removeIndex = -1;

    private void OnSceneGUI()
    {
        var spline = target as SplineComponent;


        var e = Event.current;
        GUIUtility.GetControlID(FocusType.Passive);

        //Necessery variables for screent position orientating and checking if spline is "on"
        var mousePos = (Vector2)Event.current.mousePosition;
        var view = SceneView.currentDrawingSceneView.
            camera.ScreenToViewportPoint(
            Event.current.mousePosition);


        var mouseIsOutside = view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1;
        if (mouseIsOutside) return;

        var points = serializedObject.FindProperty("points");


        //Checked if shift was clicked 
        if (Event.current.shift)
        {
            //For diferent states of spline diferent actions
            if (spline.closed)
            {
                ShowClosestPointOnClosedSpline(points);
            }
            else
            {
             
                ShowClosestPointOnOpenSpline(points);
            }
        }

        for (int i = 0; i < spline.points.Count; i++)
        {
            var prop = points.GetArrayElementAtIndex(i);
            var point = prop.vector3Value;
            var wp = spline.transform.TransformPoint(point);

            //This condition will allow user to move the selected point of spline
            if (hotIndex == i)
            {
                //Drawing control widgets
                var newWp = Handles.PositionHandle(
                    wp,
                    Tools.pivotRotation == PivotRotation.Global ? Quaternion.identity : spline.transform.rotation
                    );
                var delta = spline.transform.InverseTransformDirection(newWp - wp);
                if(delta.sqrMagnitude > 0)
                {
                    prop.vector3Value = point + delta;
                    spline.ResetIndex();
                }
                HandleCommands(wp);
            }
            //Also we need to create some cind of buttons for points they will behave like normal but the will have shape of circle
            Handles.color = i == 0 | i == spline.points.Count - 1 ? Color.red : Color.white;
            var buttonSize = HandleUtility.GetHandleSize(wp) * 0.1f;
            if (Handles.Button(wp, Quaternion.identity, buttonSize, buttonSize, Handles.SphereHandleCap))
                hotIndex = i;
            var v = SceneView.currentDrawingSceneView.camera.transform.InverseTransformPoint(wp);
            var labelIsOutside = v.z < 0;
            //drawing and index of control point in case we need to debug somethin
            if (!labelIsOutside) Handles.Label(wp, i.ToString());
            if (removeIndex >= 0 && points.arraySize > 4)
            {
                points.DeleteArrayElementAtIndex(removeIndex);
                spline.ResetIndex();
            }
            removeIndex = -1;
            serializedObject.ApplyModifiedProperties();
        }
    }
    void HandleCommands(Vector3 wp)
    {
        //We intercept the command here, so that instead of framing the game object which the spline component is attached to, we frame the hot control point.
        if(Event.current.type == EventType.ExecuteCommand)
        {
            SceneView.currentDrawingSceneView.Frame(new Bounds(wp, Vector3.one * 10), false);
            Event.current.Use();
        }
        if(Event.current.type == EventType.KeyDown)
        {
            if(Event.current.keyCode == KeyCode.Backspace)
            {
                removeIndex = hotIndex;
                Event.current.Use();
            }
        }
    }
    //Both methods check for the lefti click to use SerializedProperty API to insert item into list and to sets new cords for it
    //Also both of them use closest point so we this part of code split out into separate method
    void ShowClosestPointOnClosedSpline(SerializedProperty points)
    {
        //The method is called on shift press one of pair of methods called there on diferent condition depends on if spline is closed or not also it shows the extended line of predicted closed spline
        var spline = target as SplineComponent;
        var plane = new Plane(spline.transform.up, spline.transform.position);
        var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        float center;
        if (plane.Raycast(ray, out center))
        {
            var hit = ray.origin + ray.direction * center;
            Handles.DrawWireDisc(hit, spline.transform.up, 5);
            var p = SearchForClosestPoint(Event.current.mousePosition);
            var sp = spline.GetNonUniformPoint(p);
            Handles.DrawLine(hit, sp);

            if(Event.current.type == EventType.MouseDown && Event.current.button == 0 && Event.current.shift)
            {
                var i = (Mathf.FloorToInt(p * spline.points.Count) + 2) % spline.points.Count;
                points.InsertArrayElementAtIndex(i);
                points.GetArrayElementAtIndex(i).vector3Value = spline.transform.InverseTransformPoint(sp);//from world space to local space
                serializedObject.ApplyModifiedProperties();
                hotIndex = i;
            }
        }
    }
    void ShowClosestPointOnOpenSpline(SerializedProperty points)
    {
        var spline = target as SplineComponent;
        var plane = new Plane(spline.transform.up, spline.transform.position);
        var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        float center;
        if(plane.Raycast(ray, out center))
        {
            var hit = ray.origin + ray.direction * center;
            var discSize = HandleUtility.GetHandleSize(hit);
            Handles.DrawWireDisc(hit, spline.transform.up, discSize);
            var p = SearchForClosestPoint(Event.current.mousePosition);

            if ((hit - spline.GetNonUniformPoint(0)).sqrMagnitude < 25) p = 0;
            if ((hit - spline.GetNonUniformPoint(1)).sqrMagnitude < 25) p = 1;

            var sp = spline.GetNonUniformPoint(p);
            var extend = Mathf.Approximately(p, 0) || Mathf.Approximately(p, 1);

            Handles.color = extend ? Color.red : Color.white;
            Handles.DrawLine(hit, sp);            
            Handles.color = Color.white;

            var i = 1 + Mathf.FloorToInt(p * (spline.points.Count - 3));

            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Event.current.shift)
            {
                if (extend)
                {
                    if (i == spline.points.Count - 2) i++;
                    points.InsertArrayElementAtIndex(i);
                    points.GetArrayElementAtIndex(i).vector3Value = spline.transform.InverseTransformPoint(hit);
                    hotIndex = i;
                }
                else
                {
                    i++;
                    points.InsertArrayElementAtIndex(i);
                    points.GetArrayElementAtIndex(i).vector3Value = spline.transform.InverseTransformPoint(sp);
                    hotIndex = i;
                }
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
    float SearchForClosestPoint(Vector2 screenPoint, float A = 0f, float B = 1f, float steps = 1000)
    {
        var spline = target as SplineComponent;
        var smallestDelta = float.MaxValue;
        var step = (B - A) / steps;
        var closestI = A;
        for (var i = 0; i <= steps; i++)
        {
            var p = spline.GetNonUniformPoint(i * step);
            var gp = HandleUtility.WorldToGUIPoint(p);
            var delta = (screenPoint - gp).sqrMagnitude;
            if(delta < smallestDelta)
            {
                closestI = i;
                smallestDelta = delta;
            }
        }
        return closestI * step;
    }
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
