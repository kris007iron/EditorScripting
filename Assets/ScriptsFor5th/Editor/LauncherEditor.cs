using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Launcher))]
public class LauncherEditor : Editor
{

    //This function is drawing a dotted line how far the projectile will fly on selected obj and with current speed and mass also the field of projectile cant be null
    [DrawGizmo(GizmoType.Pickable | GizmoType.Selected)]
    static void DrawGizmoSelected(Launcher launcher, GizmoType gizmoType)
    {
        var offsetPosition = launcher.transform.position + launcher.offset;
        Handles.DrawDottedLine(launcher.transform.position, offsetPosition, 3);
        Handles.Label(offsetPosition, "Offset");
        if (launcher.projectile != null)
        {
            var positions = new List<Vector3>();
            var velocity = launcher.transform.forward *
                           launcher.velocity /
                           launcher.projectile.mass;
            var position = offsetPosition;
            var physicsStep = 0.1f;
            //This code add points which takes gravity in calculations, this creates a curved line in scene view
            for (var i = 0f; i <= 1f; i += physicsStep)
            {
                positions.Add(position);
                position += velocity * physicsStep;
                velocity += Physics.gravity * physicsStep;
            }
            //final estimated point
            using (new Handles.DrawingScope(Color.yellow))
            {
                Handles.DrawAAPolyLine(positions.ToArray());
                Gizmos.DrawWireSphere(positions[positions.Count - 1], 0.125f);
                Handles.Label(positions[positions.Count - 1], "Estimated Position (1 sec)");
            }
        }
    }

    //This method works only when scene is rendering it allows us to draw gizmos etc inside the scene view for configuration purposes etc.
    void OnSceneGUI()
    {
        var launcher = target as Launcher;
        var transform = launcher.transform;
        //Some update was made here to provide new option - undo
        using (var cc = new EditorGUI.ChangeCheckScope())
        {
            var newOffset = transform.InverseTransformDirection(
                Handles.PositionHandle(
                    transform.TransformPoint(launcher.offset),
                    transform.rotation));
            if (cc.changed)
            {
                Undo.RecordObject(launcher, "Offset Change");
                launcher.offset = newOffset;
            }
        }
        //Some transform logic between World POV and parent launcher POV
        //Also this code create extra pivot for setting up offset from scene without starting game
        

        //Now we gonna to expose fire method as an button in editor view but only during play mode 
        Handles.BeginGUI();
        var rectMin = Camera.current.WorldToScreenPoint(
            launcher.transform.position + launcher.offset);
        var rect = new Rect();
        rect.xMin = rectMin.x;
        rect.yMin = SceneView.currentDrawingSceneView.position.height - rectMin.y;
        rect.width = 64;
        rect.height = 18;
        GUILayout.BeginArea(rect);
        using (new EditorGUI.DisabledGroupScope(!Application.isPlaying))
        {
            if(GUILayout.Button("Fire"))
                launcher.Fire();
        }
        GUILayout.EndArea();
        Handles.EndGUI();
        //Button behaves that it is always facing user and it is not working during editor mode as said before
    }

}
