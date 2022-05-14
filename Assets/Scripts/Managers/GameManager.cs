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

    [Scene] [SerializeField] string menuScene;
    [Scene] [SerializeField] string gameScene;

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
    }

    public void ToGame(int numAI, Difficulty AIDifficulty, string seed) 
    {
        SceneManager.UnloadSceneAsync(menuScene);

        SceneManager.LoadScene(gameScene, LoadSceneMode.Additive);
    }

    #endregion
}
 