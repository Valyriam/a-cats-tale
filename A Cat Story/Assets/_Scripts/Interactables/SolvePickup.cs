using echo17.EndlessBook;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SolvePickup : MonoBehaviour
{
    [SerializeField] PuzzleData associatedPuzzleData;
    [SerializeField] bool disableCharacterOnSolve = true;
    [SerializeField] bool activatePageEdgeParticle = true;
    EndlessBook currentBook;
    BookItem bookItem;

    [Header("Actions")]
    [SerializeField] private UnityEvent actionOnSolve = new();

    private void Start()
    {
        currentBook = GameObject.Find("Playable Book - Children's Book -NEW").GetComponent<EndlessBook>(); 
        bookItem = currentBook.gameObject.GetComponent<BookItem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            actionOnSolve.Invoke();

            associatedPuzzleData.SolvePuzzle(currentBook.CurrentLeftPageNumber);

            if (activatePageEdgeParticle)
                bookItem.SetPageEdgeGlowActiveState(true);

            if(disableCharacterOnSolve)
                collision.gameObject.GetComponent<CharacterMovement>().DisableCharacter();

            Destroy(this.gameObject);
        }
    }

    public void DestroyIfSolved()
    {
        if (associatedPuzzleData.isPuzzleSolved) 
            Destroy(this.gameObject);
    }
}
