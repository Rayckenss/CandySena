using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public int indiceX, indiceY;
    [Range(0f, 5f)]
    public float time;
    public bool ejecucion = true;
    public Types tipos;
    
    public void SetPosition(int X, int Y)
    {
        indiceX = X;
        indiceY = Y;
    }
    IEnumerator MovePiece(Vector3 posicionDeseada, float tiempoDeAccion)
    {
        ejecucion = false;
        Vector3 posicionInicial = transform.position;
        Debug.Log($"Start: {posicionInicial}, Desire: {posicionDeseada}");
        float tiempoTranscurrido = 0f;
        while(Vector3.Distance(transform.position,posicionDeseada)>0.01f)
        {
            Debug.Log("Entra a la Corutina");
            float t =tiempoTranscurrido / tiempoDeAccion;
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
        Debug.Log("Fin de la Corutina");
        SetPosition((int)posicionDeseada.x, (int)posicionDeseada.y);
        ejecucion = true;
    }
    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.RightArrow)&&ejecucion)
        {
            StartCoroutine(MovePiece(new Vector3((int)transform.position.x + 1, (int)transform.position.y, (int)transform.position.z), time));
            Debug.Log("llamr corutina");
        }*/
    }
    public enum Types
    {
        Linear,
        Senoidal,
        Cosenoidal,
        Suave,
        Muysuave
    }
    
}
