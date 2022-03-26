using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Launcher))]
public class LauncherEditor : Editor
{
    //This method works only when scene is rendering it allows us to draw gizmos etc inside the scene view for configuration purposes etc.
    void OnSceneGUI()
    {
        var launcher = target as Launcher;

        //Some transform logic between World POV and parent launcher POV
        //Also this code create extra pivot for setting up offset from scene without starting game
        var transform = launcher.transform;
        launcher.offset = transform.InverseTransformDirection(
            Handles.PositionHandle(
                transform.TransformPoint(launcher.offset),
                transform.rotation));

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
