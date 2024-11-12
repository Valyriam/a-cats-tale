using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractActionController : MonoBehaviour
{
    [SerializeField] private UnityEvent interactAction = new();

    public void Interact() => interactAction?.Invoke();
}
