using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ProportionalChildPositioner : MonoBehaviour
{
    [SerializeField] float xValueOffset;
    [SerializeField] float yValueOffset;
    [SerializeField] float zValueOffset;

    List<Transform> childrenTransforms = new List<Transform>();

    private void Start()
    {
        //add all children except the first one
        for (int i = 1; i < (transform.childCount - 1); i++)
        {
            childrenTransforms.Add(transform.GetChild(i));
        }
    }

    public void RepositionChildrenBasedOnFirstChild()
    {
        Vector3 startPosition = transform.GetChild(0).position;
        int offsetMultiplier = 0; //multiply offset by this much to ensure they position proportionally

        foreach (Transform child in childrenTransforms)
        {
            offsetMultiplier++;
            child.position = new Vector3(startPosition.x + (xValueOffset * offsetMultiplier), startPosition.y + (yValueOffset * offsetMultiplier), startPosition.z + (zValueOffset * offsetMultiplier));
        }
    }

    public void UpdateChildren()
    {
        childrenTransforms.Clear();
        Start();
    }
}
