using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PickupableObject : MonoBehaviour
{
    public PickupObjectData myPickupObjectData;
    [SerializeField] GameObject buttonPrompt;

    [SerializeField] Sprite incorrectFormSprite;

    GameObject playerIncorrectFormVisual;

    [SerializeField] UnityEvent onPickup = new();

    private void Start()
    {
        buttonPrompt = GameObject.Find("Character").transform.GetChild(4).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Character")
        {
            CharacterAbilities characterAbilities = collision.gameObject.GetComponent<CharacterAbilities>();

            //if there isn't currently a pickup object, do this stuff
            if (characterAbilities.currentPickupObject == null)
            {
                if (collision.gameObject.GetComponent<CharacterMovement>().catState == CharacterMovement.CatStates.witchesHatCat)
                {
                    characterAbilities.currentPickupObject = this.gameObject;
                    buttonPrompt.SetActive(true);
                }

                else
                {
                    if (!playerIncorrectFormVisual)
                        playerIncorrectFormVisual = collision.gameObject.transform.GetChild(5).gameObject;

                    playerIncorrectFormVisual.gameObject.SetActive(true);
                    playerIncorrectFormVisual.GetComponent<SpriteRenderer>().sprite = incorrectFormSprite;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            CharacterAbilities characterAbilities = collision.gameObject.GetComponent<CharacterAbilities>();

            //if you leave the collider and the current pickup object data is the same as mine, remove it all
            if (characterAbilities.currentPickupObject.GetComponent<PickupableObject>().myPickupObjectData == myPickupObjectData)
            {

                characterAbilities.currentPickupObject = null;
                buttonPrompt.SetActive(false);

                if (playerIncorrectFormVisual != null)
                    playerIncorrectFormVisual.gameObject.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        onPickup.Invoke();
    }

    public void ResetFromDeathCollider()
    {
        GetComponentInParent<PlatformParent>().ResetUnsolvedPosition();
    }
}
