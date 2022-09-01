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
        CameraPosition();
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
        myBoard = new Tile[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject go = Instantiate(prefab, new Vector3(i, j,0), Quaternion.identity,transform);
                myBoard[i, j] = go.GetComponent<Tile>();
                myBoard[i, j].Indice(i, j);
                go.name = "Tile (" + i + "," + j + ")";
            }
        }
    }
    void CameraPosition()
    {
        mainCamenra.transform.position = new Vector3(((float)width / 2) - 0.5f, ((float)height / 2) - 0.5f, mainCamenra.transform.position.z);
        if (width < height)
        {
            mainCamenra.GetComponent<Camera>().orthographicSize = ((float)height / 2) + ((float)edge * 2);
        }
        else
        {
            mainCamenra.GetComponent<Camera>().orthographicSize = ((((float)Screen.height * (float)width) / (float)Screen.width) / 2) + ((float)edge * 2);
        }
    }
    public GameObject RandomGamePiece ()
    {
        int numeroR = Random.Range(0, gamePiece.Length);
        GameObject buffer = gamePiece[numeroR];
        buffer.GetComponent<GamePiece>().SetPrefab(numeroR);
        return buffer;
    }
    public void InitializePosition (int X, int Y, GamePiece piece)
    {
        piece.transform.position = new Vector3(X, Y, 0);
        piece.SetPosition(X, Y);
        myPiece[X, Y] = piece;
    }
    public void PositionOnMatriz()
    {
        myPiece = new GamePiece[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject go = Instantiate(RandomGamePiece(), new Vector3(i,j, 0), Quaternion.identity, transform);
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
            Debug.Log("Selecciona Tile");
        }
    }
    public void TargetTile(Tile tile)
    {
        if (selectTile!=null)
        {
            targetTile = tile;
            Debug.Log("Selecciona Objetivo");
        }
    }
    public void Released()
    {
        if (selectTile != null && targetTile != null&&IsNeighbour(selectTile,targetTile))
        {
            SwitchPieces(selectTile, targetTile);
        }

        if (selectTile != null || targetTile != null)
        { 
            selectTile = null;
           targetTile = null;
            Debug.Log("libera");
        }
    }
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
    public void SwitchPieces(Tile selected, Tile target)
    {
        GamePiece iniGp = myPiece[selected.indiceX, selected.indiceY];
        GamePiece finGp = myPiece[target.indiceX, target.indiceY];
        iniGp.Corutina(target.indiceX, target.indiceY);
        finGp.Corutina(selected.indiceX, selected.indiceY);
    }
    public void Match()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (i+1<width&&i-1>=0&& j+1 < height && j-1 >= 0)
                {
                    if (myPiece[i, j].colorCode == myPiece[i + 1, j].colorCode&& myPiece[i, j].colorCode == myPiece[i - 1, j].colorCode)
                    {
                        Debug.Log($"Match:{myPiece[i,j].transform.position}");
                    }
                    if (myPiece[i, j].colorCode == myPiece[i, j+1].colorCode && myPiece[i, j].colorCode == myPiece[i, j-1].colorCode)
                    {
                        Debug.Log($"Match:{myPiece[i, j].transform.position}");
                    }
                }
                
            }
        }
    }
}
