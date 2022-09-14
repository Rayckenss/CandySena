using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;

public class example : MonoBehaviour
{
    ////Alto y ancho del board
    //public int alto;
    //public int ancho;
    //public int borde;

    //public GameObject prefTile;
    //public Tile[,] m_allTiles;
    //Transform tr_Tiles;

    //public GameObject[] prefPieces;
    //GamePiece[,] m_allPieces;
    //Transform tr_GamePieces;

    //public Camera cam;

    //public Tile m_selectedTile;
    //public Tile m_targetTile;

    //public float swapTime;

    //public bool m_playerInputEnabled = true;


    //private void Start()
    //{
    //    m_allTiles = new Tile[ancho, alto];
    //    m_allPieces = new GamePiece[ancho, alto];

    //    SetUpBoard();
    //    SetUpCamere();
    //    FillBoard();
    //    // HighLightMatches();
    //}

    //public void SetUpBoard()
    //{
    //    if (tr_Tiles == null)
    //    {
    //        tr_Tiles = new GameObject().transform;
    //        tr_Tiles.parent = this.transform;
    //        tr_Tiles.name = "Tiles";
    //    }

    //    for (int i = 0; i < ancho; i++)
    //    {
    //        for (int j = 0; j < alto; j++)
    //        {
    //            GameObject go = Instantiate(prefTile, new Vector3(i, j, 0), Quaternion.identity);
    //            Tile tile = go.GetComponent<Tile>();
    //            go.name = "Tile + (" + i + "," + j + ")";
    //            go.transform.parent = tr_Tiles;
    //            tile.Init(i, j, this);
    //            m_allTiles[i, j] = tile;
    //        }
    //    }
    //}

    //public void SetUpCamere()
    //{
    //    cam.transform.position = new Vector3(((float)ancho - 1f) / 2, ((float)alto - 1f) / 2, -10f);
    //    float ortographicSizeY = ((float)alto + borde) / 2;
    //    float ortographicSizeX = (((float)ancho + borde) / 2) * ((float)Screen.height / (float)Screen.width);

    //    cam.orthographicSize = ortographicSizeX > ortographicSizeY ? ortographicSizeX : ortographicSizeY;
    //}

    //GameObject GetRandomPiece()
    //{
    //    int numeroRandom = Random.Range(0, prefPieces.Length);
    //    GameObject piesaSelccionada = prefPieces[numeroRandom];
    //    return piesaSelccionada;
    //}

    //public void SetPiece(GamePiece piesa, int x, int y)
    //{
    //    piesa.transform.position = new Vector3(x, y);
    //    piesa.transform.rotation = Quaternion.identity;
    //    piesa.SetPosition(x, y);
    //    if (InBounds(x, y))
    //    {
    //        m_allPieces[x, y] = piesa;
    //    }
    //}

    //void ReplaceWithRandom(List<GamePiece> gamePieces)
    //{
    //    foreach (GamePiece piece in gamePieces)
    //    {
    //        ClearPieceAt(piece.X_index, piece.Y_index);
    //        FillRandomAt(piece.X_index, piece.Y_index);
    //    }
    //}

    //void FillBoard()
    //{
    //    if (tr_GamePieces == null)
    //    {
    //        tr_GamePieces = new GameObject().transform;
    //        tr_GamePieces.parent = this.transform;
    //        tr_GamePieces.name = "Piezas";
    //    }

    //    List<GamePiece> addedPieces = new List<GamePiece>();

    //    for (int i = 0; i < ancho; i++)
    //    {
    //        for (int j = 0; j < alto; j++)
    //        {
    //            if (m_allPieces[i, j] == null)
    //            {
    //                GamePiece gamePiece = FillRandomAt(i, j);
    //                addedPieces.Add(gamePiece);
    //            }
    //        }
    //    }

    //    bool isFilled = false;
    //    int maxIterations = 100;
    //    int iteration = 0;

    //    while (!isFilled)
    //    {
    //        List<GamePiece> matches = FindAllMatches();
    //        if (matches.Count == 0)
    //        {
    //            isFilled = true;
    //            break;
    //        }
    //        else
    //        {
    //            matches = matches.Intersect(addedPieces).ToList();
    //            ReplaceWithRandom(matches);
    //        }

    //        if (iteration > maxIterations)
    //        {
    //            isFilled = true;
    //            Debug.LogWarning("");
    //        }

    //        iteration++;
    //    }
    //}

    //private GamePiece FillRandomAt(int _x, int _y, float falseOffsetY = 0)
    //{
    //    GameObject piesaAleatoria = Instantiate(GetRandomPiece(), Vector3.zero, Quaternion.identity);
    //    piesaAleatoria.GetComponent<GamePiece>().Init(this);
    //    SetPiece(piesaAleatoria.GetComponent<GamePiece>(), _x, _y);
    //    piesaAleatoria.transform.parent = tr_GamePieces;
    //    return piesaAleatoria.GetComponent<GamePiece>();
    //}

    //bool InBounds(int x, int y)
    //{
    //    return (x >= 0 && x < ancho && y >= 0 && y < alto);
    //}

    //public void StartTile(Tile t)
    //{
    //    if (m_selectedTile == null)
    //    {
    //        m_selectedTile = t;
    //    }
    //}

    //public void TargetTile(Tile t)
    //{
    //    if (m_selectedTile != null && IsNeighbor(m_selectedTile, t))
    //    {
    //        m_targetTile = t;
    //    }
    //}

    //public void ReleaseTile()
    //{
    //    if (m_selectedTile != null && m_targetTile != null)
    //    {
    //        SwitchTiles(m_selectedTile, m_targetTile);
    //    }

    //    m_selectedTile = null;
    //    m_targetTile = null;
    //}

    //void SwitchTiles(Tile selectedTiled, Tile targetTile)
    //{
    //    StartCoroutine(SwitchTilesRoutine(selectedTiled, targetTile));
    //}

    //IEnumerator SwitchTilesRoutine(Tile _selectedTile, Tile _targetTile)
    //{
    //    if (m_playerInputEnabled)
    //    {
    //        m_playerInputEnabled = false;
    //        GamePiece clickedPiece = m_allPieces[_selectedTile.indiceX, _selectedTile.indiceY];
    //        GamePiece targetPiece = m_allPieces[_targetTile.indiceX, _targetTile.indiceY];

    //        if (clickedPiece != null && targetPiece != null)
    //        {
    //            clickedPiece.StartMovimiento(_targetTile.indiceX, _targetTile.indiceY, swapTime);
    //            targetPiece.StartMovimiento(_selectedTile.indiceX, _selectedTile.indiceY, swapTime);

    //            yield return new WaitForSeconds(swapTime);

    //            List<GamePiece> clickedPieceMatches = FindAllMatchesAt(clickedPiece.X_index, clickedPiece.Y_index);
    //            List<GamePiece> targetPieceMatches = FindAllMatchesAt(targetPiece.X_index, targetPiece.Y_index);

    //            if (clickedPieceMatches.Count == 0 && targetPieceMatches.Count == 0)
    //            {
    //                clickedPiece.StartMovimiento(_selectedTile.indiceX, _selectedTile.indiceY, swapTime);
    //                targetPiece.StartMovimiento(_targetTile.indiceX, _targetTile.indiceY, swapTime);
    //                yield return new WaitForSeconds(swapTime);
    //                m_playerInputEnabled = true;
    //            }
    //            else
    //            {
    //                yield return new WaitForSeconds(swapTime);

    //                //HighLigthMatchesAt(clickedPiece.X_index, clickedPiece.Y_index);
    //                //HighLigthMatchesAt(targetPiece.X_index, targetPiece.Y_index);

                    
    //                ClearAndRefillBoard(clickedPieceMatches.Union(targetPieceMatches).ToList());
    //            }
    //        }
    //    }
    //}

    //bool IsNeighbor(Tile start, Tile final)
    //{
    //    if (Mathf.Abs(start.indiceX - final.indiceX) == 1 && start.indiceY == final.indiceY)
    //    {
    //        return true;
    //    }

    //    if (Mathf.Abs(start.indiceY - final.indiceY) == 1 && start.indiceX == final.indiceX)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //List<GamePiece> FindMatches(int startX, int startY, Vector2 searchDirection, int minLenght = 3)
    //{
    //    List<GamePiece> matches = new List<GamePiece>();
    //    GamePiece startPiece = null;

    //    if (InBounds(startX, startY))
    //    {
    //        startPiece = m_allPieces[startX, startY];
    //    }

    //    if (startPiece != null)
    //    {
    //        matches.Add(startPiece);
    //    }
    //    else
    //    {
    //        return null;
    //    }

    //    int nextX;
    //    int nextY;

    //    int maxValue = ancho > alto ? ancho : alto;
    //    for (int i = 1; i < maxValue - 1; i++)
    //    {
    //        nextX = startX + (int)Mathf.Clamp(searchDirection.x, -1, 1) * i;
    //        nextY = startY + (int)Mathf.Clamp(searchDirection.y, -1, 1) * i;
    //        if (!InBounds(nextX, nextY))
    //        {
    //            break;
    //        }

    //        GamePiece nextPiece = m_allPieces[nextX, nextY];

    //        if (nextPiece == null)
    //        {
    //            break;
    //        }
    //        else
    //        {
    //            if (startPiece.matchValue == nextPiece.matchValue && !matches.Contains(nextPiece))
    //            {
    //                matches.Add(nextPiece);
    //            }
    //            else
    //            {
    //                break;
    //            }
    //        }
    //    }

    //    if (matches.Count >= minLenght)
    //    {
    //        return matches;
    //    }

    //    return null;
    //}

    //List<GamePiece> VerticalMatches(int startX, int startY, int minLengt = 3)
    //{
    //    List<GamePiece> UpMatches = FindMatches(startX, startY, Vector2.up, 2);
    //    List<GamePiece> DownMatches = FindMatches(startX, startY, Vector2.down, 2);

    //    if (UpMatches == null)
    //    {
    //        UpMatches = new List<GamePiece>();
    //    }

    //    if (DownMatches == null)
    //    {
    //        DownMatches = new List<GamePiece>();
    //    }

    //    var combinedMatches = UpMatches.Union(DownMatches).ToList();
    //    return combinedMatches.Count >= minLengt ? combinedMatches : null;
    //}

    //List<GamePiece> HorizontalMatches(int startX, int startY, int minLengt = 3)
    //{
    //    List<GamePiece> rightMatches = FindMatches(startX, startY, Vector2.right, 2);
    //    List<GamePiece> leftMatches = FindMatches(startX, startY, Vector2.left, 2);

    //    if (rightMatches == null)
    //    {
    //        rightMatches = new List<GamePiece>();
    //    }

    //    if (leftMatches == null)
    //    {
    //        leftMatches = new List<GamePiece>();
    //    }

    //    var combinedMatches = rightMatches.Union(leftMatches).ToList();
    //    return combinedMatches.Count >= minLengt ? combinedMatches : null;
    //}

    //List<GamePiece> FindAllMatchesAt(List<GamePiece> gamePieces, int minLenght = 3)
    //{
    //    List<GamePiece> matches = new List<GamePiece>();
    //    foreach (GamePiece gamePiece in gamePieces)
    //    {
    //        matches = matches.Union(FindAllMatchesAt(gamePiece.X_index, gamePiece.Y_index)).ToList();
    //    }

    //    return matches;
    //}

    //private List<GamePiece> FindAllMatchesAt(int _x, int _y, int minLength = 3)
    //{
    //    List<GamePiece> horizontalMatches = HorizontalMatches(_x, _y, minLength);
    //    List<GamePiece> verticalMatches = VerticalMatches(_x, _y, minLength);

    //    if (horizontalMatches == null)
    //    {
    //        horizontalMatches = new List<GamePiece>();
    //    }

    //    if (verticalMatches == null)
    //    {
    //        verticalMatches = new List<GamePiece>();
    //    }

    //    var combinedMatches = horizontalMatches.Union(verticalMatches).ToList();
    //    return combinedMatches;
    //}

    //List<GamePiece> FindAllMatches()
    //{
    //    List<GamePiece> combinedMatches = new List<GamePiece>();
    //    for (int i = 0; i < ancho; i++)
    //    {
    //        for (int j = 0; j < alto; j++)
    //        {
    //            var matches = FindAllMatchesAt(i, j);
    //            combinedMatches = combinedMatches.Union(matches).ToList();
    //        }
    //    }

    //    return combinedMatches;
    //}

    //void HighLightTileOff(int _x, int _y)
    //{
    //    SpriteRenderer sr = m_allTiles[_x, _y].GetComponent<SpriteRenderer>();
    //    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
    //}

    //void HighLightOn(int _x, int _y, Color col)
    //{
    //    SpriteRenderer sr = m_allTiles[_x, _y].GetComponent<SpriteRenderer>();
    //    sr.color = col;
    //}

    //public void HighLightMatches()
    //{
    //    for (int i = 0; i < ancho; i++)
    //    {
    //        for (int j = 0; j < alto; j++)
    //        {
    //            HighLigthMatchesAt(i, j);
    //        }
    //    }
    //}

    //private void HighLigthMatchesAt(int _x, int _y)
    //{
    //    HighLightTileOff(_x, _y);
    //    var combinedMatches = FindAllMatchesAt(_x, _y);
    //    if (combinedMatches.Count > 0)
    //    {
    //        foreach (GamePiece piece in combinedMatches)
    //        {
    //            HighLightOn(piece.X_index, piece.Y_index,
    //                m_allPieces[piece.X_index, piece.Y_index].GetComponent<SpriteRenderer>().color);
    //        }
    //    }
    //}

    //void ClearPieceAt(int _x, int _y)
    //{
    //    GamePiece pieceClear = m_allPieces[_x, _y];
    //    if (pieceClear != null)
    //    {
    //        m_allPieces[_x, _y] = null;
    //        Destroy(pieceClear.gameObject);
    //    }

    //    HighLightTileOff(_x, _y);
    //}

    //void ClearBoard()
    //{
    //    for (int i = 0; i < ancho; i++)
    //    {
    //        for (int j = 0; j < alto; j++)
    //        {
    //            ClearPieceAt(i, j);
    //        }
    //    }
    //}

    //void ClearPieceAt(List<GamePiece> gamePieces)
    //{
    //    foreach (GamePiece piece in gamePieces)
    //    {
    //        if (piece != null)
    //        {
    //            ClearPieceAt(piece.X_index, piece.Y_index);
    //        }
    //    }
    //}

    //List<GamePiece> CollapseColumn(int column, float collapseTime = .1f)
    //{
    //    List<GamePiece> movingPieces = new List<GamePiece>();

    //    for (int i = 0; i < alto - 1; i++)
    //    {
    //        if (m_allPieces[column, i] == null)
    //        {
    //            for (int j = i + 1; j < alto; j++)
    //            {
    //                if (m_allPieces[column, j] != null)
    //                {
    //                    m_allPieces[column, j].StartMovimiento(column, i, collapseTime * (j - i));
    //                    m_allPieces[column, j] = m_allPieces[column, j];
    //                    m_allPieces[column, j].SetPosition(column, j);
    //                    if (!movingPieces.Contains(m_allPieces[column, i]))
    //                    {
    //                        movingPieces.Add(m_allPieces[column, j]);
    //                    }

    //                    m_allPieces[column, j] = null;
    //                    break;
    //                }
    //            }
    //        }
    //    }

    //    return movingPieces;
    //}

    //List<GamePiece> CollapseColumn(List<GamePiece> gamePieces)
    //{
    //    List<GamePiece> movingPieces = new List<GamePiece>();
    //    List<int> columnsToColapse = GetColumns(gamePieces);
    //    foreach (int column in columnsToColapse)
    //    {
    //        movingPieces = movingPieces.Union(CollapseColumn(column)).ToList();
    //    }

    //    return movingPieces;
    //}

    //List<int> GetColumns(List<GamePiece> gamePieces)
    //{
    //    List<int> collumnIndex = new List<int>();
    //    foreach (GamePiece piece in gamePieces)
    //    {
    //        if (!collumnIndex.Contains(piece.X_index))
    //        {
    //            collumnIndex.Add(piece.X_index);
    //        }
    //    }

    //    return collumnIndex;
    //}

    //void ClearAndRefillBoard(List<GamePiece> gamePieces)
    //{
    //    StartCoroutine(ClearAndRefillBoardRoutine(gamePieces));
    //}

    //IEnumerator ClearAndRefillBoardRoutine(List<GamePiece> gamePieces)
    //{
    //    m_playerInputEnabled = false;
    //    yield return StartCoroutine(ClearAndCollapseRoutine(gamePieces));
    //    yield return null;
    //    yield return StartCoroutine(RefillRoutine());
    //    m_playerInputEnabled = true;
    //}

    //IEnumerator ClearAndCollapseRoutine(List<GamePiece> gamePieces)
    //{
    //    List<GamePiece> movingPieces = new List<GamePiece>();
    //    List<GamePiece> matches = new List<GamePiece>();

    //    yield return new WaitForSeconds(.25f);

    //    bool isFinished = false;

    //    while (!isFinished)
    //    {
    //        ClearPieceAt(gamePieces);
    //        yield return new WaitForSeconds(.25f);
    //        movingPieces = CollapseColumn(gamePieces);
    //        while (!IsCollapsed(gamePieces))
    //        {
    //            yield return new WaitForEndOfFrame();
    //        }

    //        yield return new WaitForSeconds(.5f);
    //        matches = FindAllMatchesAt(movingPieces);

    //        if (matches.Count == 0)
    //        {
    //            isFinished = true;
    //            break;
    //        }
    //        else
    //        {
    //            yield return StartCoroutine(ClearAndCollapseRoutine(matches));
    //        }
    //    }

    //    yield return null;
    //}

    //IEnumerator RefillRoutine()
    //{
    //    FillBoard();
    //    yield return null;
    //}

    //bool IsCollapsed(List<GamePiece> gamePieces)
    //{
    //    foreach (GamePiece piece in gamePieces)
    //    {
    //        if (piece != null)
    //        {
    //            if (piece.transform.position.y - (float)piece.Y_index > 0.001f)
    //            {
    //                return false;
    //            }
    //        }
    //    }

    //    return true;
    //}
}
