using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int indiceX, indiceY;

    public void Indice(int heigth, int width)
    {
        indiceX = heigth;
        indiceY = width;
    }
}
