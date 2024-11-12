using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    BookItem currentBook;
    bool teleporting, readyToTeleport;
    GameObject character;

    [SerializeField] bool isTwoWayTeleporter;

    [Header("Point A")]
    [SerializeField] GameObject pointA;
    [SerializeField] int pointAPageNumber;
    [SerializeField] GameObject pointACanvas;
    [SerializeField] GameObject pointAButtonPrompt;

    [Header("Point B")]
    [SerializeField] GameObject pointB;
    [SerializeField] int pointBPageNumber;
    [SerializeField] GameObject pointBCanvas;
    [SerializeField] GameObject pointBButtonPrompt;

    private void Start()
    {
        character = GameObject.Find("Character");
        currentBook = GameObject.FindObjectOfType<BookItem>();
    }

    public void TeleportAtoB() => Teleport(pointB, pointBPageNumber, pointACanvas, pointBCanvas);
    public void TeleportBtoA() => Teleport(pointA, pointAPageNumber, pointBCanvas, pointACanvas);

    void Teleport(GameObject targetPositionObject, int targetPageNumber, GameObject startPositionCanvas, GameObject endPositionCanvas)
    {
        startPositionCanvas.SetActive(true);
        character.SetActive(false);
        currentBook.thisBook.TurnToPage(targetPageNumber, echo17.EndlessBook.EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 0.5f);      

        StartCoroutine(PlaceCharacter(targetPositionObject, endPositionCanvas, startPositionCanvas));
    }

    IEnumerator PlaceCharacter(GameObject targetPositionObject, GameObject endPositionCanvas, GameObject startPositionCanvas)
    {
        while (currentBook.thisBook.IsTurningPages) yield return null;

        endPositionCanvas.SetActive(true);
        character.SetActive(true);
        character.transform.position = targetPositionObject.transform.position;
        startPositionCanvas.SetActive(false);

        yield return null;
    }
}
