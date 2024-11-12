using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(AutoOutline))]
public class AutoOutlineInEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AutoOutline autoOutline = (AutoOutline)target;

        if (GUILayout.Button("Resize Outline Particles"))
        {
            autoOutline.ResizeOutlineParticles();
        }

        if (GUILayout.Button("Update Particle Systems"))
        {
            autoOutline.CollectParticleSystems();
        }
    }
}
#endif