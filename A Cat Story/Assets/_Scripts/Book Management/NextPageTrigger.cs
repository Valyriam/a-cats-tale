using echo17.EndlessBook;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NextPageTrigger : MonoBehaviour
{
    [SerializeField] BookItem myBookItem;
    [SerializeField] Transform playerTransform;
    SFXManager sFXManager;
    [SerializeField] SFXObject pageTurnSFXObject;
    [SerializeField] DoublePageSegment nextDPSpreadSegment;
    [SerializeField] DoublePageSegment myDPSpreadSegment;
    bool isGoingForward = true;

    [SerializeField] private UnityEvent<Collider2D> triggerEntered = new();
    [SerializeField] private UnityEvent<Collider2D> triggerExit = new();

    private void Start()
    {
        playerTransform = GameObject.Find("Character").transform;
        myBookItem = FindObjectOfType<BookItem>();
        sFXManager = FindObjectOfType<SFXManager>();

        //setting up info for turning off cameras
        Transform myDPSpread = transform.parent;
        Transform allDPSpreadContainer = transform.parent.parent;
        int myDPSpreadsChildIindex = myDPSpread.GetSiblingIndex();

        //if not last dp spread, collect next dp spread
        if(myDPSpread.GetSiblingIndex() != allDPSpreadContainer.childCount - 1)
            nextDPSpreadSegment = allDPSpreadContainer.GetChild(myDPSpreadsChildIindex + 1).gameObject.GetComponent<DoublePageSegment>();

        myDPSpreadSegment = myDPSpread.gameObject.GetComponent<DoublePageSegment>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("collided. I touched the " + other.tag);

        if(other.gameObject.tag == "Character") 
            triggerEntered?.Invoke(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("stopped colliding. " + other.tag + " started touching me.");

        if (other.gameObject.tag == "Character") 
            triggerExit?.Invoke(other);
    }

    public void TurnThePage()
    {
       // Debug.Log("isGoingForward is " + isGoingForward + ", character X position is " + playerTransform.position.x + ", and trigger x position is " + transform.position.x);

        if (isGoingForward)
        {
            if (playerTransform.position.x > transform.position.x)
            {
               // Debug.Log("Activating page forward.");
                myBookItem.TriggerPageForward(transform.position);

                if (nextDPSpreadSegment != null) nextDPSpreadSegment.SetSegmentStates(true); //turn on next page's cameras
                myDPSpreadSegment?.SetSegmentStates(false); //turn off my page's cameras
            }
        }

        if(!isGoingForward)
        {
            if(playerTransform.position.x < transform.position.x)
            {
                //Debug.Log("Activating page back.");
                myBookItem.TriggerPageBack(transform.position);

                myDPSpreadSegment.SetSegmentStates(true); //turn on my page's cameras
                if (nextDPSpreadSegment != null) nextDPSpreadSegment.SetSegmentStates(false); //turn off next page's cameras
            }
        }

        //PlayPageTurnSFX();
    }

    public void SetTravelDirection()
    {
        if (playerTransform.position.x < transform.position.x)
            isGoingForward = true;

        else isGoingForward = false;
    }

    public void DoubleCheckPageLocation() => StartCoroutine(myBookItem.CorrectPageLocation(transform));
    public void ContinuePlayerMovement() => StartCoroutine(myBookItem.ContinuePlayerMovement());

    public void PlayPageTurnSFX() => sFXManager.PlaySFX(pageTurnSFXObject);
}
