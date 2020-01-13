using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomGen : EditorWindow

{
    int width = 0;
    int height = 0;
    bool groupEnabled;
    [Tooltip("Shade of the room")]
    [ColorUsage(showAlpha: false, hdr: true)]
    Color color;
    [Tooltip("Number of objects in the room")]
    int objects = 0;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/Room Generator")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(RoomGen));
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        width = EditorGUILayout.IntSlider("Width", width, 0, 50);
        height = EditorGUILayout.IntSlider("Height", height, 0, 50);

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        color = EditorGUILayout.ColorField("Color", color);
        objects = EditorGUILayout.IntField("Objects", objects);
        

        EditorGUILayout.EndToggleGroup();
    }
}
