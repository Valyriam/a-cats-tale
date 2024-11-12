using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablePlayerRotation : MonoBehaviour
{
    [SerializeField] bool enablePlayerRotation = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Character")
        {
            collision.gameObject.GetComponent<CharacterMovement>().rotationEnabled = enablePlayerRotation;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            collision.gameObject.GetComponent<CharacterMovement>().rotationEnabled = false;
        }
    }
}
