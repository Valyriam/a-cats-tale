using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DebugUIManager : MonoBehaviour
{
    [SerializeField] GameObject inputField;
    GameObject debugUI;
    EventSystem es;
    private void Start()
    {
        debugUI = transform.GetChild(0).gameObject;
        es = GameObject.FindObjectOfType<EventSystem>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            debugUI.SetActive(!debugUI.activeSelf);

            if (debugUI.activeSelf == true)
                es.SetSelectedGameObject(inputField);
        }
    }
}
