using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.Playables;
using echo17.EndlessBook;

public class DoublePageSegment : MonoBehaviour
{
    public Material referenceMaterial;
    public RenderTexture referenceTexture;
    GameObject myLeftPageSegment;
    GameObject myRightPageSegment;
    [SerializeField] EndlessBook playableBook;

    private void Start()
    {
        if (transform.GetSiblingIndex() != 0)
        {
            myLeftPageSegment = transform.GetChild(0).gameObject;
            myRightPageSegment = transform.GetChild(1).gameObject;
            myLeftPageSegment.SetActive(false);
            myRightPageSegment.SetActive(false);
        }
        
        else
        {
            myRightPageSegment = transform.GetChild(0).gameObject;
        }
    }

    public void SetSegmentStates(bool setState)
    {
        List<GameObject> cameras = new List<GameObject>();

        if (myLeftPageSegment != null)
        {
            myLeftPageSegment.SetActive(setState);
            cameras.Add(myLeftPageSegment.transform.Find("Camera").gameObject);
        }

        if (myRightPageSegment != null)
        {
            myRightPageSegment.SetActive(setState);
            cameras.Add(myRightPageSegment.transform.Find("Camera").gameObject);
        }

        //activate/deactivate camera component object
        foreach (GameObject cam in cameras)
        {
            cam.SetActive(setState);
        }
    }

#if UNITY_EDITOR

    //public int myLeftPageNumber;

    public Object doublePageSpreadPrefab;

    public void CreateNewDoublePageSpread()
    {
        //set playable book reference
        playableBook = GameObject.Find("Playable Book - Children's Book -NEW").GetComponent<EndlessBook>();

        //instantiate double page prefab
        GameObject newPages = PrefabUtility.InstantiatePrefab(doublePageSpreadPrefab, transform.parent) as GameObject;
        newPages.transform.position = new Vector3(transform.position.x + 62.5f, transform.position.y, transform.position.z);
        newPages.GetComponent<DoublePageSegment>().doublePageSpreadPrefab = doublePageSpreadPrefab;

        //set page number values
        int myLeftPageNumber = int.Parse(transform.GetChild(0).gameObject.transform.GetChild(4).GetComponent<TextMeshPro>().text.Substring(8));
        string leftPageNumberText = "<wiggle>" + (myLeftPageNumber + 2);
        string rightPageNumberText = "<wiggle>" + (myLeftPageNumber + 3);
        newPages.transform.GetChild(0).gameObject.transform.GetChild(4).GetComponent<TextMeshPro>().text = leftPageNumberText;
        newPages.transform.GetChild(1).GetChild(4).GetComponent<TextMeshPro>().text = rightPageNumberText;

        //Set camera references, create materials, and add materials to book
        Camera leftPageCam = newPages.transform.GetChild(0).GetComponentInChildren<Camera>();
        Camera rightPageCam = newPages.transform.GetChild(1).GetComponentInChildren<Camera>();
        CreateAndAssignNewBookMaterial(leftPageCam, myLeftPageNumber + 2);
        CreateAndAssignNewBookMaterial(rightPageCam, myLeftPageNumber + 3);

        //create associated DP Spread Segment for puzzles
        CreateDPSpreadSegment(myLeftPageNumber + 2);

        //select new double page spread object
        Selection.activeObject = newPages;

        AssetDatabase.Refresh();
    }

    void CreateAndAssignNewBookMaterial(Camera targetCamera, int pageNumber)
    {
        Material newMaterial = new Material(referenceMaterial);
        RenderTexture newRenderTexture = new RenderTexture(referenceTexture);

        //saving material and texture
        AssetDatabase.CreateAsset(newMaterial, "Assets/_Core/Books/Children's/Children's Book Mats & Render Textures/Child_Page " + pageNumber + ".mat");
        AssetDatabase.CreateAsset(newRenderTexture, "Assets/_Core/Books/Children's/Children's Book Mats & Render Textures/Child_Page " + pageNumber + ".renderTexture");

        //setting material render texture
        newMaterial.SetTexture("_BaseMap", newRenderTexture);

        //setting camera to use render texture    
        targetCamera.targetTexture = newRenderTexture;   

        AddPageToPlayableBook(newMaterial);
    }

    void AddPageToPlayableBook(Material bookMaterial)
    {
        playableBook.AddPageData(bookMaterial);
        PrefabUtility.ApplyPrefabInstance(playableBook.gameObject, InteractionMode.UserAction);
    }


    void CreateDPSpreadSegment(int leftPageNumber)
    {
        PuzzleManager puzzleManager = GameObject.FindObjectOfType<PuzzleManager>();

        PuzzleDoublePageSpreadComponent dpSpreadComponent = new PuzzleDoublePageSpreadComponent();

        dpSpreadComponent.firstPageNumber = leftPageNumber;
        dpSpreadComponent.doublePageSpreadSolveState = PuzzleDoublePageSpreadComponent.AllDPSpreadSolveStates.unsolved;

        puzzleManager.doublePageSpreadComponents.Add(dpSpreadComponent);

        AssetDatabase.CreateAsset(dpSpreadComponent, "Assets/_Data Values/Books and Puzzles/Children's Book/Puzzle Data - Children's Book/Unused DP Spreads/Page " + leftPageNumber + "-" + (leftPageNumber + 1) + " - DP Spread Data - Children's Book.asset");

        //apply changes to prefab
        PrefabUtility.ApplyPrefabInstance(puzzleManager.gameObject, InteractionMode.UserAction);
    }

#endif
}
