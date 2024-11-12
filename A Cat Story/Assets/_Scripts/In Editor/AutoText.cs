using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

[ExecuteInEditMode]
public class AutoText : MonoBehaviour
{

#if UNITY_EDITOR
    public TextMeshPro myText;

    private void Start()
    {
        CollectTextReference();
    }

    public void CollectTextReference()
    {
        Transform textParent = transform.GetChild(1).GetChild(0);
        myText = textParent.Find("Text - Readable").GetComponent<TextMeshPro>();
    }

    public void SetTextToName()
    {
        Undo.RecordObject(myText, "Changing word in Text component");
        myText.text = this.gameObject.name;
    }

#endif
}
