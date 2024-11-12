using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SeedController : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] PlatformParent platformParent;
    [SerializeField] GameObject outlineContainer;
    public GameObject stemPrefab;
    public GameObject flowerPrefab;
    public FlowerCycle flowerCycle;
    [SerializeField] bool hasOutline;
    [SerializeField] Sprite incorrectFormSprite;
    GameObject playerIncorrectFormVisual;

    GameObject Player;   
    GameObject visual;
    SpriteRenderer visualSR;    

    [Header("Plant Variables")]
    [Range(2f, 6f)]
    [SerializeField] int plantHeight;
    [SerializeField] int stemOffset;
    [SerializeField] int flowerOffset;
    [SerializeField] float startDelay;
    [SerializeField] int plantSortingOrder;

    [Header("Bounce Variables")]
    [SerializeField] bool isBouncy;
    [SerializeField] float bounceForce;

    int partsLeftToGrow, iterations;
    CharacterAbilities characterAbilities;
    CharacterMovement characterMovement;
    GameObject buttonPrompt;

    [Header("Events")]
    public UnityEvent onSeedGrowth = new();


    //public FlowerCycle;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Character");

        partsLeftToGrow = plantHeight;
        flowerCycle = FlowerCycle.Seed;
        visual = transform.GetChild(0).gameObject;
        visualSR = visual.GetComponent<SpriteRenderer>();

        buttonPrompt = Player.transform.GetChild(4).gameObject;
        characterAbilities = Player.GetComponent<CharacterAbilities>();
        characterMovement = Player.GetComponent<CharacterMovement>();
    }

    public enum FlowerCycle
    {
        Seed,
        Growing,
        Flower
    }

    public void ChangeType()
    {
        switch (flowerCycle)
        {
            case FlowerCycle.Seed:
                break;

            case FlowerCycle.Growing:
                if (partsLeftToGrow > 0)
                {
                    GameObject plant = Instantiate(stemPrefab, transform) as GameObject;
                    plant.name = iterations.ToString();
                    plant.transform.position = new Vector3(transform.position.x, transform.position.y + (stemOffset * iterations), transform.position.z);
                    plant.GetComponent<SpriteRenderer>().sortingOrder = plantSortingOrder;
                    partsLeftToGrow--;
                    iterations++;

                    //check to grow flower head
                    if(partsLeftToGrow == 1)
                    {
                        GameObject flower = Instantiate(flowerPrefab, transform) as GameObject;
                        flower.name = iterations.ToString();
                        flower.transform.position = new Vector3(plant.transform.position.x, plant.transform.position.y + flowerOffset, plant.transform.position.z);
                        flower.GetComponent<SpriteRenderer>().sortingOrder = visualSR.sortingOrder;
                        platformParent.platformCollider = flower;

                        //bouncy
                        if (isBouncy)
                            flower.GetComponent<Platforms>().bounceForce = bounceForce;

                        partsLeftToGrow--;

                        //set outline container for platform parent
                        if (hasOutline)
                            outlineContainer = flower.transform.GetChild(1).gameObject;
                    }              
                }

                if (partsLeftToGrow == 0)
                {
                    SetSeedVisualsActiveState(false);
                    flowerCycle = FlowerCycle.Flower;
                    SetOutlineContainerInPlatformParent();
                }
                break;

            case FlowerCycle.Flower:
                break;
        }
    }


    private void Update()
    {
        ChangeType();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            if (!playerIncorrectFormVisual)
                playerIncorrectFormVisual = collision.gameObject.transform.GetChild(5).gameObject;

            if (characterMovement.catState == CharacterMovement.CatStates.wateringCanCat && flowerCycle == FlowerCycle.Seed)
            {
                characterAbilities.currentSeed = this.gameObject;
                buttonPrompt.SetActive(true);
            }

            else if (characterMovement.catState != CharacterMovement.CatStates.wateringCanCat && flowerCycle == FlowerCycle.Seed)
            {
                playerIncorrectFormVisual.SetActive(true);
                playerIncorrectFormVisual.GetComponent<SpriteRenderer>().sprite = incorrectFormSprite;
            }

            else
            {
                collision.gameObject.transform.GetChild(5).gameObject.SetActive(false);
            }
               

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            collision.gameObject.GetComponent<CharacterAbilities>().currentSeed = null;
            buttonPrompt.SetActive(false);

            if (playerIncorrectFormVisual != null)
                playerIncorrectFormVisual.SetActive(false);
        }
    }

    public void GrowSeed()
    {
        StartCoroutine(DelayedSeedGrowth());
    }

    IEnumerator DelayedSeedGrowth()
    {
        yield return new WaitForSeconds(startDelay);

        onSeedGrowth.Invoke();
        flowerCycle = FlowerCycle.Growing;
        buttonPrompt.SetActive(false);
    }

    public void ResetSeed()
    {
        flowerCycle = FlowerCycle.Seed;
        SetSeedVisualsActiveState(true);
        partsLeftToGrow = plantHeight;
        iterations = 0; 

        //destroy all flower parts but keep visual
        for (int i = 1; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    void SetSeedVisualsActiveState(bool activeState)
    {
        visual.SetActive(activeState);
    }

    void SetOutlineContainerInPlatformParent()
    {
        if(hasOutline)
        {
            platformParent.outlineContainer = outlineContainer;
        }
    }

    //    private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("red");
    //    sprite.color = Color.red;
    //}

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    Debug.Log("yellow");
    //    sprite.color = Color.yellow;
    //    flowerCycle = FlowerCycle.Growing;
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    Debug.Log("green");
    //    sprite.color = Color.green;
    //}


}  

