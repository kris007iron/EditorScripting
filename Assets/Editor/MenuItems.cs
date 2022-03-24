using UnityEngine;
using UnityEditor;

public class MenuItems
{
    //New Item on top toll bar with new Option which clears PlayerPrefs table
    [MenuItem("Tools/Clear Player Prefs")]
    private static void NewMenuOption()
    {
        PlayerPrefs.DeleteAll();
    }
}
