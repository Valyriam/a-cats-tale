using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(DoublePageSegment))]
public class DoublePageSpreadEditor : Editor
{
    SerializedProperty referenceMaterial;
    SerializedProperty referenceTexture;

    private void OnEnable()
    {
        referenceMaterial = serializedObject.FindProperty("referenceMaterial");
        referenceTexture = serializedObject.FindProperty("referenceTexture");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(referenceMaterial);
        EditorGUILayout.PropertyField(referenceTexture);


        DoublePageSegment dpSegment = (DoublePageSegment)target;

        dpSegment.doublePageSpreadPrefab = (GameObject)EditorGUILayout.ObjectField(dpSegment.doublePageSpreadPrefab, typeof(GameObject), false);

        if (GUILayout.Button("New Double Page Segment"))
        {
            dpSegment.CreateNewDoublePageSpread();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
