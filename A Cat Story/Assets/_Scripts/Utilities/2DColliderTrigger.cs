using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ColliderTrigger2D : MonoBehaviour
{
    [SerializeField] private UnityEvent<Collider2D> triggerEntered = new();
    [SerializeField] private UnityEvent<Collider2D> triggerExit = new();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Character")
            triggerEntered?.Invoke(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Character")
            triggerExit?.Invoke(other);
    }

}
