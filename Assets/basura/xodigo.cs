using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xodigo : MonoBehaviour
{
    public static xodigo Instance;
    public GameObject[,] matriz;
    public int filas, columnas, parejas;
    public GameObject prefab;
    string filastr;

    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        matriz = new GameObject[filas, columnas];
        parejas = 0;
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int j = 0; j < columnas; j++)
            {
                filastr = null;
                for (int i = 0; i < filas; i++)
                {
                    GameObject go = Instantiate(prefab, new Vector3(j, 0, i), Quaternion.identity);
                    matriz[i, j] =go;
                    //filastr += matriz[i, j].ToString()+", ";

                }
                //Debug.Log(filastr);
            }
        }
        /* if (Input.GetKeyDown(KeyCode.Q))
         {
             for (int j = 0; j < columnas; j++)
             {
                 for (int i = 0; i < filas; i++)
                 {
                     if (j-1>=0)
                     {
                         if (matriz[j-1,i]==matriz[j,i])
                         {
                             parejas++;
                         }
                     }

                 }
             }
             Debug.Log(parejas);
             parejas = 0;
         }*/
        if (Input.GetKeyDown(KeyCode.A))
        {

        }
        
    }
    public void ChangeColors(int indiceX, int indiceY)
    {

        
            matriz[indiceX,indiceY].GetComponent<Renderer>().material.color = Color.white;
            if (indiceX+1<filas)
            {
               // matriz[indiceX+1,indiceY].GetComponent<Renderer>().material.color = Color.white;
                matriz[indiceX+1, indiceY].GetComponent<cambiarcolor>().changeColor=true;
            }
            if (indiceX-1>=0)
            {
                //matriz[indiceX-1,indiceY].GetComponent<Renderer>().material.color = Color.white;
                matriz[indiceX-1, indiceY].GetComponent<cambiarcolor>().changeColor=true;
            }
            if (indiceY+1<columnas)
            { 
               // matriz[indiceX,indiceY+1].GetComponent<Renderer>().material.color = Color.white;
                matriz[indiceX, indiceY+1].GetComponent<cambiarcolor>().changeColor = true;
            }
            if (indiceY - 1 >= 0)
            {
                //matriz[indiceX, indiceY - 1].GetComponent<Renderer>().material.color = Color.white;
                matriz[indiceX, indiceY-1].GetComponent<cambiarcolor>().changeColor=true;
            }
            
        
    }
  
}
