using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public GameObject aviso;
    bool interaction;
    public GameObject gameManager;
    private void Start()
    {

        StartCoroutine(RutinaTutorial());
    }
    private void Update()
    {
        if (Input.touchCount > 0||Input.GetMouseButtonDown(0))
        {
            interaction = true;
        }
    }
    void SetBasicUI(bool bl, string text)
    {
        aviso.GetComponentInChildren<TMP_Text>().text = text;
        aviso.SetActive(bl);
    }

    IEnumerator RutinaTutorial()
    {
        string primerAviso = "Bienvenido a mi juego de frutitas";
        SetBasicUI(true, primerAviso);
        interaction = false;
        yield return new WaitForEndOfFrame();
        while (!interaction)
        {
            yield return null;
        }
        interaction = false;
        string segundoAviso = "Espero que te guste este juego, es muy facil de clon.. Jugar";
        SetBasicUI(true, segundoAviso);
        yield return new WaitForEndOfFrame();
        while (!interaction)
        {
            yield return null;
        }
        interaction = false;
        string tercerAviso = "Tu objetivo sera acumular el mayor numero de puntos antes de que se acabe el tiempo";
        SetBasicUI(true, tercerAviso);
        yield return new WaitForEndOfFrame();
        while (!interaction)
        {
            yield return null;
        }
        interaction = false;
        SetBasicUI(false, "");
        gameManager.SetActive(true);
        yield return null;
    }
}
