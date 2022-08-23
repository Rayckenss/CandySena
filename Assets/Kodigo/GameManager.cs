using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int height, width;
    public Tile[,] myBoard;
    public GameObject prefab;
    public GameObject mainCamenra;

    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        NewMap();
        mainCamenra.transform.position = new Vector3(((float)width / 2)-0.5f, ((float)height / 2)-0.5f, mainCamenra.transform.position.z);
        if (width<height)
        {
            mainCamenra.GetComponent<Camera>().orthographicSize = ((float)height/2);
        }
        else
        {
            mainCamenra.GetComponent<Camera>().orthographicSize = ((((float)Screen.height*(float)width)/(float)Screen.width) / 2);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            NewMap();
        }
    }
    public void NewMap()
    {
        myBoard = new Tile[height, width];
        for (int j = 0; j < width; j++)
        {
            for (int i = 0; i < height; i++)
            {
                GameObject go = Instantiate(prefab, new Vector3(j, i,0), Quaternion.identity,transform);
                myBoard[i, j] = go.GetComponent<Tile>();
                myBoard[i, j].Indice(i, j);
                go.name = "Tile (" + i + "," + j + ")";
            }
        }
    }
}
