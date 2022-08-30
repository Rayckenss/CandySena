using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int height, width, edge;
    public Tile[,] myBoard;
    public GamePiece[,] myPiece;
    public GameObject prefab;
    public GameObject[] gamePiece;
    public GameObject mainCamenra;
    public Tile selectTile, targetTile;

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
            mainCamenra.GetComponent<Camera>().orthographicSize = ((float)height/2)+((float)edge*2);
        }
        else
        {
            mainCamenra.GetComponent<Camera>().orthographicSize = ((((float)Screen.height*(float)width)/(float)Screen.width) / 2)+((float)edge*2);
        }
        PositionOnMatriz();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {

            PositionOnMatriz();
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
    public GameObject RandomGamePiece ()
    {
        GameObject buffer = gamePiece[Random.Range(0,gamePiece.Length)];
        return buffer;
    }
    public void InitializePosition (int X, int Y, GamePiece piece)
    {
        piece.transform.position = new Vector3(X, Y, 0);
        piece.SetPosition(X, Y);
    }
    public void PositionOnMatriz()
    {
        myPiece = new GamePiece[height, width];
        for (int j = 0; j < width; j++)
        {
            for (int i = 0; i < height; i++)
            {
                GameObject go = Instantiate(RandomGamePiece(), new Vector3(j, i, 0), Quaternion.identity, transform);
                myPiece[i, j] = go.GetComponent<GamePiece>();
                InitializePosition(i, j, go.GetComponent<GamePiece>());
                go.name = "Circle (" + i + "," + j + ")";
            }
        }
    }
    public void SelectTile(Tile tile)
    {
        if (selectTile==null)
        {
            selectTile = tile;
        }
    }
    public void TargetTile(Tile tile)
    {
        if (selectTile!=null)
        {
            targetTile = tile;
        }
    }
    public void Released()
    {
        selectTile = null;
        targetTile = null;
    }

    //Metodo para cambiar
    public void Change()
    {

    }

    bool IsNeighbour(Tile selected, Tile target)
    {
        if (Mathf.Abs(selected.transform.position.x-target.transform.position.x)==1&&selected.transform.position.y==target.transform.position.y)
        {
            return true;
        }
        if (Mathf.Abs(selected.transform.position.y - target.transform.position.y) == 1 && selected.transform.position.x == target.transform.position.x)
        {
            return true;
        }
        return false;
    }
}
