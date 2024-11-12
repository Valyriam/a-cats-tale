using echo17.EndlessBook;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] BookData bookData;
    EndlessBook myBook;
    public List<PuzzleDoublePageSpreadComponent> doublePageSpreadComponents;

    private void Awake()
    {
        ClearDoublePageSpreadComponents();
    }

    private void Start()
    {
        myBook = GameObject.FindObjectOfType<EndlessBook>();

        //wait 1 frame then solve all puzzles
        StartCoroutine(SolvePuzzlesOnSceneLoad());      
    }

    //sets all puzzles to incomplete
    public void ClearPuzzles()
    {
        foreach (PuzzleData puzzleData in bookData.AllPuzzles)
        {
            puzzleData.isPuzzleSolved = false;
        }
    }

    //resets what platforms are in the double page spread component lists
    void ClearDoublePageSpreadComponents()
    {
        foreach (PuzzleDoublePageSpreadComponent doublePageSpreadComponent in doublePageSpreadComponents)
        {
            doublePageSpreadComponent.platformParents.Clear();
            doublePageSpreadComponent.doublePageSpreadSolveState = PuzzleDoublePageSpreadComponent.AllDPSpreadSolveStates.unsolved;
        }
    }

    IEnumerator SolvePuzzlesOnSceneLoad()
    {
        int frameCount = 0;

        while (frameCount < 1)
        {
            frameCount++;
            yield return null;
        }

        foreach (PuzzleData puzzleData in bookData.AllPuzzles)
        {
            if (puzzleData.isPuzzleSolved)
                puzzleData.FastSolveDoublePageSpreadComponents();
        }
    }

    //do this on page turn to see if there is an awaited puzzle solve
    public void CheckForAwaitedPuzzleSolve()
    {     
        foreach (PuzzleDoublePageSpreadComponent doublePageSpreadComponent in doublePageSpreadComponents)
        {
            //if player is on the correct page, the puzzle has been solved, and the double page spread is awaiting player - then solve
            if (myBook.CurrentLeftPageNumber == doublePageSpreadComponent.firstPageNumber && doublePageSpreadComponent.myPuzzleData.isPuzzleSolved)
            {
                if (doublePageSpreadComponent.doublePageSpreadSolveState == PuzzleDoublePageSpreadComponent.AllDPSpreadSolveStates.awaitingPlayer)
                    doublePageSpreadComponent.SolveAllPlatformParents();
            }       
        }
    }

    public bool PageEdgeGlowRequired()
    {
        bool pageGlowIsRequired = false;

        foreach (PuzzleDoublePageSpreadComponent doublePageSpreadComponent in doublePageSpreadComponents)
        {
            //if player is on the correct page, the puzzle has been solved, and the double page spread is awaiting player - then solve
            if (myBook.CurrentLeftPageNumber == doublePageSpreadComponent.firstPageNumber && doublePageSpreadComponent.myPuzzleData.isPuzzleSolved)
            {
                pageGlowIsRequired = true; 
            }
        }

        return pageGlowIsRequired;
    }
}
