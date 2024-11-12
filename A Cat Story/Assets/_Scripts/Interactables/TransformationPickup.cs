using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationPickup : MonoBehaviour
{
    enum AllTransformationTriggers { defaultCat, telescopeCat, witchesHatCat, wateringCanCat, toasterCat }
    [SerializeField] AllTransformationTriggers selectedCatTransformation;

    [SerializeField] CatFormData defaultCatFormData, telescopeCatFormData, witchesHatCatFormDat, wateringCanCatFormDat, toasterCatFormDat;

    [SerializeField] CatFormData catFormOnReset;
    CharacterMovement charMovement;
   
    bool activated;
    public bool isSolved;
    SpriteRenderer spriteRenderer;

    [SerializeField] GameObject particleSys;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        charMovement = GameObject.Find("Character").GetComponent<CharacterMovement>();
    }

    public void SolveTransformationPickup() => isSolved = true;

    public void ResetPickup()
    {
        //if activated but unsolved, set player form to previous form
        if(activated)
            charMovement.SwapToCatForm(catFormOnReset);

        activated = false;
        spriteRenderer.enabled = true;
        particleSys.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Character" && activated == false)
        {
            CharacterMovement characterScript = collision.gameObject.GetComponent<CharacterMovement>();

            switch (selectedCatTransformation)
            {
                case AllTransformationTriggers.defaultCat:
                    characterScript.SwapToCatForm(defaultCatFormData);
                    break;

                case AllTransformationTriggers.telescopeCat:
                    characterScript.SwapToCatForm(telescopeCatFormData);
                    break;

                case AllTransformationTriggers.witchesHatCat:
                    characterScript.SwapToCatForm(witchesHatCatFormDat);
                    break;

                case AllTransformationTriggers.wateringCanCat:
                    characterScript.SwapToCatForm(wateringCanCatFormDat);
                    break;

                case AllTransformationTriggers.toasterCat:
                    characterScript.SwapToCatForm(toasterCatFormDat);
                    break;
            }

            activated = true;
            spriteRenderer.enabled = false;
            particleSys.SetActive(false);

            //Destroy(this.gameObject);
        }
    }
}
