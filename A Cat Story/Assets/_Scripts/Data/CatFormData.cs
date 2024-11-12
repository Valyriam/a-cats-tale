using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cat Form Data", menuName = "Cat Form Data")]
public class CatFormData : ScriptableObject
{
    public Sprite characterSprite;
    public string actionMapName;
    public CharacterMovement.CatStates associatedCatState;
    public float colliderOffsetX;
    public float colliderOffsetY;
    public float colliderSizeX;
    public float colliderSizeY;
}
