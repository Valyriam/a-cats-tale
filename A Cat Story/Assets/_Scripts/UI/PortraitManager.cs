using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitManager : MonoBehaviour
{
    CharacterMovement character;
    [SerializeField] Image portraitImage;

    [SerializeField] GameObject pickupItemContainer;
    [SerializeField] Image pickupItemImage;

    private void Awake()
    {
        character = GameObject.Find("Character").GetComponent<CharacterMovement>();
    }

    public void ChangePortrait(CatFormData catFormData)
    {
        portraitImage.sprite = catFormData.characterSprite;
    }

    #region Pickup UI

    public void ActivatePickupUI(PickupObjectData pickupObjectData)
    {
        pickupItemContainer.SetActive(true);
        pickupItemImage.sprite = pickupObjectData.objectSprite;
    }

    public void DeactivatePickupUI()
    {
        pickupItemImage.sprite = null;
        pickupItemContainer.SetActive(false);
    }

    #endregion
}
