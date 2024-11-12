using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using echo17.EndlessBook;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;

public class BookItem : MonoBehaviour
{
    public EndlessBook thisBook;

    //player variables
    CharacterMovement playerScript;
    [SerializeField] Transform playerTransform;

    //page turning management
    [SerializeField] bool lastPageTurnWasForwards;
    [SerializeField] Transform pageTurningPosition;
    [SerializeField] GameObject solvedPageEdgeParticles;
    Vector3 originalPosition;

    //events
    [SerializeField] private UnityEvent onPageTurn = new();
    [SerializeField] private UnityEvent afterPageTurn = new();

    private void Awake()
    {
        thisBook = gameObject.GetComponent<EndlessBook>();
        playerTransform = GameObject.Find("Character").transform;
        playerScript = playerTransform.gameObject.GetComponent<CharacterMovement>();
    }

    #region Core Functionality
    public void PageRight()
    {
        if (thisBook.CurrentState == EndlessBook.StateEnum.ClosedFront) OpenBookF(); //open book
        else if (thisBook.CurrentState == EndlessBook.StateEnum.ClosedBack) CloseBookF(); //closed back to closed front
        else if (thisBook.CurrentState == EndlessBook.StateEnum.OpenFront) thisBook.TurnToPage(1, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 1f); //open front to page 1
        else if (thisBook.CurrentState == EndlessBook.StateEnum.OpenBack) CloseBookB(); //open back to close back
        else if (thisBook.CurrentPageNumber == thisBook.LastPageNumber) OpenBookB(); //open book back if last page 
        else thisBook.TurnForward(1f); //else open next page

        onPageTurn.Invoke();
    }

    public void PageLeft()
    {
        if (thisBook.CurrentState == EndlessBook.StateEnum.ClosedFront) CloseBookB(); //close book
        else if (thisBook.CurrentState == EndlessBook.StateEnum.ClosedBack) OpenBookB(); //closed back to open back
        else if (thisBook.CurrentState == EndlessBook.StateEnum.OpenBack) thisBook.TurnToPage(thisBook.LastPageNumber, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 1f); //open back to last page
        else if (thisBook.CurrentState == EndlessBook.StateEnum.OpenFront) CloseBookF(); //open front to close front
        else if (thisBook.CurrentPageNumber == 1) OpenBookF(); //open book front if first page 
        else thisBook.TurnBackward(1f); //else open previous page

        onPageTurn.Invoke();
    }

    public void CloseBookF()
    {
        if (thisBook.CurrentState != EndlessBook.StateEnum.ClosedFront)
            thisBook.SetState(EndlessBook.StateEnum.ClosedFront);
    }

    public void CloseBookB()
    {
        if (thisBook.CurrentState != EndlessBook.StateEnum.ClosedBack)
            thisBook.SetState(EndlessBook.StateEnum.ClosedBack);
    }

    public void OpenBookF()
    {
        thisBook.SetState(EndlessBook.StateEnum.OpenFront);
    }

    public void OpenBookB()
    {
        thisBook.SetState(EndlessBook.StateEnum.OpenBack);
    }

    public void OpenBookMid()
    {
        thisBook.SetState(EndlessBook.StateEnum.OpenMiddle);
    }
    public void DelayedBookOpenMid(float waitTime) => StartCoroutine(OpenBookMidAfterTime(waitTime));

    public void DelayedBookOpenF(float waitTime) => StartCoroutine(OpenBookFrontAfterTime(waitTime));

    public void DelayedPageTurn(float waitTime, int pageNumber, float timePerPage) => StartCoroutine(TurnToPageAfterTime(waitTime, pageNumber, timePerPage));

    IEnumerator OpenBookFrontAfterTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        OpenBookF();
    }

    IEnumerator OpenBookMidAfterTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        OpenBookMid();
    }

    IEnumerator TurnToPageAfterTime(float waitTime, int pageNumber, float timePerPage)
    {
        yield return new WaitForSeconds(waitTime);
        thisBook.TurnToPage(pageNumber, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, timePerPage);
    }
    #endregion

    #region Trigger Functionality

    public void TriggerPageForward(Vector2 triggerPosition)
    {
        if (!thisBook.IsTurningPages && !thisBook.isChangingState)
        {
            lastPageTurnWasForwards = true;
            PageRight();
        }
    }

    public void TriggerPageBack(Vector2 triggerPosition)
    {
        if (!thisBook.IsTurningPages && !thisBook.isChangingState && thisBook.CurrentState != EndlessBook.StateEnum.OpenFront)
        {
            PageLeft();
            lastPageTurnWasForwards = false;
        }
    }


    //ARCHIVED

    public IEnumerator CorrectPageLocation(Transform triggerTransform)
    {
        while (thisBook.IsTurningPages || thisBook.isChangingState)
        {
            yield return null;
        }

        //if animation is complete, player is left of trigger, and we just turned page forward, go back
        if (playerTransform.position.x < triggerTransform.position.x && lastPageTurnWasForwards)
        {
            TriggerPageBack(triggerTransform.position);
        }

        //if animation is complete, player is right of trigger, and we just turned page back, go forward
        else if (playerTransform.position.x > triggerTransform.position.x && !lastPageTurnWasForwards)
        {
            TriggerPageForward(triggerTransform.position);
        }
    }

    public IEnumerator ContinuePlayerMovement()
    {
        while (thisBook.IsTurningPages || thisBook.isChangingState)
        {
            yield return null;
        }

        if (!thisBook.IsTurningPages && !thisBook.isChangingState)
        {
            playerScript.ReenableCharacter();
        }
    }

    #endregion

    #region Visual Book Management
    public void OpeningBookFlip()
    {
        if (GameObject.FindObjectOfType<CheckpointManager>().latestCheckpointPageNumber == 0)
            OpenBookF();
    }

    public void MoveBookForPageTurn() => StartCoroutine(LerpToPageTurnPosition());
    public void AwaitAfterPageTurnInvoke() => StartCoroutine(AwaitAfterPageTurn());

    IEnumerator AwaitAfterPageTurn()
    {
        while (thisBook.IsTurningPages || thisBook.isChangingState)
        {
            yield return null;
        }

        if (!thisBook.IsTurningPages && !thisBook.isChangingState)
        {
            if(thisBook.CurrentState != EndlessBook.StateEnum.ClosedFront)
                afterPageTurn.Invoke();
        } 
    }

    IEnumerator LerpToPageTurnPosition()
    {
        float lerpDuration = 0.5f; 

        float timeElapsed = 0f;

        while (timeElapsed < lerpDuration)
        {
            timeElapsed += Time.deltaTime;

            // Calculate the interpolation factor (t) between 0 and 1
            float t = Mathf.Clamp01(timeElapsed / lerpDuration);

            // Lerp the object's position between the start and end positions
            transform.position = Vector3.Lerp(originalPosition, pageTurningPosition.position, t);

            // Yielding null will make the coroutine wait until the next frame
            yield return null;
        }

        transform.position = pageTurningPosition.position;

        StartCoroutine(ReturnToOriginalPosition());
    }

    IEnumerator ReturnToOriginalPosition()
    {
        float lerpDuration = 0.5f;

        float timeElapsed = 0f;

        while (timeElapsed < lerpDuration)
        {
            timeElapsed += Time.deltaTime;

            // Calculate the interpolation factor (t) between 0 and 1
            float t = Mathf.Clamp01(timeElapsed / lerpDuration);

            // Lerp the object's position between the start and end positions
            transform.position = Vector3.Lerp(pageTurningPosition.position, originalPosition, t);

            // Yielding null will make the coroutine wait until the next frame
            yield return null;
        }

        transform.position = originalPosition;
    }

    public void DisableAnimator()
    {
        GetComponent<Animator>().enabled = false;
        originalPosition = transform.position;
    }
    
    public void CheckForPageEdgeGlow(PuzzleManager puzzleManager)
    {
        if (puzzleManager.PageEdgeGlowRequired())
            SetPageEdgeGlowActiveState(true);
    }

    public void SetPageEdgeGlowActiveState(bool state)
    {
        solvedPageEdgeParticles.SetActive(state);
    }

    #endregion

    #region Character Management
    public void DisableCharacter() => playerScript.DisableCharacter();
    
    public void ReenableCharacter() => playerScript.ReenableCharacter();
    #endregion
}
