using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterAbilities : MonoBehaviour
{
    CharacterMovement charMovement;
    //PortraitManager portraitManager;
    Animator playerVisualAnimator;

    #region Toast Variables
    enum toastinessLevel { rawBread, lightlyToasted, perfectlyToasted, burntToast }

    [Header("Toast Variables")]
    [SerializeField] toastinessLevel toastChargeLevel;
    bool chargingToast;
    bool isRightFacing;
    float toastChargeTimer;

    [SerializeField] GameObject toastFiringPointA, toastFiringPointB;
    TrajectoryRenderer toastPointATrajectory, toastPointBTrajectory;
    [SerializeField] GameObject rawBreadPrefab, lightlyToastedPrefab, perfectlyToastedPrefab, burntToastPrefab;
    #endregion

    #region Pickup Variables
    [Header("Pickup Variables")]
    [SerializeField] Transform dropItemPositionR;
    [SerializeField] Transform dropItemPositionL;
    [SerializeField] GameObject heldItemVisual;
    public PickupObjectData heldObjectData;
    public GameObject currentPickupObject;
    public GameObject pickupObjectParent;
    #endregion

    #region Seed Variables
    [Header("Seed Variables")]
    public GameObject currentSeed;
    public float offSet;
    #endregion

    #region Scale Variables
    [Header("Scale Variables")]
    public GameObject currentScalableObject;
    #endregion

    #region Swipe Variables
    [Header("Swipe Variables")]
    public GameObject currentSwipableObject;
    #endregion

    #region Interact Variables
    [Header("Interact Variables")]
    public GameObject currentInteractObject;
    #endregion

    #region Events
    [Header("Events")]
    public UnityEvent onSwipe = new();
    public UnityEvent onWateringSeed = new();
    public UnityEvent onChargingToast = new();
    public UnityEvent onFiringToast = new();
    public UnityEvent onScaleUp = new();
    public UnityEvent onScaleDown = new();
    public UnityEvent onPickup = new();
    public UnityEvent onDrop = new();
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        charMovement = GetComponent<CharacterMovement>();
        //portraitManager = GameObject.Find("Portrait Manager").GetComponent<PortraitManager>();
        toastPointATrajectory = toastFiringPointA.transform.GetChild(0).GetComponent<TrajectoryRenderer>();
        toastPointBTrajectory = toastFiringPointB.transform.GetChild(0).GetComponent<TrajectoryRenderer>();
        playerVisualAnimator = transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //toast charging
        if (!chargingToast)
        {
            toastPointATrajectory.gameObject.SetActive(false);
            toastPointBTrajectory.gameObject.SetActive(false);
            return;
        }
            

        else
        {
            Debug.Log("Chargin toast...");
            toastChargeTimer += Time.deltaTime;
            charMovement.horizontal = 0;

            if (toastChargeTimer < 1)
            {
                toastChargeLevel = toastinessLevel.rawBread;
                DrawFiringLines(rawBreadPrefab.GetComponent<ToastProjectile>().explosionForce);
            }

            if (toastChargeTimer >= 1)
            {
                toastChargeLevel = toastinessLevel.lightlyToasted;
                DrawFiringLines(lightlyToastedPrefab.GetComponent<ToastProjectile>().explosionForce);
            }

            if (toastChargeTimer >= 2)
            {
                toastChargeLevel = toastinessLevel.perfectlyToasted;
                DrawFiringLines(perfectlyToastedPrefab.GetComponent<ToastProjectile>().explosionForce);
            }

            if (toastChargeTimer >= 3f)
            {
                toastChargeLevel = toastinessLevel.burntToast;
                DrawFiringLines(burntToastPrefab.GetComponent<ToastProjectile>().explosionForce);
            }
        }
    }

    #region Toast
    public void ActivateToastAbility(InputAction.CallbackContext context, bool characterFacingForward)
    {
        playerVisualAnimator.SetBool("isAbility", true);
        isRightFacing = characterFacingForward;
        ChargeToast(context);
        //charMovement.horizontal = 0;
    }

    void ChargeToast(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            chargingToast = true;
            onChargingToast.Invoke();
        }

        if (context.canceled)
        {
            FireToast();
            onFiringToast.Invoke();
        }
    }

    void FireToast()
    {
        playerVisualAnimator.SetBool("isAbility", false);
        if (toastChargeLevel == toastinessLevel.rawBread) TriggerFiringPoints(rawBreadPrefab);
        else if (toastChargeLevel == toastinessLevel.lightlyToasted) TriggerFiringPoints(lightlyToastedPrefab);
        else if (toastChargeLevel == toastinessLevel.perfectlyToasted) TriggerFiringPoints(perfectlyToastedPrefab);
        else if (toastChargeLevel == toastinessLevel.burntToast) TriggerFiringPoints(burntToastPrefab);

        chargingToast = false;
        toastChargeTimer = 0;
    }

    void TriggerFiringPoints(GameObject prefab)
    {
        GameObject toastA = Instantiate(prefab, toastFiringPointA.transform.position, toastFiringPointA.transform.rotation);
        GameObject toastB = Instantiate(prefab, toastFiringPointB.transform.position, toastFiringPointB.transform.rotation);

        toastA.transform.position = new Vector3(toastA.transform.position.x, toastA.transform.position.y, 20);
        toastB.transform.position = new Vector3(toastB.transform.position.x, toastB.transform.position.y, 20);

        toastA.GetComponent<ToastProjectile>().MoveToast(isRightFacing);
        toastB.GetComponent<ToastProjectile>().MoveToast(isRightFacing);

        

    }

    void DrawFiringLines(float explosionForce)
    {
        toastPointATrajectory.gameObject.SetActive(true);
        toastPointBTrajectory.gameObject.SetActive(true);

        toastPointATrajectory.throwForce = explosionForce;
        toastPointBTrajectory.throwForce = explosionForce;

        if (isRightFacing == true)
        {
            toastPointATrajectory.throwAngle = 45f;
            toastPointBTrajectory.throwAngle = 45f;
            toastPointATrajectory.DrawTrajectory();
            toastPointBTrajectory.DrawTrajectory();
        }
        else if (isRightFacing == false)
        {
            toastPointATrajectory.throwAngle = 135f;
            toastPointBTrajectory.throwAngle = 135f;
            toastPointATrajectory.DrawTrajectory();
            toastPointBTrajectory.DrawTrajectory();
        }
    }
    #endregion

    #region Pickup

    public void ActivatePickupAbility()
    {
        if (currentPickupObject != null)
        {
            pickupObjectParent = currentPickupObject.transform.parent.gameObject;
            PickupItem(currentPickupObject.GetComponent<PickupableObject>().myPickupObjectData);
            onPickup.Invoke();
        }
    }

    void PickupItem(PickupObjectData objectData)
    {
        playerVisualAnimator.SetBool("isAbility", true);
        //portraitManager.ActivatePickupUI(objectData);

        heldObjectData = objectData;
        heldItemVisual.GetComponent<SpriteRenderer>().sprite = objectData.objectSprite;
        heldItemVisual.SetActive(true);

        Destroy(currentPickupObject.gameObject);
        currentPickupObject = null;
    }

    public void DropItem()
    {       
        GameObject droppedItem = Instantiate(heldObjectData.objectPrefab, Vector3.zero, Quaternion.identity, pickupObjectParent.transform);

        //set position
        if (charMovement.isRightFacing)
            droppedItem.transform.position = dropItemPositionR.position;

        else
            droppedItem.transform.position = dropItemPositionL.position;

        PlatformParent platformParent = pickupObjectParent.GetComponent<PlatformParent>();

        //set dropped item as unsolved position
        platformParent.unsolvedPosition = droppedItem;
        platformParent.platform = droppedItem;
        platformParent.platformCollider = droppedItem.transform.GetChild(0).gameObject;

        ResetPickupReferences();

        onDrop.Invoke();
    }

    void ResetPickupReferences()
    {
        heldObjectData = null;
        pickupObjectParent = null;
        heldItemVisual.SetActive(false);
    }

    public void ResetHeldPickupObject()
    {
        //portraitManager.DeactivatePickupUI();
        
        GameObject droppedItem = Instantiate(heldObjectData.objectPrefab, Vector3.zero, Quaternion.identity, pickupObjectParent.transform);
        PlatformParent platformParent = pickupObjectParent.GetComponent<PlatformParent>();

        //set dropped item as unsolved position
        platformParent.unsolvedPosition = droppedItem;
        platformParent.platform = droppedItem;
        platformParent.platformCollider = droppedItem.transform.GetChild(0).gameObject;

        platformParent.ResetUnsolvedPosition();

        ResetPickupReferences();
    }

    #endregion

    #region Seed

    public void ActivateSeedAbility()
    {
        if (currentSeed != null)
        {
            playerVisualAnimator.SetBool("isAbility", true);
            currentSeed.GetComponent<SeedController>().GrowSeed();

            //left side, right facing
            if ((charMovement.transform.position.x < currentSeed.transform.position.x) && charMovement.isRightFacing == true)
            {
                offSet = -2.5f;
                charMovement.transform.position = new Vector3(currentSeed.transform.position.x + offSet, charMovement.transform.position.y, charMovement.transform.position.z);
            }

            //left side, left facing
            if ((charMovement.transform.position.x < currentSeed.transform.position.x) && charMovement.isRightFacing == false)
            {
                charMovement.Flip();
                offSet = -2.5f;
                charMovement.transform.position = new Vector3(currentSeed.transform.position.x + offSet, charMovement.transform.position.y, charMovement.transform.position.z);
            }

            //right side, left facing
            if ((charMovement.transform.position.x > currentSeed.transform.position.x) && charMovement.isRightFacing == false)
            {
                offSet = 2.5f;
                charMovement.transform.position = new Vector3(currentSeed.transform.position.x + offSet, charMovement.transform.position.y, charMovement.transform.position.z);
            }

            //right side, right facing
            if ((charMovement.transform.position.x > currentSeed.transform.position.x) && charMovement.isRightFacing == true)
            {
                charMovement.Flip();
                offSet = 2.5f;
                charMovement.transform.position = new Vector3(currentSeed.transform.position.x + offSet, charMovement.transform.position.y, charMovement.transform.position.z);
            }

            onWateringSeed.Invoke();
        }
    }

    #endregion

    #region Scale

    public void ActivateScaleAbility()
    {
        if(currentScalableObject != null)
        {

            if (currentScalableObject.GetComponent<ScalableObject>().state == ScalableObject.ScaledStates.defaultSize)
            {
                //left side, right facing
                if ((charMovement.transform.position.x < currentScalableObject.transform.position.x) && (charMovement.isRightFacing == true))
                {
                    offSet = -2.5f;
                    charMovement.transform.position = new Vector3(currentScalableObject.transform.position.x + offSet, charMovement.transform.position.y, charMovement.transform.position.z);
                }

                //right side, right facing
                else if ((charMovement.transform.position.x > currentScalableObject.transform.position.x) && (charMovement.isRightFacing == true))
                {
                    offSet = 2.5f;
                    charMovement.transform.position = new Vector3(currentScalableObject.transform.position.x + offSet, charMovement.transform.position.y, charMovement.transform.position.z);
                    charMovement.Flip();
                }

                //left side, left facing
                else if ((charMovement.transform.position.x < currentScalableObject.transform.position.x) && (charMovement.isRightFacing == false))
                {
                    offSet = -2.5f;
                    charMovement.transform.position = new Vector3(currentScalableObject.transform.position.x + offSet, charMovement.transform.position.y, charMovement.transform.position.z);
                    charMovement.Flip();
                }

                //right side, left facing
                else if ((charMovement.transform.position.x > currentScalableObject.transform.position.x) && (charMovement.isRightFacing == false))
                {
                    offSet = 2.5f;
                    charMovement.transform.position = new Vector3(currentScalableObject.transform.position.x + offSet, charMovement.transform.position.y, charMovement.transform.position.z);
                }


                playerVisualAnimator.SetBool("IsScalingUp", true);
                ActivateScaleUpAbility();

            }

            else if (currentScalableObject.GetComponent<ScalableObject>().state == ScalableObject.ScaledStates.scaledSize)
            {
                //left side, right facing
                if ((charMovement.transform.position.x < currentScalableObject.transform.position.x) && (charMovement.isRightFacing == true))
                {
                    offSet = -2.5f;
                    charMovement.transform.position = new Vector3(currentScalableObject.transform.position.x + offSet, charMovement.transform.position.y, charMovement.transform.position.z);
                    charMovement.Flip();
                }

                //right side, right facing
                else if ((charMovement.transform.position.x > currentScalableObject.transform.position.x) && (charMovement.isRightFacing == true))
                {
                    offSet = 2.5f;
                    charMovement.transform.position = new Vector3(currentScalableObject.transform.position.x + offSet, charMovement.transform.position.y, charMovement.transform.position.z);
                }

                //left side, left facing
                else if ((charMovement.transform.position.x < currentScalableObject.transform.position.x) && (charMovement.isRightFacing == false))
                {
                    offSet = -2.5f;
                    charMovement.transform.position = new Vector3(currentScalableObject.transform.position.x + offSet, charMovement.transform.position.y, charMovement.transform.position.z);
                }

                //right side, left facing
                else if ((charMovement.transform.position.x > currentScalableObject.transform.position.x) && (charMovement.isRightFacing == false))
                {
                    offSet = 2.5f;
                    charMovement.transform.position = new Vector3(currentScalableObject.transform.position.x + offSet, charMovement.transform.position.y, charMovement.transform.position.z);
                    charMovement.Flip();
                }

                playerVisualAnimator.SetBool("IsScalingDown", true);
                ActivateScaleDownAbility();
            }
        }
    }

    void ActivateScaleUpAbility()
    {
        currentScalableObject.GetComponent<ScalableObject>().ScaleObjectUp();
        onScaleUp.Invoke();
    }

    void ActivateScaleDownAbility()
    {
        currentScalableObject.GetComponent<ScalableObject>().ScaleObjectDown();
        onScaleDown.Invoke();
    }
    #endregion

    #region Swipe

    public void ActivateSwipeAbility()
    {
        if(currentSwipableObject != null)
        {
            playerVisualAnimator.SetBool("isAbility", true);
            currentSwipableObject.GetComponent<SwipableObject>().MoveObject();

            //left side, left facing
            if ((charMovement.transform.position.x < currentSwipableObject.transform.position.x) && charMovement.isRightFacing == false)
            {
                charMovement.Flip();
            }

            //right side, right facing
            if (charMovement.transform.position.x > currentSwipableObject.transform.position.x)
            {
                charMovement.Flip();
            }

            onSwipe.Invoke();
        }
    }

    #endregion

    #region Interact

    public void ActivateInteractAbility()
    {
        if (currentInteractObject != null)
        {
            currentInteractObject.GetComponent<InteractActionController>().Interact();
        }
    }

    #endregion
}
