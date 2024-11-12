using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[CustomEditor(typeof(ObjectCopier))]
public class ObjectCopierEditor : Editor
{
    SerializedProperty templateObject;
    SerializedProperty objectNames;

    private void OnEnable()
    {
        templateObject = serializedObject.FindProperty("templateObject");
        objectNames = serializedObject.FindProperty("objectNames");
    }

    public override void OnInspectorGUI()
    {
        ObjectCopier objectCopierTarget = (ObjectCopier)target;
        
        // Update the serialized object
        serializedObject.Update();

        // Display the int field using the SerializedProperty
        EditorGUILayout.PropertyField(templateObject);
        EditorGUILayout.PropertyField(objectNames);

        // Apply any changes to the serialized object
        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Create Objects"))
        {
            objectCopierTarget.CreateObjects();
        }

    }
}

    


