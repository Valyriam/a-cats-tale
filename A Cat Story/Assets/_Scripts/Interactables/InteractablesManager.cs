using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesManager : MonoBehaviour
{
    public List<PlatformParent> platformParents = new List<PlatformParent>();

    // Start is called before the first frame update
    void Start()
    {
        platformParents.AddRange(FindObjectsOfType<PlatformParent>());
    }

    public void SetActiveInteractables(CatFormData currentCatForm)
    {
        if (currentCatForm.associatedCatState == CharacterMovement.CatStates.defaultCat)
        {
            EnableAndDisableInteractableParticles(PlatformParent.AllPlatformTypes.swipableText, true);
            EnableAndDisableInteractableParticles(PlatformParent.AllPlatformTypes.swipableImage, true);
        }

        else if(currentCatForm.associatedCatState == CharacterMovement.CatStates.wateringCanCat)
        {
            //growable platform type doesn't exist yet
        }

        else if (currentCatForm.associatedCatState == CharacterMovement.CatStates.toasterCat)
        {
            //destructible platform type doesn't exist yet
        }
    }

    void EnableAndDisableInteractableParticles(PlatformParent.AllPlatformTypes activePlatformType, bool isSwipable)
    {
        foreach (PlatformParent platformParent in platformParents)
        {
            //if active platform type
            if (platformParent.platformType == activePlatformType)
                Debug.Log("Insert function to enable visuals");

            //if neutral platform type
            else if (platformParent.platformType == PlatformParent.AllPlatformTypes.neutral || platformParent.platformType == PlatformParent.AllPlatformTypes.image)
                Debug.Log("Insert function to enable visuals");

            //if interactible that isn't active
            else
            {
                if(isSwipable)
                {
                    //disable non swipables only as there is more than one platform type for swipables
                    if (platformParent.platformType != PlatformParent.AllPlatformTypes.swipableText && platformParent.platformType != PlatformParent.AllPlatformTypes.swipableImage)
                        Debug.Log("Insert function to disable all other interactible visuals");
                }
                
                else 
                    Debug.Log("Insert function to enable visuals");
            }
               
        }      
    }
}
