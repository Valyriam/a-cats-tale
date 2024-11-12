using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeleporterPoints : MonoBehaviour
{
    [SerializeField] GameObject buttonPrompt;

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            collision.gameObject.GetComponent<CharacterAbilities>().currentInteractObject = this.gameObject;
            buttonPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            collision.gameObject.GetComponent<CharacterAbilities>().currentInteractObject = null;
            buttonPrompt.SetActive(false);
        }
    }
}
