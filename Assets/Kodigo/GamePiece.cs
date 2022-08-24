using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public int indiceX, indiceY;
    [Range(0f, 10f)]
    public float time;

    public bool ejecucion = true;

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
            float t = tiempoTranscurrido / tiempoDeAccion;
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
        if (Input.GetKeyDown(KeyCode.Q)&&ejecucion)
        {
            StartCoroutine(MovePiece(new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), time));
            Debug.Log("llamr corutina");
        }
    }
    
}
