using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int height, width, edge;

    public Tile[,] myBoard;
    public GameObject prefab;
    Transform board;

    public GamePiece[,] myPiece;
    public GameObject[] gamePiece;

    public GameObject mainCamenra;

    public Tile selectTile, targetTile;
    public bool enEjecucion;

    [Range(0f, 5f)]
    public float swapTime;

    [Range(0f, 5f)]
    public float collapseTime;

    [Header("Reloj")]
    [Tooltip("Tiempo iniciar en Segundos")]
    public int tInicial;
    [Tooltip("Escala del Tiempo del Reloj")]
    [Range(-10.0f, 10.0f)]
    public float escalaDeTiempo = 1;
    private float tFrameTScale = 0f;
    private float tEnSegundos = 0f;
    private float escalatiempoPausa, escalaTInicial;
    public TMP_Text reloj;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        myPiece = new GamePiece[width, height];
        myBoard = new Tile[width, height];
        NewMap();
        CameraPosition();
        PositionOnMatriz();
        enEjecucion = false;

        //Reloj durante el juego
        escalaTInicial = escalaDeTiempo;
        tEnSegundos = tInicial;
        ActualizarReloj(tInicial);

    }
    void Update()
    {
        //Reloj
        tFrameTScale = Time.deltaTime * escalaDeTiempo;
        tEnSegundos += tFrameTScale;
        ActualizarReloj(tEnSegundos);

        if (Input.GetKey(KeyCode.F1))
        {
            StartCoroutine(Restart());
        }
    }
    void ActualizarReloj(float tiempo)
    {
        int minutos = 0;
        int segundos = 0;
        //int milisegundos=0;
        string textoDelReloj;
        if (tiempo < 0) tiempo = 0;
        minutos = (int)tiempo / 60;
        segundos = (int)tiempo % 60;
        //milisegundos=(int)tiempo/1000;
        textoDelReloj = minutos.ToString("00") + ":" + segundos.ToString("00");//+ ":" + milisegundos.ToString("00");
        reloj.text = textoDelReloj;
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
        List<GamePiece> addedPieces = new List<GamePiece>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (myPiece[i, j] == null)
                {
                    GamePiece gamePiece = FillPieces(i, j);
                    addedPieces.Add(gamePiece);
                }
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
                matches = matches.Intersect(addedPieces).ToList();
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
    private GamePiece FillPieces(int x, int y)
    {
        GameObject go = Instantiate(RandomGamePiece(), new Vector3(x, y, 0), Quaternion.identity, transform);
        InitializePosition(x, y, go.GetComponent<GamePiece>());
        go.name = "Circle (" + x + "," + y + ")";
        return go.GetComponent<GamePiece>();
    }
    public void SelectTile(Tile tile)
    {
        if (selectTile == null && !enEjecucion)
        {
            selectTile = tile;
        }
    }
    public void TargetTile(Tile tile)
    {
        if (selectTile != null)
        {
            targetTile = tile;
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
            iniGp.Corutina(target.indiceX, target.indiceY, swapTime);
            finGp.Corutina(selected.indiceX, selected.indiceY, swapTime);
            yield return new WaitForSeconds(swapTime);
            List<GamePiece> selectedPieceMatches = Check(selected.indiceX, selected.indiceY);
            List<GamePiece> targetPieceMatches = Check(target.indiceX, target.indiceY);
            if (selectedPieceMatches.Count == 0 && targetPieceMatches.Count == 0)
            {
                iniGp.Corutina(selected.indiceX, selected.indiceY, swapTime);
                finGp.Corutina(target.indiceX, target.indiceY, swapTime);
            }
            else
            {
                yield return new WaitForSeconds(swapTime);
                ClearandFilltheBoard(selectedPieceMatches.Union(targetPieceMatches).ToList());
            }
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
    List<GamePiece> Check(List<GamePiece> gamePieces, int minLength = 3)//FindMatchAt
    {

        List<GamePiece> matches = new List<GamePiece>();
        foreach (GamePiece piece in gamePieces)
        {
            matches = matches.Union(Check(piece.indiceX, piece.indiceY, minLength)).ToList();
        }
        return matches;
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
        List<GamePiece> upList = FindMatches(starX, starY, new Vector3(0, 1f, 0), 2);
        List<GamePiece> downList = FindMatches(starX, starY, new Vector3(0, -1f, 0), 2);
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
        List<GamePiece> leftList = FindMatches(starX, starY, new Vector3(-1f, 0f, 0), 2);
        List<GamePiece> rigthList = FindMatches(starX, starY, new Vector3(1f, 0f, 0), 2);
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
    List<GamePiece> Check(int indexX, int indexY, int minLength = 3)
    {
        List<GamePiece> hList = FindHorizontal(indexX, indexY, minLength);
        List<GamePiece> vList = FindVertical(indexX, indexY, minLength);



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
                        myPiece[columna, j].Corutina(columna, i, collapseTime);
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
            movingPieces = movingPieces.Union(MovementPieces(columna)).ToList();
        }
        return movingPieces;
    }
    List<int> GetColumns(List<GamePiece> gamePieces)
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
            if (piece != null)
            {
                Eliminar(piece.indiceX, piece.indiceY);
            }
        }
    }
    void ClearandFilltheBoard(List<GamePiece> gamePieces)
    {
        StartCoroutine(ClearandFill(gamePieces));
    }
    IEnumerator ClearandFill(List<GamePiece> gamePieces)
    {
        enEjecucion = true;
        yield return StartCoroutine(ClearandCollpse(gamePieces));
        yield return null;
        yield return StartCoroutine(RefillRoutine());
        enEjecucion = false;
    }
    IEnumerator ClearandCollpse(List<GamePiece> gamePieces)
    {
        enEjecucion = false;
        List<GamePiece> movingPieces = new List<GamePiece>();
        List<GamePiece> matches = new List<GamePiece>();
        yield return new WaitForSeconds(0.25f);
        bool isFinished = false;
        while (!isFinished)
        {
            Eliminar(gamePieces);
            yield return new WaitForSeconds(0.25f);
            movingPieces = CollapseColumn(gamePieces);
            while (!EstaColapsado(gamePieces))
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
            matches = Check(movingPieces);
            if (matches.Count == 0)
            {
                isFinished = true;
                break;
            }
            else
            {
                yield return StartCoroutine(ClearandCollpse(matches));
            }
        }
        enEjecucion = true;
        yield return null;
    }
    IEnumerator RefillRoutine()
    {
        PositionOnMatriz();
        yield return null;
    }
    bool EstaColapsado(List<GamePiece> gamePieces)
    {
        foreach (GamePiece piece in gamePieces)
        {
            if (piece != null)
            {
                if (piece.transform.position.y - (float)piece.indiceY > 0.001f)
                {
                    return false;
                }
            }
        }
        return true;
    }
    IEnumerator Restart()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Eliminar(i, j);
            }
        }
        yield return new WaitForEndOfFrame();
        tEnSegundos = 0f;
        PositionOnMatriz();
        yield return new WaitForEndOfFrame();
    }
    public void RestartButton()
    {
        StartCoroutine(Restart());
    }
}
