using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SolvedPositionDisplayManager))]
public class SolvedPositionDisplayManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SolvedPositionDisplayManager spdManagerTarget = (SolvedPositionDisplayManager)target;

        if (GUILayout.Button("Solved Positions Only"))
        {
            spdManagerTarget.SetSolvedPositionsOnlyVisible();
        }

        if (GUILayout.Button("Unsolved Positions Only"))
        {
            spdManagerTarget.SetUnsolvedPositionsOnlyVisible();
        }

        if (GUILayout.Button("Show All"))
        {
            spdManagerTarget.SetAllPositionsVisible();
        }

        if (GUILayout.Button("Update Object Data"))
        {
            spdManagerTarget.UpdateObjectData();
        }
    }
}
