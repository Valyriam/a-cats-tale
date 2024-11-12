using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SwipableObject : MonoBehaviour
{
    Vector3 secondLocation, thirdLocation;
    Vector3 originalLocation;
    Vector3 temporaryTargetLocation;

    [SerializeField] GameObject buttonPrompt;
    [SerializeField] bool canBeMovedBack, multiMove;
    [SerializeField] float movementSpeed;
    public GameObject reloadLocationObject;
    Vector3 reloadVector3;
    [SerializeField] Sprite incorrectFormSprite;
    [SerializeField] GameObject interactParticleContainer;

    GameObject playerIncorrectFormVisual;

    Transform myUnsolvedPositionParent;

    public enum ScaledStates { readyToMove, moving, moved }
    public ScaledStates state;

    [SerializeField] private UnityEvent onSwipe = new();
    [SerializeField] private UnityEvent onSwipeCompleted = new();

    void Start()
    {
        myUnsolvedPositionParent = transform.parent;
        originalLocation = myUnsolvedPositionParent.position;
        secondLocation = transform.GetChild(0).position;
        thirdLocation = transform.GetChild(1).position;
        state = ScaledStates.readyToMove;
        buttonPrompt = GameObject.Find("Character").transform.GetChild(4).gameObject;

        //reloadVector3 = reloadLocationObject.transform.position    
    }

    private void Update()
    {
        //move my parent
        if (state == ScaledStates.moving)
        {
            //Debug.Log("About to move " + myUnsolvedPositionParent.gameObject.name + " to " + temporaryTargetLocation);
            myUnsolvedPositionParent.position = Vector3.MoveTowards(myUnsolvedPositionParent.position, temporaryTargetLocation, movementSpeed * Time.deltaTime);

            if (myUnsolvedPositionParent.position == originalLocation) state = ScaledStates.readyToMove;

            if (myUnsolvedPositionParent.position == secondLocation)
            {
                state = ScaledStates.moved;
                onSwipeCompleted.Invoke();
            }
        }
    }

    public void MoveObject()
    {
        if (state == ScaledStates.readyToMove)
        {
            temporaryTargetLocation = secondLocation;
            state = ScaledStates.moving;
        }

        if(state == ScaledStates.moved && canBeMovedBack)
        {
            temporaryTargetLocation = originalLocation;
            state = ScaledStates.moving;
        }

        //Multi Move
        if (state == ScaledStates.moved && multiMove)
        {
            state = ScaledStates.readyToMove;
        }
        if (state == ScaledStates.readyToMove && multiMove)
        {
            if (transform.position == secondLocation)
            {
                temporaryTargetLocation = thirdLocation;
                state = ScaledStates.moving;
            }
        }

        interactParticleContainer.SetActive(false);

        onSwipe.Invoke();

        //if (state == ScaledStates.readyToMove)
        //{
        //    Vector3.MoveTowards(transform.position, targetLocation, movementSpeed);
        //    state = ScaledStates.moving;
        //}

            //if(state == ScaledStates.moved && canBeMovedBack)
            //{
            //    Vector3.MoveTowards(transform.position, originalLocation, movementSpeed);
            //    state = ScaledStates.moving;
            //}
    }

    public void MoveToReloadLocation()
    {
        myUnsolvedPositionParent.position = originalLocation;
        interactParticleContainer.SetActive(true);
        state = ScaledStates.readyToMove;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            if (collision.gameObject.GetComponent<CharacterMovement>().catState == CharacterMovement.CatStates.defaultCat)
            {
                if (state == ScaledStates.readyToMove || (state == ScaledStates.moved && canBeMovedBack) || (state == ScaledStates.moved && multiMove))
                {
                    collision.gameObject.GetComponent<CharacterAbilities>().currentSwipableObject = this.gameObject;
                    buttonPrompt.SetActive(true);
                }
            }

            else
            {
                if(!playerIncorrectFormVisual)
                    playerIncorrectFormVisual = collision.gameObject.transform.GetChild(5).gameObject;

                playerIncorrectFormVisual.gameObject.SetActive(true);
                playerIncorrectFormVisual.GetComponent<SpriteRenderer>().sprite = incorrectFormSprite;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            collision.gameObject.GetComponent<CharacterAbilities>().currentSwipableObject = null;
            buttonPrompt.SetActive(false);
            
            if(playerIncorrectFormVisual != null)
                playerIncorrectFormVisual.gameObject.SetActive(false);
        }
    }
}
