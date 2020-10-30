using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] private int pieceNum = 0;

    public int GetPieceNum {
        get { return pieceNum; }
    }

    public int SetPieceNum {
        set { pieceNum = value; }
    }
}
