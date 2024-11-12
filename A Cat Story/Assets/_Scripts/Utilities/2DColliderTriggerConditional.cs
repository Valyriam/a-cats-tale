using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Conditional2DColliderTrigger : MonoBehaviour
{
    [SerializeField] string comparisonTag;
    [SerializeField] private UnityEvent<Collider2D> triggerEntered = new();
    [SerializeField] private UnityEvent<Collider2D> triggerExit = new();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(comparisonTag))
            triggerEntered?.Invoke(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(comparisonTag))
            triggerExit?.Invoke(other);
    }

}
