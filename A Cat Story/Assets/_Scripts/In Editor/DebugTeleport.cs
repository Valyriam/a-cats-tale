using echo17.EndlessBook;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugTeleport : MonoBehaviour
{
    [SerializeField] List<GameObject> debugPageReferences = new List<GameObject>();
    EndlessBook currentBook;
    GameObject character;

    [SerializeField] TMP_InputField inputField;

    private void Start()
    {
        GameObject[] allGameObjects = GameObject.FindGameObjectsWithTag("Debug Page Reference");
        debugPageReferences.AddRange(allGameObjects);

        currentBook = GameObject.Find("Playable Book - Children's Book -NEW").GetComponent<EndlessBook>();
        character = GameObject.Find("Character");
    }

    public void GoToPage()
    {
        float targetPageNumber = float.Parse(inputField.text);
        float currentPageNumber = currentBook.CurrentPageNumber;

        //turn page
        currentBook.TurnToPage((int)targetPageNumber, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 0.2f); 

        //find target double page spread number
        float targetDoublePageSpreadNumber = targetPageNumber / 2; 
        targetDoublePageSpreadNumber = Mathf.Ceil(targetDoublePageSpreadNumber);

        //find current double page spread
        float currentDoublePageSpreadNumber = currentPageNumber / 2;
        currentDoublePageSpreadNumber = Mathf.Ceil(currentDoublePageSpreadNumber);

        //use target number to find reference to target debug page reference
        GameObject targetPageReference = debugPageReferences[(int)targetDoublePageSpreadNumber - 1]; 
        DoublePageSegment targetDoublePageSegment = targetPageReference.transform.parent.GetComponent<DoublePageSegment>();

        //use current number to find reference to current debug page reference
        GameObject currentPageReference = debugPageReferences[(int)currentDoublePageSpreadNumber - 1];
        DoublePageSegment currentDoublePageSegment = currentPageReference.transform.parent.GetComponent<DoublePageSegment>();

        //move character to target
        character.transform.position = new Vector3(targetPageReference.transform.position.x, targetPageReference.transform.position.y, character.transform.position.z);

        //activate and deactivate cameras
        targetDoublePageSegment.SetSegmentStates(true);
        currentDoublePageSegment.SetSegmentStates(false);
    }
}
