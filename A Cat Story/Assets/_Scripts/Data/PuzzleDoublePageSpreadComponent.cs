using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Page #-# - DP Spread Data - [Book]", menuName = "Puzzle DP Spread Data")]
public class PuzzleDoublePageSpreadComponent : ScriptableObject
{
    public int firstPageNumber;
    public PuzzleData myPuzzleData;
    public enum AllDPSpreadSolveStates { unsolved, awaitingPlayer, solved }
    public AllDPSpreadSolveStates doublePageSpreadSolveState = AllDPSpreadSolveStates.unsolved;

    [Header("Platform Parents")]
    public List<PlatformParent> platformParents = new List<PlatformParent>();

    public void SolveAllPlatformParents()
    {
        foreach (PlatformParent platformParent in platformParents)
        {
            platformParent.Solve();
        }

        doublePageSpreadSolveState = AllDPSpreadSolveStates.solved;
    }

    public void FastSolveAllPlatformParents()
    {
        foreach (PlatformParent platformParent in platformParents)
        {
            platformParent.FastSolve();
        }

        doublePageSpreadSolveState = AllDPSpreadSolveStates.solved;
    }
}
