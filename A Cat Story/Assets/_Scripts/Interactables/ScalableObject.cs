using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalableObject : MonoBehaviour
{
    public SpriteRenderer sprite;
    public float scaleMagnitude;
    public float defaultScaleMagnitude;
    [SerializeField] GameObject buttonPrompt;

    public enum ScaledStates { defaultSize, scaledSize }
    public ScaledStates state;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        defaultScaleMagnitude = transform.localScale.x;
        state = ScaledStates.defaultSize;
        buttonPrompt = GameObject.Find("Character").transform.GetChild(4).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            if (collision.gameObject.GetComponent<CharacterMovement>().catState == CharacterMovement.CatStates.telescopeCat)
            {
                collision.gameObject.GetComponent<CharacterAbilities>().currentScalableObject = this.gameObject;
                buttonPrompt.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            collision.gameObject.GetComponent<CharacterAbilities>().currentScalableObject = null;
            buttonPrompt.SetActive(false);
        }
    }

    public void ScaleObjectUp()
    {
        transform.localScale = new Vector3(scaleMagnitude, scaleMagnitude);
        state = ScaledStates.scaledSize;
    }

    public void ScaleObjectDown()
    {
        transform.localScale = new Vector3(defaultScaleMagnitude, defaultScaleMagnitude);
        state = ScaledStates.defaultSize;
    }
}
