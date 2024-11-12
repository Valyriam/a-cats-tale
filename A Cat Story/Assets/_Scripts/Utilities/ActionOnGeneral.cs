using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionOnGeneral : MonoBehaviour
{
    [SerializeField] private UnityEvent onAwake = new();
    [SerializeField] private UnityEvent onEnable = new();
    [SerializeField] private UnityEvent onStart = new();
    [SerializeField] private UnityEvent onFirstFrame = new();

    private void Awake()
    {
        onAwake.Invoke();
    }

    private void Start()
    {
        onStart.Invoke();
        StartCoroutine(FirstFrameDelay());
    }

    private void OnEnable()
    {
        onEnable.Invoke();
    }

    IEnumerator FirstFrameDelay()
    {
        int frameTimer = 0;

        while(frameTimer < 1)
        {
            frameTimer++;
            yield return null;
        }

        onFirstFrame.Invoke();
    }
}
