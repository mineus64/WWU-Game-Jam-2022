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
        
    }

    #endregion

    #region Specific Methods

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
    }

    public void Respawn(GameObject entity) 
    {
        entity.transform.position = spawnPoints[Random.Range(0, spawnPoints.Count - 1)].transform.position;
    }

    #endregion
}
 