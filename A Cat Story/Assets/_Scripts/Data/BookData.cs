using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Name - Book Data", menuName = "Book Data")]
public class BookData : ScriptableObject
{
    public string bookName;

    public List<PuzzleData> AllPuzzles = new List<PuzzleData>();
}
