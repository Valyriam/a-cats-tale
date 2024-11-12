using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[CustomEditor(typeof(ProportionalChildPositioner))]
public class ProportionalChildPositionerEditor : Editor
{
    SerializedProperty xValueOffset;
    SerializedProperty yValueOffset;
    SerializedProperty zValueOffset;

    private void OnEnable()
    {
        xValueOffset = serializedObject.FindProperty("xValueOffset");
        yValueOffset = serializedObject.FindProperty("yValueOffset");
        zValueOffset = serializedObject.FindProperty("zValueOffset");
    }

    public override void OnInspectorGUI()
    {
        ProportionalChildPositioner pCPTarget = (ProportionalChildPositioner)target;

        if (GUILayout.Button("Reposition Children"))
        {
            pCPTarget.RepositionChildrenBasedOnFirstChild();
        }

        if (GUILayout.Button("Update Children"))
        {
            pCPTarget.UpdateChildren();
        }

        // Update the serialized object
        serializedObject.Update();

        // Display the int field using the SerializedProperty
        EditorGUILayout.PropertyField(xValueOffset);
        EditorGUILayout.PropertyField(yValueOffset);
        EditorGUILayout.PropertyField(zValueOffset);

        // Apply any changes to the serialized object
        serializedObject.ApplyModifiedProperties();

    }
}

    


