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
    private void OnMouseDown()
    {
        GameManager.Instance.SelectTile(this);
        //Debug.Log("Selected");
    }
    private void OnMouseEnter()
    {
        GameManager.Instance.TargetTile(this);
    }
    private void OnMouseUp()
    {
        GameManager.Instance.Released();
    }
}
