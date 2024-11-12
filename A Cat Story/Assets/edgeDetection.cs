using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class edgeDetection : MonoBehaviour
{
    public float upwardPushForce, horizontalPushForce;

    public bool leftSide, rightSide, Left, Right, active;
    public Rigidbody2D playerRb;
    public CharacterMovement playerScript;
    public GameObject leftPositon, rightPositon;


    private void Start()
    {

    }

    void Update()
    {


        //if (playerScript.isGrounded() == false && active)
        //{
        //    if (leftSide && playerRb.velocity.x > 0f)
        //    {
        //        Vector3 storedVelocity = playerRb.velocity;

        //        playerRb.velocity = Vector3.zero;

        //        playerRb.AddForce(new Vector2(-horizontalPushForce, 0), ForceMode2D.Force);
        //        //playerRb.AddForce(new Vector2(0, upwardPushForce), ForceMode2D.Impulse);
        //        playerRb.velocity = storedVelocity;
        //    }
        //    else if (rightSide)
        //    {
        //        playerRb.AddForce(new Vector2(horizontalPushForce, 0), ForceMode2D.Force);
        //        playerRb.AddForce(new Vector2(0, upwardPushForce), ForceMode2D.Impulse);
        //    }
        //}

        //if (playerScript.isGrounded() == true)
        //{
        //    active = false;
        //}
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Character Ceiling")
        {
            Debug.Log("beforeedgetaga");
            if (gameObject.tag == "Right Edge")
            {
                Debug.Log("works");
                playerRb.position = rightPositon.transform.position;
                playerRb.AddForce(new Vector2(0, upwardPushForce), ForceMode2D.Impulse);
            }
            else if (gameObject.tag == "Left Edge")
            {
                Debug.Log("works");
                playerRb.position = leftPositon.transform.position;
            }
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Character Ceiling")
        {
            if (gameObject.tag == "Right Edge")
            {
                Debug.Log("exitworks");
            }
            else if (gameObject.tag == "Left Edge")
            {
                Debug.Log("exitworks");
            }
        }
    }

   
}
