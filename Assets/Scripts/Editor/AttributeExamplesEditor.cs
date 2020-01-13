using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AttributeExamples))]
public class AttributeExamplesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Do something"))
        {
            Debug.Log("doing smth");
        }
    }
}
