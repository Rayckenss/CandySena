using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Parametros de juego")]
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
    float puntuacion;
    public static bool estaPausado = false;
    float multi = 1;
    public Levels nivel;
    int goal;
    float timeGoal;

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

    [Header("Interfaz Gráfica")]
    public Image sound;
    public Image pause;
    public Sprite pauseOn, PauseOff;
    public Sprite soundOn, soundOff;
    public TMP_Text puntajeEnPantalla;
    public GameObject menuPausa;
    public GameObject banana;
    public ParticleSystem flowers;
    public GameObject aviso;


    // UNITY--------------------------------------------------------------
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
        FillPicesAt();
        enEjecucion = false;
        puntuacion = 0;
        estaPausado = false;
        pause.sprite = estaPausado ? pauseOn : PauseOff;
        menuPausa.SetActive(false);
        banana.SetActive(false);

        //Reloj durante el juego
        escalaTInicial = escalaDeTiempo;
        tEnSegundos = tInicial;
        ActualizarReloj(tInicial);
        //Music
        sound.sprite = OptionManager.music ? soundOff : soundOn;
        GetComponent<AudioSource>().mute = OptionManager.music;

        //configurar niveles
        switch (nivel)
        {
            case Levels.Level1:
                goal = 1000;
                timeGoal = 120f;
                tInicial = 120;
                break;
            case Levels.Level2:
                goal = 1500;
                timeGoal = 240;
                tInicial = 240;
                break;
            case Levels.Level3:
                goal = 2000;
                timeGoal = 360f;
                tInicial = 360;
                break;
            case Levels.Level4:
                goal = 2500;
                timeGoal = 420f;
                tInicial = 420;
                break;
            case Levels.Level5:
                goal = 3000;
                timeGoal = 540f;
                tInicial = 540;
                break;
            case Levels.infinito:

                break;
            default:
                break;
        }
    }
    void Update()
    {
        if (estaPausado)
        {
            return;
        }
        //Reloj
        tFrameTScale = Time.deltaTime * escalaDeTiempo;
        tEnSegundos += tFrameTScale;
        ActualizarReloj(tEnSegundos);

        if (Input.GetKey(KeyCode.F1))
        {
            StartCoroutine(Restart());
        }
    }
    //INTERFAZ--------------------------------------------------------------
    void ActualizarReloj(float tiempo)  //Metodo encargado del funcionamiento del reloj del juego
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
    void CameraPosition()  //Metodo encargado en posicionar la camara de juego
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
    IEnumerator Restart()  //Rutina que se encarga de reiniciar el tablero de juego
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Remove(i, j);
            }
        }
        yield return new WaitForEndOfFrame();
        switch (nivel)
        {
            case Levels.Level1:
                tEnSegundos = 120f;
                escalaDeTiempo = -1;
                break;
            case Levels.Level2:
                tEnSegundos = 120f;
                escalaDeTiempo = -1;
                break;
            case Levels.Level3:
                tEnSegundos = 120f;
                escalaDeTiempo = -1;
                break;
            case Levels.Level4:
                tEnSegundos = 120f;
                escalaDeTiempo = -1;
                break;
            case Levels.Level5:
                tEnSegundos = 120f;
                escalaDeTiempo = -1;
                break;
            case Levels.infinito:
                tEnSegundos = 0;
                escalaDeTiempo = 1;
                break;
            default:
                break;
        }
        puntuacion = 0;
        puntajeEnPantalla.text = "0000/0000";
        FillPicesAt();
        estaPausado = true;
        PauseButton();
        pause.sprite = estaPausado ? pauseOn : PauseOff;
        yield return new WaitForEndOfFrame();
    }
    public void RestartButton()  //Metodo que se encarga de reiniciar el tablero de juego
    {
        StartCoroutine(Restart());
    }
    public void SoundButton()  //Metodo encargado de configurar el sonido del juego
    {
        OptionManager.music = !OptionManager.music;
        sound.sprite = OptionManager.music ? soundOff : soundOn;
        GetComponent<AudioSource>().mute = OptionManager.music;
    }
    public void ExitButton()  //Metodo encargado del boton de salida
    {
        SceneManager.LoadScene(0);
    }
    public void PauseButton()  //Metodo encargado de el boton de pausa
    {
        estaPausado = !estaPausado;
        if (estaPausado)
        {
            enEjecucion = true;
            menuPausa.SetActive(true);
            banana.SetActive(true);
        }
        else
        {
            enEjecucion = false;
            menuPausa.SetActive(false);
            banana.SetActive(false);
        }
        pause.sprite = estaPausado ? pauseOn : PauseOff;
    }

    //JUEGO --------------------------------------------------------------
    public void NewMap()  // Crea la tabla de juego
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
    public void FillPicesAt()  //Instancia y llenas las piezas de juego
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
                    foreach (GamePiece piece in addedPieces)
                    {
                        GameObject Ps = Instantiate(flowers, new Vector3(piece.indiceX, piece.indiceY, 0), Quaternion.identity).gameObject;
                        Destroy(Ps, 1.2f);
                    }
                }
            }
        }
        bool isFilled = false;
        int maxInterations = 100;
        int interations = 0;
        while (!isFilled)
        {
            List<GamePiece> matches = FindMatchesAt();
            if (matches.Count == 0)
            {
                isFilled = true;
                break;
            }
            else
            {
                matches = matches.Intersect(addedPieces).ToList();
                ReplacePiece(matches);
            }
            if (interations > maxInterations)
            {
                isFilled = true;
                List<GamePiece> parejassueltas = FindMatchesAt();
                if (parejassueltas.Count >= 0)
                {
                    ReplacePiece(parejassueltas);
                }
            }
            interations++;
        }
    }
    public void InitializePosition(int X, int Y, GamePiece piece)  //Configura la posicion de las piezas en la matriz de la tabla del juego
    {
        piece.transform.position = new Vector3(X, Y, 0);
        piece.SetPosition(X, Y);
        if (IsWithInBounds(X, Y))
        {
            myPiece[X, Y] = piece;
        }
    }
    public GameObject RandomGamePiece()  //Crea una ficha aleatoria
    {
        int numeroR = Random.Range(0, gamePiece.Length);
        GameObject buffer = gamePiece[numeroR];
        buffer.GetComponent<GamePiece>().SetPrefab(numeroR);
        return buffer;
    }
    void ReplacePiece(List<GamePiece> lista)  //Reemplaza las fichas vacias
    {
        foreach (GamePiece piece1 in lista)
        {
            Points(0);
            Remove(piece1.indiceX, piece1.indiceY);
        }
        foreach (GamePiece piece in lista)
        {
            FillPieces(piece.indiceX, piece.indiceY);
        }
    }
    private GamePiece FillPieces(int x, int y) //Instancia o crea las piezas de juego en la tabla
    {
        GameObject go = Instantiate(RandomGamePiece(), new Vector3(x, y, 0), Quaternion.identity, transform);
        InitializePosition(x, y, go.GetComponent<GamePiece>());
        go.name = "Circle (" + x + "," + y + ")";
        return go.GetComponent<GamePiece>();
    }
    public void SelectTile(Tile tile)  //Selecciona la ficha de juego con la cual se interactua
    {
        if (selectTile == null && !enEjecucion)
        {
            selectTile = tile;
        }
    }
    public void TargetTile(Tile tile)  //Selecciona la ficha de juego con la cual se realiza las interacciones
    {
        if (selectTile != null)
        {
            targetTile = tile;
        }
    }
    public void Released()  //Suelta las fichas seleccionadas para poder seguir avanzando en el juego
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
    bool IsNeighbour(Tile selected, Tile target)  //Define si las fichas a interactuar son vecinos
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
    public void SwitchPieces(Tile selected, Tile target)  //Metodo para hacer cambio de fichas
    {
        StartCoroutine(SwitchTilesRoutine(selected, target));
    }
    IEnumerator SwitchTilesRoutine(Tile selected, Tile target)  //Rutina para hacer cambio de fichas
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
                if (IsFigure(selected.indiceX, selected.indiceY) || IsFigure(target.indiceX, target.indiceY))
                {
                    ClearandFilltheBoard(selectedPieceMatches.Union(targetPieceMatches).ToList(), 10, 5);
                }
                else
                {
                    ClearandFilltheBoard(selectedPieceMatches.Union(targetPieceMatches).ToList(), 10);
                }
            }
        }
    }
    bool IsWithInBounds(int x, int y)  //Metodo que define el limite de la tabla de juego
    {
        return (x >= 0 && x < width && y >= 0 && y < height);
    }
    List<GamePiece> FindMatches(int startX, int startY, Vector3 SerchDirection, int minLength = 3)  //Lista que busca fichas que hacen pareja
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
    List<GamePiece> Check(List<GamePiece> gamePieces, int minLength = 3)  //Lista que busca coincidencias de fichas
    {
        List<GamePiece> matches = new List<GamePiece>();
        foreach (GamePiece piece in gamePieces)
        {
            matches = matches.Union(Check(piece.indiceX, piece.indiceY, minLength)).ToList();
        }
        return matches;
    }
    List<GamePiece> Check(int indexX, int indexY, int minLength = 3)  //Lista que une las coincidencias de posiciones verticales y horizontales
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
    bool IsFigure(int indexX, int indexY, int minLength = 3)  //Metodo que define cuando se esta haciendo una figura en las coincidencias
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
        if (hList.Count == 0 && vList.Count != 0)
        {
            return false;
        }
        if (hList.Count != 0 && vList.Count == 0)
        {
            return false;
        }
        if (hList.Count != 0 && vList.Count != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    List<GamePiece> FindMatchesAt()  //Lista que busca coincidencias en toda la tabla
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
    public List<GamePiece> FindVertical(int starX, int starY, int minLegth = 3)  //Encuentra coincidencias en posicion vertical
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
    public List<GamePiece> FindHorizontal(int starX, int starY, int minLegth = 3)  //Encuentra coincidencias en posicion horizontal
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
    List<GamePiece> MovementPieces(int columna)  //Lista que reconoce las fichas que estan en movimiento en la tabla de juego
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
    List<GamePiece> CollapseColumn(List<GamePiece> gamePieces)  //Lista que reconoce cuales son las columnas en las cuales se estan eliminando fichas
    {
        List<GamePiece> movingPieces = new List<GamePiece>();
        List<int> columnsToCollapse = GetColumns(gamePieces);
        foreach (int columna in columnsToCollapse)
        {
            movingPieces = movingPieces.Union(MovementPieces(columna)).ToList();
        }
        return movingPieces;
    }
    List<int> GetColumns(List<GamePiece> gamePieces) //Lista que crea un indice de las columnas en las cuales se estan eliminando fichas
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
    void Remove(int x, int y)  //Elimina las fichas en unas coordenadas indicadas
    {
        GamePiece eliminarpiece = myPiece[x, y];
        if (eliminarpiece != null)
        {
            myPiece[x, y] = null;
            Destroy(eliminarpiece.gameObject);
        }
    }
    void Remove(List<GamePiece> gamePiece)  //Eliminas las fichas en una lista dada
    {
        foreach (GamePiece piece in gamePiece)
        {
            if (piece != null)
            {
                Remove(piece.indiceX, piece.indiceY);
            }
        }
    }
    void ClearandFilltheBoard(List<GamePiece> gamePieces, int point, float multiplicador = 1)  //Metodo que se usa para eliminar fichas y rellenarlas (Mecanica Principal del juego)
    {
        StartCoroutine(ClearandFill(gamePieces, point, multiplicador));
    }
    IEnumerator ClearandFill(List<GamePiece> gamePieces, int point, float multiplicador = 1)  //Rutina que se encarga de inicial las corutinas que conforman las mecanicas del juego
    {
        enEjecucion = true;
        List<GamePiece> matches = gamePieces;
        do
        {
            yield return StartCoroutine(ClearandCollpse(matches, point, multiplicador));
            multi = 1;
            yield return null;
            yield return StartCoroutine(RefillRoutine());
            matches = FindMatchesAt();
            yield return new WaitForSeconds(.5f);

        } while (matches.Count != 0);
        enEjecucion = false;
    }
    IEnumerator ClearandCollpse(List<GamePiece> gamePieces, int points, float multiplicador = 1)  //Se encarga de eliminar las piezas que hacen coincidencias y revisar que al colapsar no se creen mas coincidencias para continuar
    {
        enEjecucion = true;
        List<GamePiece> movingPieces = new List<GamePiece>();
        List<GamePiece> matches = new List<GamePiece>();
        yield return new WaitForSeconds(0.25f);
        bool isFinished = false;

        while (!isFinished)
        {
            while (estaPausado)
            {
                yield return null;
            }
            foreach (GamePiece piece in gamePieces)
            {
                Points(points, multiplicador);
                Remove(piece.indiceX, piece.indiceY);
            }
            yield return new WaitForSeconds(0.25f);
            movingPieces = CollapseColumn(gamePieces);
            while (!IsCollapsed(gamePieces))
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
                multi += 1;
                Debug.Log("multiplicador" + multi);
                yield return StartCoroutine(ClearandCollpse(matches, 10, multi));
                yield return new WaitForSeconds(0.5f);
            }
        }
        enEjecucion = false;
        yield return null;
    }
    IEnumerator RefillRoutine() //Rutina que se encarga de llenar las fichas eliminadas
    {
        FillPicesAt();
        yield return null;
    }
    bool IsCollapsed(List<GamePiece> gamePieces)  //Metodo que me identifica que en las columnas estan colapsadas
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
    void Points(int pnt, float multiplicador = 1)  //Metodo que suma y lleva el puntaje en pantalla
    {
        puntuacion += (float)pnt * multiplicador;
        string puntaje;
        puntaje = puntuacion.ToString("0000");
        puntajeEnPantalla.text = puntaje + "/" + goal.ToString();
    }
    void GameFinish(int puntos, float tiempo)
    {
        if (puntos >= goal && tiempo < timeGoal)
        {

        }
    }
    private void OnEnable()
    {
        switch (nivel)
        {
            case Levels.Level1:
                goal = 1000;
                timeGoal = 120f;
                tInicial = 120;
                break;
            case Levels.Level2:
                goal = 1500;
                timeGoal = 240;
                tInicial = 240;
                break;
            case Levels.Level3:
                goal = 2000;
                timeGoal = 360f;
                tInicial = 360;
                break;
            case Levels.Level4:
                goal = 2500;
                timeGoal = 420f;
                tInicial = 420;
                break;
            case Levels.Level5:
                goal = 3000;
                timeGoal = 540f;
                tInicial = 540;
                break;
            case Levels.infinito:

                break;
            default:
                break;
        }
    }
    void SetBasicUI(bool bl, string text)
    {
        aviso.GetComponentInChildren<TMP_Text>().text = text;
        aviso.SetActive(bl);
    }
    public enum Levels
    {
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
        infinito
    }
}
