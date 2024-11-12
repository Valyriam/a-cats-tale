using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class DeathCollider : MonoBehaviour
{
    public GameManager gameManager;
    [SerializeField] CharacterAbilities characterAbilities;
    [SerializeField] private UnityEvent onDeath = new();

    [SerializeField] List<SwipableObject> swipObjects = new List<SwipableObject>();
    [SerializeField] List<SeedController> seedControllers = new List<SeedController>();
    [SerializeField] List<TransformationPickup> transformationPickups = new List<TransformationPickup>();
    [SerializeField] List<DestructibleBasicImage> destructibleImages = new List<DestructibleBasicImage>();
    [SerializeField] List<GameObject> pickupObjects = new List<GameObject>();

    private void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        characterAbilities = GameObject.FindObjectOfType<CharacterAbilities>();

        swipObjects.AddRange(GameObject.FindObjectsOfType<SwipableObject>());
        seedControllers.AddRange(GameObject.FindObjectsOfType<SeedController>());
        transformationPickups.AddRange(GameObject.FindObjectsOfType<TransformationPickup>());
        destructibleImages.AddRange(GameObject.FindObjectsOfType<DestructibleBasicImage>());

        pickupObjects.AddRange(GameObject.FindGameObjectsWithTag("Pickup PP"));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Character")
        {
            gameManager.Reload();
            onDeath.Invoke();
            ResetSwipableObjects();
            ResetSeedObjects();
            ResetTransformationPickups();
            ResetDestructibleImages();
            ResetPickupObjects();
        }
    }

    void ResetPickupObjects()
    {
        //if holding an object, drop it and reset
        if (characterAbilities.pickupObjectParent != null)
        {
            characterAbilities.ResetHeldPickupObject();
        }

        foreach (GameObject pickupObject in pickupObjects)
        {
            PlatformParent platformParent = pickupObject.GetComponent<PlatformParent>();

            if (platformParent != null)
            {
                //if not solved then reset to original location
                if (!platformParent.isSolved)
                {
                    platformParent.ResetUnsolvedPosition();
                }
            }
        }
    }

    void ResetDestructibleImages()
    {
        foreach (DestructibleBasicImage destructibleImage in destructibleImages)
        {
            if (destructibleImage != null)
            {
                //if not solved then reset to original location
                if (!destructibleImage.associatedPuzzleData.isPuzzleSolved)
                {
                    destructibleImage.ResetDestructibleObject();
                }
            }
        }
    }

    void ResetTransformationPickups()
    {
        foreach (TransformationPickup transformPickup in transformationPickups)
        {
            if (transformPickup != null)
            {
                //if not solved then reset to original location
                if (!transformPickup.isSolved)
                {
                    transformPickup.ResetPickup();
                }
            }
        }
    }

    void ResetSwipableObjects()
    {
        foreach (SwipableObject swipObject in swipObjects)
        {
            if (swipObject != null)
            {
                PlatformParent platformParent = swipObject.transform.parent.GetComponentInParent<PlatformParent>();

                //if not solved then reset to original location
                if (!platformParent.isSolved)
                {
                    swipObject.MoveToReloadLocation();
                }
            }
        }
    }

    void ResetSeedObjects()
    {
        foreach (SeedController seedController in seedControllers)
        {
            if (seedController != null)
            {
                PlatformParent platformParent = seedController.GetComponentInParent<PlatformParent>();

                //if not solved then reset seed
                if (!platformParent.isSolved)
                {
                    seedController.ResetSeed();
                }
            }
        }
    }
}
