using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    #region State Variables   
    public enum CatStates { defaultCat, toasterCat, witchesHatCat, wateringCanCat, telescopeCat }
    [Header("States")]
    public CatStates catState;
    public bool characterDisabled;
    public bool characterActive;

    bool toasterCatAvailable = true;
    bool witchesHatCatAvaialable = true;
    bool wateringCanCatAvailable = true;
    bool telescopeCatAvailable = true;
    #endregion

    #region Collision Variables
    Rigidbody2D rb;
    BoxCollider2D boxCollider;
    [SerializeField] Transform groundCheckCollider;
    [SerializeField] LayerMask groundLayer;
    #endregion

    #region Movement Variables
    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] float jumpSpeed;
    [SerializeField] GameObject movementParticleCanvas;
    [SerializeField] GameObject jumpingParticleCanvas;
    [SerializeField] GameObject transformationParticleCanvas;
    public float horizontal;
    public float storedHorizontal;
    public float Vertical;
    public float storedY;
    public bool isRightFacing = true;
    [SerializeField] bool readyToDoubleJump = false;
    public bool IsJumping = false;
    public bool isFalling = false;
    public bool rotationEnabled = false;
    public float groundedOverlapSize = 0.5f;
    bool wasGrounded;

    //coyote time
    [Header("Coyote Time")]
    [SerializeField] float coyoteTimeLimit;
    [SerializeField] float coyoteTimer;
    [SerializeField] bool coyoteTimeActive;  
    #endregion

    //PortraitManager portraitManager;
    PlayerInput myPlayerInput;
    SpriteRenderer visual;
    CharacterAbilities abilities;
    Animator playerVisualAnimator;

    #region Events
    [Header("Events")]
    public UnityEvent onJump = new(); 
    public UnityEvent onDoubleJump = new();
    public UnityEvent onMove = new();
    public UnityEvent onTransformation = new();
    #endregion

    #region Mono

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        myPlayerInput = GetComponent<PlayerInput>();
        abilities = GetComponent<CharacterAbilities>();
        visual = transform.GetChild(0).GetComponent<SpriteRenderer>();

        //portraitManager = GameObject.Find("Portrait Manager").GetComponent<PortraitManager>();
        //portraitManager.transform.parent.gameObject.SetActive(false);
        
        playerVisualAnimator = transform.GetChild(0).GetComponent<Animator>();

        SetCharacterActiveState(false);
    }

    private void Update()
    {
        if (characterActive)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

            //coyote time
            if (wasGrounded && !isGrounded())
            {
                wasGrounded = false;
                coyoteTimeActive = true;
            }

            if (coyoteTimeActive)
            {
                coyoteTimer += Time.deltaTime;

                if (coyoteTimer >= coyoteTimeLimit)
                {
                    coyoteTimer = 0;
                    coyoteTimeActive = false;
                }
            }

            if (isGrounded())
            {
                wasGrounded = true;
                coyoteTimer = 0;
                coyoteTimeActive = false;
            }

            //move 
            if (horizontal > 0.05f || horizontal < -0.05f)
            {
                playerVisualAnimator.SetBool("isMoving", true);

                if (isGrounded())
                    movementParticleCanvas.SetActive(true);
                else
                    movementParticleCanvas.SetActive(false);
            }

            else
            {
                playerVisualAnimator.SetBool("isMoving", false);
                movementParticleCanvas.SetActive(false);
            }

            //jump control
            if (rb.velocity.y > -2f && rb.velocity.y < 2f)
            {
                playerVisualAnimator.SetBool("IsJumping", false);
                playerVisualAnimator.SetBool("isFalling", false);

                if (isFalling || rotationEnabled)
                {
                    readyToDoubleJump = false;
                    IsJumping = false;
                    isFalling = false;
                    playerVisualAnimator.SetBool("canDoubleJump", false);
                }
            }

            else if (rb.velocity.y > 0)
            {
                IsJumping = true;
                isFalling = false;
            }

            else if (rb.velocity.y < 0)
            {
                if (IsJumping)
                    playerVisualAnimator.SetBool("isFalling", true);

                isFalling = true;
                IsJumping = false;
            }

            //flip character
            if (!isRightFacing && horizontal > 0f)
                Flip();

            else if (isRightFacing && horizontal < 0f)
                Flip();

            //clamp rotation
            ClampRotation();

            storedY = transform.position.y;
        }
        //Debug.Log("current y velocity is " + rb.velocity.y);
    }
    #endregion

    public void SwapToCatForm(CatFormData catFormData)
    {
        if (!characterDisabled)
        {
            catState = catFormData.associatedCatState;
            transformationParticleCanvas.SetActive(true);
            myPlayerInput.SwitchCurrentActionMap(catFormData.actionMapName);
            visual.sprite = catFormData.characterSprite;

            if (catFormData.associatedCatState == CatStates.defaultCat) SetAnimatorStateVariables("isDefault");
            else if (catFormData.associatedCatState == CatStates.toasterCat) SetAnimatorStateVariables("isToaster");
            else if (catFormData.associatedCatState == CatStates.wateringCanCat) SetAnimatorStateVariables("isWateringCan");
            else if (catFormData.associatedCatState == CatStates.witchesHatCat) SetAnimatorStateVariables("isWitchesHat");
            else SetAnimatorStateVariables("isTelescope");

            //portraitManager.ChangePortrait(catFormData);

            //set collider info
            boxCollider.size = new Vector2(catFormData.colliderSizeX, catFormData.colliderSizeY);
            boxCollider.offset = new Vector2(catFormData.colliderOffsetX, catFormData.colliderOffsetY);

            onTransformation.Invoke();
        }
    }

    public void SetTransformationAction(InputAction.CallbackContext context)
    {
        if (!characterDisabled)
        {
            switch (catState)
            {
                case CatStates.defaultCat:
                    if (context.performed)
                        abilities.ActivateSwipeAbility();
                    break;

                case CatStates.toasterCat:
                    abilities.ActivateToastAbility(context, isRightFacing);
                    break;

                case CatStates.wateringCanCat:
                    if (context.performed)
                        abilities.ActivateSeedAbility();
                    break;

                case CatStates.telescopeCat:
                    if (context.performed)
                        abilities.ActivateScaleAbility();
                    break;

                case CatStates.witchesHatCat:
                    if (context.performed)
                    {
                        if (abilities.heldObjectData == null)
                            abilities.ActivatePickupAbility();

                        else if (abilities.heldObjectData != null)
                            abilities.DropItem();
                    }
                    break;
            }
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!characterDisabled)
        {
            //perform jump
            if (context.performed)
            {
                playerVisualAnimator.SetBool("IsJumping", true);

                //double jump
                if (readyToDoubleJump)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                    readyToDoubleJump = false;
                    //playerVisualAnimator.SetBool("isDoubleJump", true);

                    playerVisualAnimator.SetTrigger("doubleJumpTriggered");
                    playerVisualAnimator.SetBool("canDoubleJump", false);

                    jumpingParticleCanvas.SetActive(true);
                    coyoteTimeActive = false;

                    onDoubleJump.Invoke();

                    return;
                }

                //regular jump
                if (isGrounded() || coyoteTimeActive)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                    readyToDoubleJump = true;
                    playerVisualAnimator.SetBool("canDoubleJump", true);

                    coyoteTimeActive = false;
                    coyoteTimer = 0;

                    jumpingParticleCanvas.SetActive(true);

                    onJump.Invoke();
                }              
            }

            //if button is released then reduce jump strength
            if (context.canceled && rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }            
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!characterDisabled)
        {
            horizontal = context.ReadValue<Vector2>().x;
            onMove.Invoke();
        }

        else horizontal = 0;

        playerVisualAnimator.SetFloat("Hori", horizontal);
        //playerVisualAnimator.SetFloat("Vert", horizontal);
        storedHorizontal = context.ReadValue<Vector2>().x;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (!characterDisabled)
        {
            if (context.performed) abilities.ActivateInteractAbility();
        }
    }

    public bool isGrounded()
    {
        if (Physics2D.OverlapCircle(groundCheckCollider.position, groundedOverlapSize, groundLayer) != null)
        {
            //Debug.Log("Grounded check is true");
            return true;
        }

        else
        {
            //Debug.Log("Grounded check is false");
            return false;
        }
    }

    public void Flip()
    {
        if (!characterDisabled)
        {
            isRightFacing = !isRightFacing;
            Vector3 localScale = visual.transform.localScale;
            localScale.x *= -1f;
            visual.transform.localScale = localScale;
        }
    }


    void SetAnimatorStateVariables(string catStateBoolName)
    {
        playerVisualAnimator.SetBool(catStateBoolName, true);

        switch (catStateBoolName)
        {
            case "isDefault":
                playerVisualAnimator.SetBool("isToaster", false);
                playerVisualAnimator.SetBool("isWitchesHat", false);
                playerVisualAnimator.SetBool("isTelescope", false);
                playerVisualAnimator.SetBool("isWateringCan", false);
                break;

            case "isToaster":
                playerVisualAnimator.SetBool("isDefault", false);
                playerVisualAnimator.SetBool("isWitchesHat", false);
                playerVisualAnimator.SetBool("isTelescope", false);
                playerVisualAnimator.SetBool("isWateringCan", false);
                break;

            case "isWitchesHat":
                playerVisualAnimator.SetBool("isToaster", false);
                playerVisualAnimator.SetBool("isDefault", false);
                playerVisualAnimator.SetBool("isTelescope", false);
                playerVisualAnimator.SetBool("isWateringCan", false);
                break;

            case "isTelescope":
                playerVisualAnimator.SetBool("isToaster", false);
                playerVisualAnimator.SetBool("isWitchesHat", false);
                playerVisualAnimator.SetBool("isDefault", false);
                playerVisualAnimator.SetBool("isWateringCan", false);
                break;

            case "isWateringCan":
                playerVisualAnimator.SetBool("isToaster", false);
                playerVisualAnimator.SetBool("isWitchesHat", false);
                playerVisualAnimator.SetBool("isTelescope", false);
                playerVisualAnimator.SetBool("isDefault", false);
                break;
        }  
    }

    #region Disabling and Enabling
    public void DisableCharacter()
    {
        horizontal = 0;
        playerVisualAnimator.SetFloat("Hori", horizontal);
        characterDisabled = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;

        //rb.simulated = false;

        //StopRBSimulation();
    }

    public void ReenableCharacter()
    {
        horizontal = storedHorizontal;
        characterDisabled = false;
        rb.constraints = RigidbodyConstraints2D.None;
        playerVisualAnimator.SetFloat("Hori", horizontal);

        //rb.simulated = true;
    }

    public void SetCharacterActiveState(bool activeState)
    {
        if(activeState)
        {
            characterActive = true;
            ReenableCharacter();
        }

        else
        {
            characterActive = false;
            DisableCharacter();
        } 
            
    }

    IEnumerator StopRBSimulation()
    {
        bool amGroundedYet = false;

        while (!amGroundedYet)
        {
            if (isGrounded())
                amGroundedYet = true;
        }

        rb.simulated = false;
        yield return null;
    }
    #endregion

    void ClampRotation()
    {
        if (rotationEnabled)
        {
            float currentZRotation = transform.rotation.eulerAngles.z;

            if (currentZRotation > 20 && currentZRotation < 90)
                transform.rotation = Quaternion.Euler(0, 0, 20);

            else if (currentZRotation < 340 && currentZRotation > 270)
                transform.rotation = Quaternion.Euler(0, 0, 340);
        }

        else transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void MoveCharacterToTransform(Transform targetTransform)
    {
        transform.position = targetTransform.position;
    }
}
