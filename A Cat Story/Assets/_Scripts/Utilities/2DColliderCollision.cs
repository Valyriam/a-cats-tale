using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderCollision2D : MonoBehaviour
{
    [SerializeField] string comparisonTag;
    [SerializeField] private UnityEvent<Collision2D> colliderEntered = new();
    [SerializeField] private UnityEvent<Collision2D> colliderExited = new();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(comparisonTag))
            colliderEntered?.Invoke(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(comparisonTag))
            colliderExited?.Invoke(collision);
    }
}
