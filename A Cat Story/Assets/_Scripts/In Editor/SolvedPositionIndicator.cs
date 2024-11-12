using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class SolvedPositionIndicator : MonoBehaviour
{
    public PlatformParent platformParent;
    public GameObject platformReference;

#if UNITY_EDITOR
    private void Start()
    {      
        platformParent = transform.parent.GetComponent<PlatformParent>();
        platformReference = platformParent.platform;

        InstantiateReference();
    }

    public void InstantiateReference()
    {
        if(transform.childCount > 0)
            DestroyImmediate(transform.GetChild(0).gameObject);

        if ((transform.childCount == 0))
        {
            if (platformReference != null)
            {
                GameObject platformReferenceInstance = Instantiate(platformReference, transform.position, Quaternion.identity, transform);
                platformReferenceInstance.transform.localEulerAngles = Vector3.zero;

                List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
                spriteRenderers.AddRange(platformReferenceInstance.transform.GetComponentsInChildren<SpriteRenderer>());

                //SpriteRenderer platformColourBG = platformReferenceInstance.transform.GetComponentInChildren<SpriteRenderer>();
                TextMeshPro platformText = platformReferenceInstance.transform.GetComponentInChildren<TextMeshPro>();

                if(spriteRenderers.Count > 0)
                {
                    foreach(SpriteRenderer sr in spriteRenderers)
                    {
                        Color tempColor = sr.color;
                        tempColor.a = 0.3f;
                        sr.color = tempColor;
                    }
                }

                //if (platformColourBG)
                //{
                //    Color tempColor = platformColourBG.color;
                //    tempColor.a = 0.3f;
                //    platformColourBG.color = tempColor;
                //}

                if(platformText)
                {
                    Color tempColor = platformText.color;
                    tempColor.a = 0.3f;
                    platformText.color = tempColor;
                }

                platformReferenceInstance.AddComponent<DestroyOnLoad>();
            }
        }
    }

#endif
}
