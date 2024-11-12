using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "[FontName] - Font Collection Data", menuName = "Font Collection Data")]
public class FontCollectionData : ScriptableObject
{
    public int standardFontSize;

    [Header("Untextured Fonts")]
    public TMP_FontAsset neutralFont;
    public TMP_FontAsset bounceFont;
    public TMP_FontAsset bulletFont;
    public TMP_FontAsset conveyorFont;
    public TMP_FontAsset gravityFont;
    public TMP_FontAsset swipableFont;
    public TMP_FontAsset slipperyFont;

    [Header("Textured Fonts")]
    public TMP_FontAsset neutralFontTextured;
    public TMP_FontAsset bounceFontTextured;
    public TMP_FontAsset bulletFontTextured;
    public TMP_FontAsset conveyorFontTextured;
    public TMP_FontAsset gravityFontTextured;
    public TMP_FontAsset swipableFontTextured;
    public TMP_FontAsset slipperyFontTextured;

   

}
