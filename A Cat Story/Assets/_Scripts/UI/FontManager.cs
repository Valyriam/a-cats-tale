using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FontManager : MonoBehaviour
{
    public enum AllFonts { BLKCHCRY, SnickerSnack, font3, font4, font5 };
    public AllFonts defaultFont;
    public FontCollectionData currentFontCollection;

    [SerializeField] List<PlatformParent> platformParents = new List<PlatformParent>();
    [SerializeField] FontCollectionData BLKCHCRY_FC;
    [SerializeField] FontCollectionData SnickerSnack_FC;
    [SerializeField] FontCollectionData font3;
    [SerializeField] FontCollectionData font4;
    [SerializeField] FontCollectionData font5;

    void Start()
    {
        platformParents.AddRange(FindObjectsOfType<PlatformParent>());
        SetFont(defaultFont);
    }

    public void SetFont(AllFonts selectedFont)
    {
        if (selectedFont == AllFonts.BLKCHCRY)
            SetPlatformParentFonts(BLKCHCRY_FC);

        else if (selectedFont == AllFonts.SnickerSnack)
            SetPlatformParentFonts(SnickerSnack_FC);

        else if (selectedFont == AllFonts.font3)
            SetPlatformParentFonts(font3);

        else if (selectedFont == AllFonts.font4)
            SetPlatformParentFonts(font4);

        else if (selectedFont == AllFonts.font5)
            SetPlatformParentFonts(font5);
    }

    void SetPlatformParentFonts(FontCollectionData fontCollection)
    {
        currentFontCollection = fontCollection;

        foreach (PlatformParent platformParent in platformParents)
        {

            switch(platformParent.platformType)
            {
                case PlatformParent.AllPlatformTypes.bouncy:

                    if(platformParent.isSolved && platformParent.removeOutlineOnSolve)
                        platformParent.SetPlatformFont(fontCollection.bounceFontTextured);

                    else
                        platformParent.SetPlatformFont(fontCollection.bounceFont);                   
                break;

                case PlatformParent.AllPlatformTypes.bulletpoint:

                    if (platformParent.isSolved && platformParent.removeOutlineOnSolve)
                        platformParent.SetPlatformFont(fontCollection.bulletFontTextured);

                    else
                        platformParent.SetPlatformFont(fontCollection.bulletFont);
                    break;

                case PlatformParent.AllPlatformTypes.conveyor:

                    if (platformParent.isSolved && platformParent.removeOutlineOnSolve)
                        platformParent.SetPlatformFont(fontCollection.conveyorFontTextured);

                    else
                        platformParent.SetPlatformFont(fontCollection.conveyorFont);
                    break;

                case PlatformParent.AllPlatformTypes.neutral:

                    if (platformParent.isSolved && platformParent.removeOutlineOnSolve)
                        platformParent.SetPlatformFont(fontCollection.neutralFontTextured);

                    else
                        platformParent.SetPlatformFont(fontCollection.neutralFont);
                    break;

                case PlatformParent.AllPlatformTypes.gravity:

                    if (platformParent.isSolved && platformParent.removeOutlineOnSolve)
                        platformParent.SetPlatformFont(fontCollection.gravityFontTextured);

                    else
                        platformParent.SetPlatformFont(fontCollection.gravityFont);
                    break;

                case PlatformParent.AllPlatformTypes.swipableText:

                    if (platformParent.isSolved && platformParent.removeOutlineOnSolve)
                        platformParent.SetPlatformFont(fontCollection.swipableFontTextured);

                    else
                        platformParent.SetPlatformFont(fontCollection.swipableFont);
                    break;

                case PlatformParent.AllPlatformTypes.slippery:

                    if (platformParent.isSolved && platformParent.removeOutlineOnSolve)
                        platformParent.SetPlatformFont(fontCollection.slipperyFontTextured);

                    else
                        platformParent.SetPlatformFont(fontCollection.slipperyFont);
                    break;
            }

            /*
            if (platformParent.platformType == PlatformParent.AllPlatformTypes.bouncy)
                platformParent.SetPlatformFont(fontCollection.bounceFont);

            else if (platformParent.platformType == PlatformParent.AllPlatformTypes.bulletpoint)
                platformParent.SetPlatformFont(fontCollection.bulletFont);

            else if (platformParent.platformType == PlatformParent.AllPlatformTypes.conveyor)
                platformParent.SetPlatformFont(fontCollection.conveyorFont);

            else if (platformParent.platformType == PlatformParent.AllPlatformTypes.neutral)
                platformParent.SetPlatformFont(fontCollection.neutralFont);

            else if (platformParent.platformType == PlatformParent.AllPlatformTypes.gravity)
                platformParent.SetPlatformFont(fontCollection.gravityFont);

            else if (platformParent.platformType == PlatformParent.AllPlatformTypes.swipable)
                platformParent.SetPlatformFont(fontCollection.swipableFont);

            else if (platformParent.platformType == PlatformParent.AllPlatformTypes.slippery)
                platformParent.SetPlatformFont(fontCollection.slipperyFont);

            else
                return;
            */
        }
    }

    public TMP_FontAsset FindCorrectSolvedFont(PlatformParent platformParent)
    {
        if (platformParent.platformType == PlatformParent.AllPlatformTypes.bouncy)
            return currentFontCollection.bounceFontTextured;

        else if (platformParent.platformType == PlatformParent.AllPlatformTypes.bulletpoint)
            return currentFontCollection.bulletFontTextured;

        else if (platformParent.platformType == PlatformParent.AllPlatformTypes.conveyor)
            return currentFontCollection.conveyorFontTextured;

        else if (platformParent.platformType == PlatformParent.AllPlatformTypes.neutral)
            return currentFontCollection.neutralFontTextured;

        else if (platformParent.platformType == PlatformParent.AllPlatformTypes.gravity)
            return currentFontCollection.gravityFontTextured;

        else if (platformParent.platformType == PlatformParent.AllPlatformTypes.swipableText)
            return currentFontCollection.swipableFontTextured;

        else if (platformParent.platformType == PlatformParent.AllPlatformTypes.slippery)
            return currentFontCollection.slipperyFontTextured;

        else
            return null;
    }
}
