// This script handles overall things such as scene management and scene loading

using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager current = null;

    #endregion

    #region Variables

    [Header("Scenes")]
    [Scene] [SerializeField] string menuScene;
    [Scene] [SerializeField] string gameScene;

    [Header("Components")]
    public NetworkManager networkManager;

    [Header("Global Variables")]
    public PlayerController currentClient;

    [Header("Objects")]
    public GameObject sound;
    public Weapon[] weapons;
    [SerializeField] GameObject AI;

    [Header("Spawn Points")]
    [SerializeField] List<GameObject> spawnPoints;
    [SerializeField] int numAI;
    [SerializeField] Difficulty aiDifficulty;

    [Header("Objective")]
    public float gameLength = 300.0f;
    public float gameTimer {get; private set;}
    [SerializeField] bool gameStarted = false;
    public GameState gameState = GameState.Starting;
    public int playerKills = 0;
    public int playerDeaths = 0;
    [SerializeField] float gameEndTimer;
    [SerializeField] float gameEndLength = 5.0f;
    [SerializeField] bool gameEnding = false;

    #endregion

    #region General Methods

    void Awake() 
    {
        current = this;

        ToMenu(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted) {
            gameTimer = TimerController.current.Create(gameLength,GameStateAndGameEnding);

        }

        if (gameEnding) {
            gameEndTimer = TimerController.current.Create(gameEndLength,ToMenu);
        }
    }

    #endregion

    #region Specific Methods

    public void GameStateAndGameEnding()
    {
        gameState = GameState.Ended;

        gameEnding = true;
    }

    public void ToMenu()
    {
        ToMenu(true);
    }
    
    public void ToMenu(bool unload)
    {
        if (unload == true) {
            SceneManager.UnloadSceneAsync(gameScene);
        }

        SceneManager.LoadScene(menuScene, LoadSceneMode.Additive);
    }

    public void ToGame(bool unload) 
    {
        if (unload == true) {
            SceneManager.UnloadSceneAsync(menuScene);
        }

        SceneManager.LoadScene(gameScene, LoadSceneMode.Additive);

        networkManager.StartHost();
    }

    public void ToGame(int numAI, Difficulty AIDifficulty, string seed) 
    {
        SceneManager.UnloadSceneAsync(menuScene);

        networkManager.StartHost();

        SceneManager.LoadScene(gameScene, LoadSceneMode.Additive);

        this.numAI = numAI;
        this.aiDifficulty = AIDifficulty;
    }

    public void SpawnPlayers() 
    {
        Debug.Log(string.Format("Spawning {0} AI", numAI));

        List<GameObject> availableSpawns = spawnPoints;

        int playerSpawn = (int)Random.Range(0, availableSpawns.Count - 1);

        currentClient.transform.position = availableSpawns[playerSpawn].transform.position;

        availableSpawns.Remove(availableSpawns[playerSpawn]);

        for (int i = 0; i < numAI; i++) {
            int spawnIndex = (int)Random.Range(0, availableSpawns.Count - 1);

            GameObject thisAI = Instantiate(AI, availableSpawns[spawnIndex].transform.position, Quaternion.identity);
            thisAI.GetComponent<AIController>().aiDifficulty = this.aiDifficulty;

            NetworkServer.Spawn(thisAI);

            availableSpawns.Remove(availableSpawns[spawnIndex]);
        }

        StartGame();
    }

    public void Respawn(GameObject entity) 
    {
        entity.transform.position = spawnPoints[Random.Range(0, spawnPoints.Count - 1)].transform.position;

        entity.GetComponent<MeshRenderer>().enabled = true;
    }

    void StartGame() 
    {
        gameTimer = 300.0f;
        gameStarted = true;
        gameState = GameState.Running;
        playerKills = 0;
        playerDeaths = 0;
        gameEndTimer = 5.0f;
        gameEnding = false;
    }

    #endregion
}

public enum GameState 
{
    Starting,
    Running,
    Ended
}