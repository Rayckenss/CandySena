using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kodigo : MonoBehaviour
{
    public int length, width;
    public GameObject prefab;
    string pattern;

    void Start()
    {
    }
    private void Update()
    {
        //X pattern
        if (Input.GetButtonDown("Jump"))
        {
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i - j == 0)
                    {

                        Vector3 pos = new Vector3(i, 0, j);
                        GameObject go = Instantiate(prefab, pos, Quaternion.identity);
                    }
                    if (i + j == length - 1)
                    {
                        Vector3 pos = new Vector3(i, 0, j);
                        GameObject go = Instantiate(prefab, pos, Quaternion.identity);
                    }
                }
            }
        }
        //empty cube
        if (Input.GetKeyDown(KeyCode.Q))
        {
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == 0)
                    {
                        Vector3 pos = new Vector3(i, 0, j);
                        GameObject go = Instantiate(prefab, pos, Quaternion.identity);
                    }
                    else if (j == 0)
                    {
                        Vector3 pos = new Vector3(i, 0, j);
                        GameObject go = Instantiate(prefab, pos, Quaternion.identity);
                    }
                    else if (i == length - 1)
                    {
                        Vector3 pos = new Vector3(i, 0, j);
                        GameObject go = Instantiate(prefab, pos, Quaternion.identity);
                    }
                    else if (j == width - 1)
                    {
                        Vector3 pos = new Vector3(i, 0, j);
                        GameObject go = Instantiate(prefab, pos, Quaternion.identity);
                    }

                }
            }
        }
        //full cube
        if (Input.GetKeyDown(KeyCode.W))
        {
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Vector3 pos = new Vector3(i, 0, j);
                    GameObject go = Instantiate(prefab, pos, Quaternion.identity);
                }
            }
        }
        //Wall
        if (Input.GetKeyDown(KeyCode.E))
        {
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Vector3 pos = new Vector3(i, j, 0);
                    GameObject go = Instantiate(prefab, pos, Quaternion.identity);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            for (int i = 0; i < length; i++)
            {

                for (int j = 0; j < width; j++)
                {
                    for (int k = 0; k < length; k++)
                    {
                        Vector3 pos = new Vector3(i, k, j);
                        GameObject go = Instantiate(prefab, pos, Quaternion.identity);
                    }
                }
            }
        }
        
        
    }

}
