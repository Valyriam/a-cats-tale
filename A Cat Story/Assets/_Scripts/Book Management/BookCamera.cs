using echo17.EndlessBook;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BookCamera : MonoBehaviour
{
    [SerializeField] Vector3 turningPagesPosition;
    [SerializeField] Vector3 playingPosition;

    EndlessBook myBook;
    [SerializeField] float lerpSpeed = 0.5f;

    private void Start()
    {
        turningPagesPosition = transform.GetChild(0).transform.position;
        playingPosition = transform.GetChild(1).transform.position;       
    }

    public void ActivateOnPageCameraLerp(EndlessBook book)
    {
        Debug.Log("On page camera lerp called");

        myBook = book;

        StartCoroutine(LerpToTurningPosition());
    }

    IEnumerator LerpToTurningPosition()
    {
        while (transform.position != turningPagesPosition)
        {
            transform.position = Vector3.Lerp(transform.position, turningPagesPosition, lerpSpeed * Time.deltaTime);
            yield return null;
        }

        if(transform.position == turningPagesPosition) StartCoroutine(LerpToPlayingPosition());

        yield return null;
    }

    IEnumerator LerpToPlayingPosition()
    {
        while (transform.position != playingPosition)
        {
            transform.position = Vector3.Lerp(transform.position, playingPosition, lerpSpeed * Time.deltaTime);
            yield return null;
        }

        yield return null;
    }
}
