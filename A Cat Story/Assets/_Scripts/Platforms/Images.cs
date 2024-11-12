using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Images : MonoBehaviour
{
    public bool hasShadow;
    public GameObject visual;
    public float xPosOffset = 0.5f;
    public float yPosOffset = 1f;
    // Start is called before the first frame update
    void Start()
    {
        if(hasShadow && visual)
        {
            CreateShadow();
        }
    }

    void CreateShadow()
    {
        GameObject shadowVisual = Instantiate(visual, transform.position, Quaternion.identity, transform);

        SpriteRenderer sr = shadowVisual.GetComponent<SpriteRenderer>();

        //colour
        Color tempColor = sr.color;
        tempColor = Color.black;
        tempColor.a = 0.3f;
        sr.color = tempColor;

        //position
        float newXPos = transform.position.x + xPosOffset;
        float newYPos = transform.position.y + yPosOffset;
        shadowVisual.transform.position = new Vector3(newXPos, newYPos, transform.position.z);
    }
}
