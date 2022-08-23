using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public int indiceX, indiceY;

    public void SetPosition(int X, int Y)
    {
        indiceX = X;
        indiceY = Y;
    }

}
