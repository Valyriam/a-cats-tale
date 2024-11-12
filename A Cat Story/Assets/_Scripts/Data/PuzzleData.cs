using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Book - Puzzle Data #", menuName = "Puzzle Data")]
public class PuzzleData : ScriptableObject
{
    public string puzzleName;
    public BookData book;
    public bool isPuzzleSolved;

    [Header("Double Page Spread Components")]
    public List<PuzzleDoublePageSpreadComponent> puzzleDoublePageSpreadComponents = new List<PuzzleDoublePageSpreadComponent>();

    public void SolvePuzzle(int currentLeftPageNumber)
    {
        isPuzzleSolved = true;
        SolveAllDoublePageSpreadComponents(currentLeftPageNumber);
    }

    void SolveAllDoublePageSpreadComponents(int currentLeftPageNumber)
    {
        foreach (PuzzleDoublePageSpreadComponent doublePageSpreadComponent in puzzleDoublePageSpreadComponents)
        {
            if (doublePageSpreadComponent.firstPageNumber == currentLeftPageNumber)
                doublePageSpreadComponent.SolveAllPlatformParents();

            else doublePageSpreadComponent.doublePageSpreadSolveState = PuzzleDoublePageSpreadComponent.AllDPSpreadSolveStates.awaitingPlayer;
        }
    }

    public void FastSolveDoublePageSpreadComponents()
    {
        foreach (PuzzleDoublePageSpreadComponent doublePageSpreadComponent in puzzleDoublePageSpreadComponents)
        {
            doublePageSpreadComponent.FastSolveAllPlatformParents();
        }
    }
}
