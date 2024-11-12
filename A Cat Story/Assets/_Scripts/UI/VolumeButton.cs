using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class VolumeButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI myTMP;
    VolumeController volumeController;


    private void Start()
    {
        volumeController = GameObject.FindObjectOfType<VolumeController>(); 
    }

    public void AddToTextValue()
    {
        int currentTextValue = int.Parse(myTMP.text);

        if (currentTextValue == 100)
            return;

        else
        {
            int newTextValue = currentTextValue + 5;
            myTMP.text = "" + newTextValue;
        }
    }

    public void SubtractFromTextValue()
    {
        int currentTextValue = int.Parse(myTMP.text);

        if (currentTextValue == 0)
            return;

        else
        {
            int newTextValue = currentTextValue - 5;
            myTMP.text = "" + newTextValue;
        }
    }
}
