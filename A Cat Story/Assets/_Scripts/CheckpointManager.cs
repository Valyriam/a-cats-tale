using echo17.EndlessBook;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CheckpointManager : MonoBehaviour
{
    public Vector3 latestCheckpointPosition;
    public int latestCheckpointPageNumber;

    [SerializeField] List<GameObject> debugPageReferences = new List<GameObject>();


    EndlessBook currentBook;
    GameObject character;
    private void Start()
    {
        character = GameObject.Find("Character");
        currentBook = GameObject.Find("Playable Book - Children's Book -NEW").GetComponent<EndlessBook>();

        GameObject[] allGameObjects = GameObject.FindGameObjectsWithTag("Debug Page Reference");
        debugPageReferences.AddRange(allGameObjects);
    }

    public void ResetPlayer()
    {
        if(currentBook.CurrentLeftPageNumber != latestCheckpointPageNumber && currentBook.CurrentRightPageNumber != latestCheckpointPageNumber)
        {
            //currentBook.TurnToPage(latestCheckpointPageNumber, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 1f);
            GoToPage(latestCheckpointPageNumber);
        }

        character.transform.position = latestCheckpointPosition;
    }

    void GoToPage(int pageNumber)
    {
        float targetPageNumber = pageNumber;
        float currentPageNumber = currentBook.CurrentPageNumber;

        //turn page
        currentBook.TurnToPage((int)targetPageNumber, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 1f);

        //find target double page spread number
        float targetDoublePageSpreadNumber = targetPageNumber / 2;
        targetDoublePageSpreadNumber = Mathf.Ceil(targetDoublePageSpreadNumber);

        //find current double page spread number
        float currentDoublePageSpreadNumber = currentPageNumber / 2;
        currentDoublePageSpreadNumber = Mathf.Ceil(currentDoublePageSpreadNumber);

        //use target number to find reference to target debug page reference
        GameObject targetPageReference = debugPageReferences[(int)targetDoublePageSpreadNumber - 1];
        DoublePageSegment targetDoublePageSegment = targetPageReference.transform.parent.GetComponent<DoublePageSegment>();

        //use current number to find reference to current debug page reference
        GameObject currentPageReference = debugPageReferences[(int)currentDoublePageSpreadNumber - 1];
        DoublePageSegment currentDoublePageSegment = currentPageReference.transform.parent.GetComponent<DoublePageSegment>();

        //activate and deactivate cameras
        targetDoublePageSegment.SetSegmentStates(true);
        currentDoublePageSegment.SetSegmentStates(false);
    }

    //ARCHIVED SYSTEM
    /*
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;      
    }

    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        debugPageReferences.Clear();
        GameObject[] allGameObjects = GameObject.FindGameObjectsWithTag("Debug Page Reference");
        debugPageReferences.AddRange(allGameObjects);

        character = GameObject.Find("Character");
        currentBook = GameObject.FindObjectOfType<EndlessBook>();

        if (latestCheckpointPosition != Vector3.zero)
        {
            character.transform.position = latestCheckpointPosition;

            if (latestCheckpointPageNumber != 0)
            {
                currentBook.SetState(EndlessBook.StateEnum.OpenMiddle, 1f);
                currentBook.SetPageNumber(latestCheckpointPageNumber);
                SetCameraStates();
            }
        }

        else currentBook.gameObject.GetComponent<BookItem>().OpenBookF();
    }

    void SetCameraStates()
    {
        float targetPageNumber = latestCheckpointPageNumber;

        //find target double page spread number
        float targetDoublePageSpreadNumber = targetPageNumber / 2;
        targetDoublePageSpreadNumber = Mathf.Ceil(targetDoublePageSpreadNumber);

        //use target number to find reference to target debug page reference
        GameObject targetPageReference = debugPageReferences[(int)targetDoublePageSpreadNumber - 1];
        DoublePageSegment targetDoublePageSegment = targetPageReference.transform.parent.GetComponent<DoublePageSegment>();

        //find reference to title page
        GameObject titlePageReference = debugPageReferences[1];
        DoublePageSegment titlePageSegment = titlePageReference.transform.parent.GetComponent<DoublePageSegment>();

        //activate and deactivate cameras
        Debug.Log("targetDoublePageSegment is " + targetDoublePageSegment.gameObject.name);
        Debug.Log("titlePageSegment is " + titlePageSegment.gameObject.name);
        targetDoublePageSegment.SetSegmentStates(true);
        titlePageSegment.SetSegmentStates(false);
    }
    */
}
