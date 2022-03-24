using UnityEngine;
using UnityEditor;

public class MenuItemsExample
{
    //New menu item under an existing menu in top toll bar
    //Also you can add hotkeys after name of window button etc.
    // % – CTRL on Windows / CMD on OSX
    //# – Shift
    //& – Alt
    //LEFT/RIGHT/UP/DOWN – Arrow keys
    // F1…F2 – F keys
    //HOME, END, PGUP, PGDN
    //alone letters need to be provided with _ at first
    //You can combine these like Ctrl+A or whatever you want
    //Assets - under PPM in project
    //Assets/Create - new create object in context menu
    //CONTEXT/ComponentName  PPM in inspector of component

    [MenuItem("Windows/New Option %#&a")]
    private static void NewMenuOption()
    {
        //Your code like in Menu Items
    }

    //New menu item but with multiple levels of nesting(context menu)

    [MenuItem("Tools/SubMenu/Option")]
    private static void NewNestedOption()
    {
        //Your code like in Menu Items
        //You can also open here custom window like in Eidtor Scripting cs
    }

    //Add a new item that is accessed by right-clicking on an asset in the project view
    //Obsolete can work unproperly

    [MenuItem("Assets/Load Additive Scene")]
    [System.Obsolete]
    private static void LoadAdditiveScene()
    {
        var selected = Selection.activeObject;
        EditorApplication.OpenSceneAdditive(AssetDatabase.GetAssetPath(selected));
    }

    //Adding a new menu item under Assets.Create

    [MenuItem("Assets/Create/Add Configuration")]
    private static void AddConfig()
    {
        // Create and add a new ScriptableObject for storing configuration
    }

    //Add a new menu item that is accessed byr right-clicking inside the RigidBody component

    [MenuItem("CONTEXT/Rigidbody/New Option")]
    private static void NewOpenForRigidBody()
    {

    }

    //Validation, condition for extended menus etc.

    [MenuItem("Assets/ProcessTexture")]
    private static void DoSomethingWithTexture()
    {

    }

    //Note that we pass the same path and also pass "true" to the second argument

    [MenuItem("Assets/ProcessTexture", true)]
    private static bool NewMenuOptionValidation()
    {
        return Selection.activeObject.GetType() == typeof(Texture3D);

    } // The condtition wont let function run if GO isn't the needed type}
}

