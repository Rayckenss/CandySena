using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prueba : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(PruebaCorutina());
    }
    IEnumerator PruebaCorutina()
    {
        Debug.Log("hola");
        yield return new WaitForSeconds(5f);
        Debug.Log("adios");
    }
}
