using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;
using TMPro;
using Febucci.UI.Effects;
using UnityEngine.Events;
using Unity.VisualScripting;
using UnityEngine.UI;
using Febucci.UI;

public class PlatformParent : MonoBehaviour
{
    [Header("References")]
    public GameObject platform;
    public GameObject solvedPosition, unsolvedPosition;
    public GameObject platformCollider;
    public GameObject outlineContainer;
    [SerializeField] TextMeshPro platformTextReadable;
    [SerializeField] Material solvedSpriteMaterial;
    [SerializeField] Material branchTopMaterial;
    FontManager fontManager;
    SpriteRenderer branchTopRenderer;

    public enum AllPlatformTypes { bouncy, bulletpoint, bulletpointImage, conveyor, gravity, moving, neutral, slippery, image, swipableText, swipableImage, hazard, pickup}

    [Header("Setup")]
    public AllPlatformTypes platformType;
    public PuzzleDoublePageSpreadComponent associatedPuzzleDPComponent;
    public bool removeOutlineOnSolve = true;
    public bool isSolved;
    public bool hasBranchTop;

    [Header("Gravity")]
    public Vector3 unsolvedOriginalPosition;
    public Quaternion unsolvedOriginalRotation;

    [Header("Actions")]
    [SerializeField] private UnityEvent onSolve = new();

    private void Start()
    {
        unsolvedOriginalPosition = unsolvedPosition.transform.position;
        unsolvedOriginalRotation = unsolvedPosition.transform.rotation;

        fontManager = GameObject.FindObjectOfType<FontManager>();

        if(associatedPuzzleDPComponent != null)
            associatedPuzzleDPComponent.platformParents.Add(GetComponent<PlatformParent>());

        if (platformType == AllPlatformTypes.swipableImage)
            platformCollider = platform.transform.GetChild(2).GetChild(0).gameObject;

        if (hasBranchTop)
            branchTopRenderer = platform.transform.GetChild(0).GetComponent<SpriteRenderer>();

        /*if(IsTextPlatform()) //THIS IS GROSS AND NEEDS TO BE A MANUAL REFERENCE
        {
            if(platformType == AllPlatformTypes.swipableText)
                platformCollider = platform.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(1).gameObject;

            else
                platformCollider = platform.transform.GetChild(0).gameObject;

            outlineContainer = platformCollider.transform.GetChild(1).gameObject;
        }*/

        if (platformTextReadable != null) 
            SetTextTags();
    }

    public void Solve() => StartCoroutine(LerpPlatform());

    public void FastSolve()
    {
        platform.transform.position = solvedPosition.transform.position;
        platform.transform.rotation = solvedPosition.transform.rotation;
        CheckOutlineDisable();
        onSolve.Invoke();
        isSolved = true;  
    }

    IEnumerator LerpPlatform()
    {
        //disable platform collider
        if(IsTextPlatform())
            platformCollider.GetComponent<BoxCollider2D>().enabled = false;

        //PickupDisableCheck();

        //move platform
        CharacterMovement characterMovement = GameObject.Find("Character").GetComponent<CharacterMovement>();
        float lerpTime = 2;
        float currentLerpTime = 0;
        float interpFactor;

        while (currentLerpTime < lerpTime)
        {
            currentLerpTime += Time.deltaTime;

            interpFactor = currentLerpTime / lerpTime;
            platform.transform.position = Vector3.Lerp(unsolvedPosition.transform.position, solvedPosition.transform.position, interpFactor);
            platform.transform.rotation = Quaternion.Lerp(unsolvedPosition.transform.rotation, solvedPosition.transform.rotation, interpFactor);
            yield return null;
        }

        platform.transform.position = solvedPosition.transform.position;
        platform.transform.rotation = solvedPosition.transform.rotation;
        isSolved = true;

        //check for disabling outline and animations
        if (IsTextPlatform())
            CheckOutlineDisable();

        //else check if image should receive new material
        else if (removeOutlineOnSolve)
        {
            SetSolvedSpriteMaterials();
            DisableImageCollider();
        }

        //reenable character
        if (characterMovement.characterDisabled)
            characterMovement.ReenableCharacter();

        //trigger on solve effects
        onSolve.Invoke();
    }

    void SetTextTags()
    {
        string existingReadableText = platformTextReadable.text;

        switch (platformType)
        {
            case AllPlatformTypes.bouncy:
                if (platformTextReadable != null)
                    platformTextReadable.text = "<bounce>" + existingReadableText;
                break;
                
            case AllPlatformTypes.bulletpoint:
                if (platformTextReadable != null)
                    platformTextReadable.text = "<incr>" + existingReadableText;
                break;

            case AllPlatformTypes.conveyor:
                if (platformTextReadable != null)
                    platformTextReadable.text = "<wave>" + existingReadableText;
                break;

            case AllPlatformTypes.gravity:
                if (platformTextReadable != null)
                    platformTextReadable.text = "<dangle>" + existingReadableText;
                break;

            case AllPlatformTypes.swipableText:
                if (platformTextReadable != null)
                    platformTextReadable.text = "<slide>" + existingReadableText;
                break;

            case AllPlatformTypes.neutral:
                if (platformTextReadable != null)
                    platformTextReadable.text = "<wiggle>" + existingReadableText;
                break;

            case AllPlatformTypes.slippery:
                if (platformTextReadable != null)
                    platformTextReadable.text = "<swing>" + existingReadableText;
                break;
        }
    }

    void DisableImageCollider()
    {
        platformCollider.GetComponent<Collider2D>().enabled = false;

        ParticleSystem particleSystem = platformCollider.GetComponent<ParticleSystem>();

        if (particleSystem != null)
            particleSystem.Stop();
    }

    void PickupDisableCheck()
    {
        //disable rb if pickup, disable trigger
        if (platformType == AllPlatformTypes.pickup)
        {
            platformCollider.GetComponent<Rigidbody2D>().simulated = false;
            unsolvedPosition.GetComponent<Collider2D>().enabled = false;
        }
    }

    void CheckOutlineDisable()
    {
        if (removeOutlineOnSolve)
        {
            outlineContainer.SetActive(false);

            Collider2D collider = platformCollider.GetComponent<Collider2D>();
            collider.enabled = false;

            //set solved font
            TMP_FontAsset solvedFont = fontManager.FindCorrectSolvedFont(this);
            SetPlatformFont(solvedFont);

            platformTextReadable.gameObject.GetComponent<TextAnimator_TMP>().enabled = false;          
        }

        else
        {
            outlineContainer.SetActive(true);
            platformCollider.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    bool IsTextPlatform()
    {
        if (platformType == AllPlatformTypes.image || platformType == AllPlatformTypes.pickup || platformType == AllPlatformTypes.swipableImage || platformType == AllPlatformTypes.bulletpointImage || platformType == AllPlatformTypes.hazard)
            return false;

        else
            return true;

        /*if (platformType != AllPlatformTypes.image && platformType != AllPlatformTypes.swipableImage && platformType != AllPlatformTypes.hazard)
            return false;

        else 
            return true;*/
    }

    public void SetPlatformFont(TMP_FontAsset newFont)
    {
        if(platformTextReadable != null)
            platformTextReadable.font = newFont;
    }

    void SetSolvedSpriteMaterials()
    {
        List<SpriteRenderer> platformSpriteRenderers = new List<SpriteRenderer>();

        if(platform.GetComponent<SpriteRenderer>() != null)
            platformSpriteRenderers.Add(platform.GetComponent<SpriteRenderer>()); //add parent SR

        platformSpriteRenderers.AddRange(platform.GetComponentsInChildren<SpriteRenderer>()); //add SRs in children

        foreach (SpriteRenderer sr in platformSpriteRenderers)
        {
            sr.material = solvedSpriteMaterial;
        }

        if(hasBranchTop)
            branchTopRenderer.material = branchTopMaterial;
    }

    public void ResetUnsolvedPosition()
    {
        unsolvedPosition.transform.position = unsolvedOriginalPosition;
        unsolvedPosition.transform.rotation = unsolvedOriginalRotation;
    }

}
