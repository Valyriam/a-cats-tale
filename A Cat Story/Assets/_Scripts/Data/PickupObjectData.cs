using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pickup Object Data", menuName = "Pickup Object Data")]
public class PickupObjectData : ScriptableObject
{
    public string objectName;
    public Sprite objectSprite;
    public GameObject objectPrefab;
}
