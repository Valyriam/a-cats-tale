using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    public enum Types {Moving, Slippery, Bouncy, Conveyor}
    public Types platformType;
    [SerializeField] Rigidbody2D thisRb;

    //Movement
    public bool moveRight, moveLeft, MovingOn;
    public float movingDistance, movingSpeed;

    //Slippery
    public bool isOnSlipperyPlatform;
    public float slipperyForce, friction;
    Rigidbody2D playerRb;
    Vector3 rightPos, leftPos;
    public Transform objectparent;

    //Conveyor
    public float cNewPos, cSpeed;

    //Bouncy
    public float bounceForce;

    void Start()
    {
        playerRb = GameObject.Find("Character").GetComponent<Rigidbody2D>();

        rightPos = transform.position + new Vector3(movingDistance, 0);
        leftPos = transform.position - new Vector3(movingDistance, 0);


        if (platformType == Types.Moving)
        {
            MovingOn = true;

        }
        else if (platformType == Types.Slippery || platformType == Types.Bouncy)
        {
            MovingOn = false;
        }
 
    }

    void Update()
    {
        #region Moving Platform
        //Moving Platform
        if (MovingOn) 
        {
            if (moveLeft)
            {
                //transform.position = Vector3.MoveTowards(transform.position, leftPos, movingSpeed);

                //if (transform.position.x <= leftPos.x)
                //{
                //    moveRight = true;
                //    moveLeft = false;
                //}

                
            }
            else if (moveRight)
            {
                //transform.position = Vector3.MoveTowards(transform.position, rightPos, movingSpeed);

                //if (transform.position.x >= rightPos.x)
                //{
                //    moveLeft = true;
                //    moveRight = false;
                //}
            }
         
        }
        #endregion

    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        //Moving platform collision 
        if (collision.gameObject.tag == "Character" && platformType == Types.Moving)
        {
            collision.gameObject.transform.SetParent(transform);
        }

        //Conveyor platform collision
        if (collision.gameObject.tag == "Character" && platformType == Types.Conveyor)
        {
            playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            Debug.Log("enter");
        }

        //Slippery platform collision
        if (collision.gameObject.tag == "Character" && platformType == Types.Slippery)
        {
            playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            isOnSlipperyPlatform = true;
            Debug.Log("enter");
        }

        //Bouncy platform collison
        if (collision.gameObject.tag == "Character" && platformType == Types.Bouncy)
        {
            playerRb.velocity = Vector2.zero;

            playerRb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        //Slippery platform - adding slippery force
        if (collision.gameObject.tag == "Character" && platformType == Types.Slippery)
        {
            //Vector2 slideDirection = collision.transform.right * (collision.transform.localScale.x > 0 ? 1 : -1);
            //Debug.Log(slideDirection);
            //playerRb.AddForce(slideDirection * slipperyForce, ForceMode2D.Force);

            Vector2 slideDirection = new Vector2(collision.transform.localScale.x, 0).normalized;
            playerRb.AddForce(slideDirection * slipperyForce, ForceMode2D.Force);

            playerRb.velocity = Vector2.Lerp(playerRb.velocity, Vector2.zero, Time.deltaTime);
        }

        //Conveyor belt push
        if (collision.gameObject.tag == "Character" && platformType == Types.Conveyor)
        {
            Vector2 direction = collision.transform.right * (collision.transform.localScale.x > 0 ? 1 : -1);
            Vector2 targetPos = direction * cNewPos;

            playerRb.position = Vector2.MoveTowards(playerRb.position, targetPos, cSpeed);
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        //Moving platform collision 
        if (collision.gameObject.tag == "Character" && platformType == Types.Moving)
        {
            collision.gameObject.transform.SetParent(objectparent);
        }

        //Slippery + Conveyor platform collision
        if (collision.gameObject.tag == "Character" && platformType == Types.Slippery || collision.gameObject.tag == "Character" && platformType == Types.Conveyor)
        {
            isOnSlipperyPlatform = false;
            playerRb = null;
        }
    }

}
