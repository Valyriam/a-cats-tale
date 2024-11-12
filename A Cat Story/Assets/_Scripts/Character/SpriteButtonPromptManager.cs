using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpriteButtonPromptManager : MonoBehaviour
{
    PlayerInput playerInput;
    SpriteRenderer spriteRenderer;

    [SerializeField] Sprite keyboardSprite, controllerSprite;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerInput = GameObject.Find("Character").GetComponent<PlayerInput>();
        //gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (playerInput != null)
            playerInput = GameObject.Find("Character").GetComponent<PlayerInput>();

        if (playerInput.currentControlScheme == "KeyboardMouse")
        {
            spriteRenderer.sprite = keyboardSprite;
        }

        else if (playerInput.currentControlScheme == "Gamepad")
        {
            spriteRenderer.sprite = controllerSprite;
        }

        else
        {
            spriteRenderer.sprite = keyboardSprite;
        }
    }
}
