using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SomeScript))]
public class SomeScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //Default displaying like normall but allows for further customizations 

        DrawDefaultInspector();

        //Displays help box or different types of information like warning or error

        EditorGUILayout.HelpBox("This is a help box", MessageType.Info);

        //Other things we can display like in EditorScripting also we can use LabelField which just displays some text etc
    }
}
