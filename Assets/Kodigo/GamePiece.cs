using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GamePiece : MonoBehaviour
{
    public int indiceX, indiceY;
    [Range(0f, 5f)]
    public bool enMovimiento;
    public Types tipos;
    public int colorCode;

    public void SetPosition(int X, int Y)
    {
        indiceX = X;
        indiceY = Y;
    }
    public void SetPrefab(int color)
    {
        colorCode = color;
    }
    IEnumerator MovePiece(int x, int y, float tiempoDeAccion)
    {
        GameManager.Instance.enEjecucion = true;
        enMovimiento = true;
        Vector3 posicionDeseada = new Vector3(x, y, 0);
        Vector3 posicionInicial = transform.position;
        Debug.Log($"Start: {posicionInicial}, Desire: {posicionDeseada}");
        float tiempoTranscurrido = 0f;
        while (Vector3.Distance(transform.position, posicionDeseada) > 0.01f)
        {
            float t = tiempoTranscurrido / tiempoDeAccion;
            switch (tipos)
            {
                case Types.Linear:

                    break;
                case Types.Senoidal:
                    t = Mathf.Sin(t * Mathf.PI * 0.5f);
                    break;
                case Types.Cosenoidal:
                    t = 1 - Mathf.Cos(t * Mathf.PI * 0.5f);
                    break;
                case Types.Suave:
                    t = t * t * (3 - 2 * t);
                    break;
                case Types.Muysuave:
                    t = t * t * t * (t * (t * 6f - 15f) + 10f);
                    break;
                default:
                    break;
            }
            transform.position = Vector3.Lerp(posicionInicial, posicionDeseada, t);
            yield return new WaitForEndOfFrame();
            tiempoTranscurrido += Time.deltaTime;
        }
        transform.position = posicionDeseada;
        SetPosition(x, y);
        GameManager.Instance.InitializePosition(x, y, this);
        GameManager.Instance.enEjecucion = false;
        Debug.Log("Fin de la Corutina");
        enMovimiento = false;
    }

    public enum Types
    {
        Linear,
        Senoidal,
        Cosenoidal,
        Suave,
        Muysuave
    }
    public void Corutina(int x, int y)
    {
        if (!enMovimiento)
        {
            StartCoroutine(MovePiece(x, y, GameManager.Instance.swapTime));
        }
    }

}
