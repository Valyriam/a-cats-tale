using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectCopier : MonoBehaviour
{
#if UNITY_EDITOR
    public GameObject templateObject;
    public string[] objectNames;

    private void Start()
    {
        templateObject = this.gameObject;
    }

    public void CreateObjects()
    {
        foreach (string objectName in objectNames)
        {
            GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(templateObject);
            GameObject newObject;

            if (prefab != null)
                newObject = PrefabUtility.InstantiatePrefab(prefab, templateObject.transform.parent) as GameObject;

            else
                newObject = Instantiate(prefab, templateObject.transform.position, templateObject.transform.rotation, templateObject.transform.parent);

            newObject.gameObject.name = objectName;

            Undo.RegisterCreatedObjectUndo(newObject, "Duplicate");
        }
    }
#endif
}
