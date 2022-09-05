using System.Linq;
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
    public List<GamePiece> matchFounded;
    public bool enEjecucion;

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
        enEjecucion = false;
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
        if (IsWithInBounds(X,Y))
        {
            myPiece[X, Y] = piece;
        }
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
        if (selectTile==null&&!enEjecucion)
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
    /*public void Match()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (i + 1 < width && i - 1 >= 0 && j + 1 < height && j - 1 >= 0)
                {
                    if (myPiece[i, j].colorCode == myPiece[i + 1, j].colorCode&& myPiece[i, j].colorCode == myPiece[i - 1, j].colorCode)
                    {
                        Debug.Log($"Match:{myPiece[i,j].transform.position}, Width");
                    }
                    if (myPiece[i, j].colorCode == myPiece[i, j+1].colorCode && myPiece[i, j].colorCode == myPiece[i, j-1].colorCode)
                    {
                        Debug.Log($"Match:{myPiece[i, j].transform.position}, Height");
                    }
                }
                
            }
        }
    }*/
    bool IsWithInBounds (int x, int y)
    {
        return (x >= 0 && x < width && y >= 0 && y < height);
    }
     List<GamePiece> FindMatches(int startX, int startY, Vector3 SerchDirection, int minLength=3)
    {
        List<GamePiece> match = new List<GamePiece>();
        GamePiece startPiece = null;
        if (IsWithInBounds(startX,startY))
        {
            startPiece = myPiece[startX, startY];
        }
        if (startPiece!=null)
        {
            match.Add(startPiece);
        }
        else
        {
            return null;
        }
        int nextX;
        int nextY;
        int maxValue = (width > height) ? width : height;
        for (int i = 1; i < maxValue - 1; i++)
        {
            nextX = startX + (int)Mathf.Clamp(SerchDirection.x, -1, 1) * i;
            nextY = startY + (int)Mathf.Clamp(SerchDirection.y, -1, 1) * i;
            if (!IsWithInBounds(nextX, nextY))
            {
                break;
            }
            GamePiece nextPiece = myPiece[nextX, nextY];
            if (nextPiece.colorCode==startPiece.colorCode && !match.Contains(nextPiece))
            {
                match.Add(nextPiece);
            }
            else
            {
                break;
            }
        }
        if (match.Count >= minLength)
        {
            return match;
        }
        return null;
    }
    public List<GamePiece> FindVertical (int starX, int starY, int minLegth=3)
    {
        List<GamePiece> upList = FindMatches(starX, starY, new Vector3(0, 1f, 0));
        List<GamePiece> downList = FindMatches(starX, starY, new Vector3(0, -1f, 0));
        if (upList==null)
        {
            upList = new List<GamePiece>();
        }
        if (downList==null)
        {
            downList = new List<GamePiece>();
        }
        var combinedMatches = upList.Union(downList).ToList();
        return (combinedMatches.Count >= minLegth) ? combinedMatches : null;
    }
    public List<GamePiece> FindHorizontal(int starX, int starY, int minLegth = 3)
    {
        List<GamePiece> leftList = FindMatches(starX, starY, new Vector3(-1f, 0f, 0));
        List<GamePiece> rigthList = FindMatches(starX, starY, new Vector3(1f, 0f, 0));
        if (leftList == null)
        {
            leftList = new List<GamePiece>();
        }
        if (rigthList == null)
        {
            rigthList = new List<GamePiece>();
        }
        var combinedMatches = leftList.Union(rigthList).ToList();
        return (combinedMatches.Count >= minLegth) ? combinedMatches : null;
    }

    public void Check(int indexX, int indexY)
    {
        List<GamePiece> hList = FindHorizontal(indexX, indexY);
        List<GamePiece> vList = FindVertical(indexX, indexY);

        if (hList == null)
        {
            hList = new List<GamePiece>();
        }

        if (vList == null)
        {
            vList = new List<GamePiece>();
        }

        var AllList = hList.Union(vList).ToList();
        Debug.Log(AllList.Count);
        if (AllList.Count>=2)
        {
            foreach (GamePiece piece in AllList)
            {
                SpriteRenderer sr = myBoard[piece.indiceX, piece.indiceY].GetComponent<SpriteRenderer>();
                sr.color = myPiece[piece.indiceX, piece.indiceY].GetComponent<SpriteRenderer>().color;
            }
        }
    }
}
