using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Codigo : MonoBehaviour
{
    public int[] myArray=new int[10];
    int pares, impares, posicion;

    private void Start()
    {
        impares = 0;
        pares = 0;

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            for (int i = 0; i < myArray.Length; i++)
            {
                myArray[i] = UnityEngine.Random.Range(1, 20);
                int contador = 0;
                while (contador < i)
                {
                    if (myArray[contador] == myArray[i])
                    {
                        myArray[i] = UnityEngine.Random.Range(1, 20);
                        contador = 0;
                    }
                    contador++;

                }
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            for (int i = 0; i < myArray.Length; i++)
            {
              
                int contador1 = 0;
                int buffer=0;
                Debug.Log(contador1);
                while (contador1<myArray.Length)
                {
                    if (contador1 +1 < myArray.Length)  
                    {
                        if (myArray[contador1] > myArray[contador1 + 1])
                        {
                            buffer = myArray[contador1];
                            myArray[contador1] = myArray[contador1 + 1];
                            myArray[contador1 + 1] = buffer;
                            contador1 = 0; 
                        }
                    }
                    contador1++;
                }

                
               /* if (i - 1 >= 0)
                {
                    if (myArray[i - 1] > myArray[i])
                    {
                        int tem = myArray[i];
                        myArray[i] = myArray[i - 1];
                        myArray[i - 1] = tem;
                        i = 0;
                    }
                }*/


            }

           /* int indice = 0;
            while (indice < myArray.Length)
            {
                if (indice - 1 >= 0)
                {
                    if (myArray[indice - 1] > myArray[indice])
                    {
                        int tem = myArray[indice];
                        myArray[indice] = myArray[indice - 1];
                        myArray[indice - 1] = tem;
                        indice = 0;
                    }
                }
                indice++;
            }*/


            

        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            for (int i = 0; i < myArray.Length; i++)
            {
                /*
                int contador2 = 0;
                while (contador2<i)
                {
                    if (myArray[i] % 2 == 0)
                    {
                        pares++;
                    }
                    else
                    {
                        impares++;
                    }
                    contador2++;

                }*/

                if (myArray[i] % 2 == 0)
                {
                    pares++;
                }
                else 
                {
                    impares++;
                }
            }
            Debug.Log(impares);
            Debug.Log(pares);
        }
    }
}
