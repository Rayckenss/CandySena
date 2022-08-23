using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cambiarcolor : MonoBehaviour
{
      public bool changeColor = false;
    private void OnMouseDown()
    {
        changeColor = true;
        //xodigo.Instance.ChangeColors((int)transform.position.z, (int)transform.position.x);

    }
    private void Update()
    {
        if (changeColor)
        {
            xodigo.Instance.ChangeColors((int)transform.position.z, (int)transform.position.x);
        }
    }
}
