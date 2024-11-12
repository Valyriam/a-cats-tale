using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBasicImage : MonoBehaviour
{
    [SerializeField] GameObject container;
    [SerializeField] GameObject particleCanvas;
    public PuzzleData associatedPuzzleData;
    [SerializeField] List<PlatformParent> targetImagePlatformParents = new List<PlatformParent>();

    Vector3 startPosition;
    Quaternion startRotation;

    private void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void Explode()
    {
        container.SetActive(false);
        particleCanvas.SetActive(true);

        //enable colliders for taget images
        foreach (PlatformParent targetImage in targetImagePlatformParents)
        {
            targetImage.unsolvedPosition.GetComponent<Rigidbody2D>().simulated = true;
        }
    }

    public void ResetDestructibleObject()
    {
        if (associatedPuzzleData != null)
        {
            //if puzzle hasn't been solved yet
            if (!associatedPuzzleData.isPuzzleSolved)
            {
                //return to original position
                transform.position = startPosition;
                transform.rotation = startRotation;

                //reset visuals
                container.SetActive(true);
                particleCanvas.SetActive(false);

                //reset target images
                foreach (PlatformParent targetImage in targetImagePlatformParents)
                {
                    targetImage.unsolvedPosition.GetComponent<Rigidbody2D>().simulated = false;
                    targetImage.ResetUnsolvedPosition();
                }
            }
        }
    }
}
