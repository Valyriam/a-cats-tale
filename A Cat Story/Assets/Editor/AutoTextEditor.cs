using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(AutoText)), CanEditMultipleObjects]
public class AutoTextEditor : Editor
{
    SerializedProperty myText;

    private void OnEnable()
    {
        myText = serializedObject.FindProperty("myText");
    }

    public override void OnInspectorGUI()
    {
        var allScripts = targets;
      
        if (GUILayout.Button("Set Text To Name"))
        {
            foreach (var script in allScripts)
            {
                SerializedObject obj = new SerializedObject(script);
                ((AutoText)script).SetTextToName();
            }
        }

        if (GUILayout.Button("Collect Text Reference"))
        {
            foreach (var script in allScripts)
            {
                ((AutoText)script).CollectTextReference();
            }
        }

        EditorGUILayout.PropertyField(myText);

        serializedObject.ApplyModifiedProperties();
    }
}

#endif