using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ButtonPromptManager : MonoBehaviour
{
    [SerializeField] TextMeshPro myTextMesh;
    [SerializeField] PlayerInput playerInput;
    FontManager fontManager;

    private void Start()
    {
        myTextMesh = GetComponent<TextMeshPro>();
        playerInput = transform.parent.gameObject.GetComponent<PlayerInput>();
        fontManager = GameObject.FindObjectOfType<FontManager>();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        myTextMesh = GetComponent<TextMeshPro>();      
        
        if(playerInput != null)
            playerInput = transform.parent.GetComponent<PlayerInput>();

        if(fontManager != null)
            myTextMesh.font = fontManager.currentFontCollection.neutralFont;

        if (playerInput.currentControlScheme == "KeyboardMouse") 
        {
            myTextMesh.text = "<wiggle> Press Q";
        }

        else if (playerInput.currentControlScheme == "Gamepad")
            myTextMesh.text = "<wiggle> Press B";

        else
        {
            myTextMesh.text = "<wiggle> Press Q";
        }
    }
}
