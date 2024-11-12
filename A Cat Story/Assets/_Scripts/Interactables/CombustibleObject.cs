using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombustibleObject : MonoBehaviour
{
    GameObject originalPageContent;
    GameObject holeInPageContent;
    GameObject holeInPageContent2;

    private void Start()
    {
        originalPageContent = transform.GetChild(0).gameObject;
        holeInPageContent = transform.GetChild(1).gameObject;
        holeInPageContent2 = transform.GetChild(2).gameObject;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Combust();
        }
    }

    void Combust()
    {
        holeInPageContent.SetActive(true);
        originalPageContent.SetActive(false);
        holeInPageContent2.SetActive(true);
    }
}
