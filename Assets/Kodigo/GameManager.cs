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

    [Range(0f, 5f)]
    public float swapTime;

    private void Awake()
    {
        if (Instance == null)
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
    public void NewMap()
    {
        myBoard = new Tile[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject go = Instantiate(prefab, new Vector3(i, j, 0), Quaternion.identity, transform);
                myBoard[i, j] = go.GetComponent<Tile>();
                myBoard[i, j].Indice(i, j);
                go.name = "Tile (" + i + "," + j + ")";
            }
        }
    }
    public void PositionOnMatriz()
    {
        myPiece = new GamePiece[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                FillPieces(i, j);
            }
        }
        bool isFilled = false;
        int maxInterations = 100;
        int interations = 0;
        while (!isFilled && interations < maxInterations)
        {
            List<GamePiece> matches = EncontrarParejas();
            if (matches.Count == 0)
            {
                isFilled = true;
                break;
            }
            else
            {
                Remplazar(matches);
            }
            interations++;
        }
    }
    public void InitializePosition(int X, int Y, GamePiece piece)
    {
        piece.transform.position = new Vector3(X, Y, 0);
        piece.SetPosition(X, Y);
        if (IsWithInBounds(X, Y))
        {
            myPiece[X, Y] = piece;
        }
    }
    public GameObject RandomGamePiece()
    {
        int numeroR = Random.Range(0, gamePiece.Length);
        GameObject buffer = gamePiece[numeroR];
        buffer.GetComponent<GamePiece>().SetPrefab(numeroR);
        return buffer;
    }
    void Remplazar(List<GamePiece> lista)
    {
        Eliminar(lista);
        foreach (GamePiece piece in lista)
        {
            FillPieces(piece.indiceX, piece.indiceY);
        }
    }
    void FillPieces(int x, int y)
    {
        GameObject go = Instantiate(RandomGamePiece(), new Vector3(x, y, 0), Quaternion.identity, transform);
        InitializePosition(x, y, go.GetComponent<GamePiece>());
        go.name = "Circle (" + x + "," + y + ")";
    }
    public void SelectTile(Tile tile)
    {
        if (selectTile == null && !enEjecucion)
        {
            selectTile = tile;
            Debug.Log("Selecciona Tile");
        }
    }
    public void TargetTile(Tile tile)
    {
        if (selectTile != null)
        {
            targetTile = tile;
            Debug.Log("Selecciona Objetivo");
        }
    }
    public void Released()
    {
        if (selectTile != null && targetTile != null && IsNeighbour(selectTile, targetTile))
        {
            SwitchPieces(selectTile, targetTile);
        }
        if (selectTile != null || targetTile != null)
        {
            selectTile = null;
            targetTile = null;
        }

    }
    bool IsNeighbour(Tile selected, Tile target)
    {
        if (Mathf.Abs(selected.transform.position.x - target.transform.position.x) == 1 && selected.transform.position.y == target.transform.position.y)
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
        StartCoroutine(SwitchTilesRoutine(selected, target));
    }
    IEnumerator SwitchTilesRoutine(Tile selected, Tile target)
    {
        GamePiece iniGp = myPiece[selected.indiceX, selected.indiceY];
        GamePiece finGp = myPiece[target.indiceX, target.indiceY];
        if (iniGp != null && finGp != null)
        {
            iniGp.Corutina(target.indiceX, target.indiceY);
            finGp.Corutina(selected.indiceX, selected.indiceY);
            yield return new WaitForSeconds(swapTime);
            List<GamePiece> selectedPieceMatches = Check(selected.indiceX, selected.indiceY);
            List<GamePiece> targetPieceMatches = Check(target.indiceX, target.indiceY);
            if (selectedPieceMatches.Count == 0 && targetPieceMatches.Count == 0)
            {
                iniGp.Corutina(selected.indiceX, selected.indiceY);
                finGp.Corutina(target.indiceX, target.indiceY);
            }
            else
            {
                yield return new WaitForSeconds(swapTime);
                Eliminar(selectedPieceMatches);
                Eliminar(targetPieceMatches);
            }
            //Prender(selected.indiceX, selected.indiceY);
            //Prender(target.indiceX, target.indiceY);
        }
    }
    bool IsWithInBounds(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0 && y < height);
    }
    List<GamePiece> FindMatches(int startX, int startY, Vector3 SerchDirection, int minLength = 3)
    {
        List<GamePiece> match = new List<GamePiece>();
        GamePiece startPiece = null;
        if (IsWithInBounds(startX, startY))
        {
            startPiece = myPiece[startX, startY];
        }
        if (startPiece != null)
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
            if (nextPiece == null)
            {
                break;
            }
            else
            {
                if (nextPiece.colorCode == startPiece.colorCode && !match.Contains(nextPiece))
                {
                    match.Add(nextPiece);
                }
                else
                {
                    break;
                }
            }
        }
        if (match.Count >= minLength)
        {
            return match;
        }
        return null;
    }
    List<GamePiece> EncontrarParejas()
    {
        List<GamePiece> combinedmatches = new List<GamePiece>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var matches = Check(i, j);
                combinedmatches = combinedmatches.Union(matches).ToList();
            }
        }
        return combinedmatches;
    }
    public List<GamePiece> FindVertical(int starX, int starY, int minLegth = 3)
    {
        List<GamePiece> upList = FindMatches(starX, starY, new Vector3(0, 1f, 0));
        List<GamePiece> downList = FindMatches(starX, starY, new Vector3(0, -1f, 0));
        if (upList == null)
        {
            upList = new List<GamePiece>();
        }
        if (downList == null)
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
    public List<GamePiece> Check(int indexX, int indexY)
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
        return AllList;
    }
    List<GamePiece> MovementPieces(int columna)
    {
        List<GamePiece> movingPieces = new List<GamePiece>();
        for (int i = 0; i < height - 1; i++)
        {
            if (myPiece[columna, i] == null)
            {
                for (int j = i + 1; j < height; j++)
                {
                    if (myPiece[columna, j] != null)
                    {
                        myPiece[columna, j].Corutina(columna, i);
                        myPiece[columna, i] = myPiece[columna, j];
                        myPiece[columna, i].SetPosition(columna, i);
                        if (!movingPieces.Contains(myPiece[columna, i]))
                        {
                            movingPieces.Add(myPiece[columna, i]);
                        }
                        myPiece[columna, j] = null;
                        break;
                    }
                }
            }
        }
        return movingPieces;
    }
    List<GamePiece> CollapseColumn(List<GamePiece> gamePieces)
    {
        List<GamePiece> movingPieces = new List<GamePiece>();
        List<int> columnsToCollapse = GetColumns(gamePieces);
        foreach (int columna in columnsToCollapse)
        {
            movingPieces = movingPieces.Union(gamePieces).ToList();
        }
    }
    List<int>GetColumns(List<GamePiece> gamePieces)
    {
        List<int> indiceColumnas = new List<int>();
        foreach (GamePiece piece in gamePieces)
        {
            if (!indiceColumnas.Contains(piece.indiceX))
            {
                indiceColumnas.Add(piece.indiceX);
            }
        }
        return indiceColumnas;
    }
    void ApagarLuces(int x, int y)
    {
        SpriteRenderer sr = myBoard[x, y].GetComponent<SpriteRenderer>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
    }
    void PrenderLuces(int x, int y, Color color)
    {
        SpriteRenderer sr = myBoard[x, y].GetComponent<SpriteRenderer>();
        sr.color = color;
    }
    public void PrenderResultados()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Check(i, j);
            }
        }
    }
    public void Prender(int x, int y)
    {
        ApagarLuces(x, y);
        var combinedmatches = Check(x, y);
        if (combinedmatches.Count > 0)
            foreach (GamePiece piece in combinedmatches)
            {
                PrenderLuces(piece.indiceX, piece.indiceY, myPiece[x, y].GetComponent<SpriteRenderer>().color);
            }
    }
    void Eliminar(int x, int y)
    {
        GamePiece eliminarpiece = myPiece[x, y];
        if (eliminarpiece != null)
        {
            myPiece[x, y] = null;
            Destroy(eliminarpiece.gameObject);
        }
        ApagarLuces(x, y);
    }
    void Eliminar(List<GamePiece> gamePiece)
    {
        foreach (GamePiece piece in gamePiece)
        {
            Eliminar(piece.indiceX, piece.indiceY);
        }
    }
}
