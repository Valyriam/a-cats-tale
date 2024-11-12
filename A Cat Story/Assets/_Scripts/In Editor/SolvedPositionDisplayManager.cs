using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class SolvedPositionDisplayManager : MonoBehaviour
{

    public List<PlatformParent> platformParents = new List<PlatformParent>();
    public List<GameObject> solvedPositions = new List<GameObject>();
    public List<GameObject> unsolvedPositions = new List<GameObject>();

#if UNITY_EDITOR
    private void Start()
    {
        UpdateObjectData();
    }

    void ClearAllLists()
    {
        platformParents.Clear();
        solvedPositions.Clear();
        unsolvedPositions.Clear();
    }

    void CollectPositionObjects()
    {
        foreach (PlatformParent platformParent in platformParents)
        {
            solvedPositions.Add(platformParent.solvedPosition);
            unsolvedPositions.Add(platformParent.unsolvedPosition);
        }
    }

    public void UpdateObjectData()
    {
        ClearAllLists();
        platformParents.AddRange(FindObjectsOfType<PlatformParent>());
        CollectPositionObjects();
        CreateSolvedIndicators();
    }

    public void SetSolvedPositionsOnlyVisible()
    {
        SetListObjectsActiveState(solvedPositions, true);
        SetListObjectsActiveState(unsolvedPositions, false);
    }

    public void SetUnsolvedPositionsOnlyVisible()
    {
        SetListObjectsActiveState(solvedPositions, false);
        SetListObjectsActiveState(unsolvedPositions, true);
    }

    public void SetAllPositionsVisible()
    {
        SetListObjectsActiveState(solvedPositions, true);
        SetListObjectsActiveState(unsolvedPositions, true);
    }

    void SetListObjectsActiveState(List<GameObject> list, bool activeState)
    {
        foreach (GameObject listItems in list)
        {
            listItems.SetActive(activeState);
        }
    }

    void CreateSolvedIndicators()
    {
        foreach (GameObject solvedPosition in solvedPositions)
        {
            solvedPosition.GetComponent<SolvedPositionIndicator>().InstantiateReference(); 
        }
    }
#endif
}
