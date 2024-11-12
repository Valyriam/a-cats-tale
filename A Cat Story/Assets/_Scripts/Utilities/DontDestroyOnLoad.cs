using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        if(GameObject.FindGameObjectsWithTag(transform.tag).Length > 1) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
