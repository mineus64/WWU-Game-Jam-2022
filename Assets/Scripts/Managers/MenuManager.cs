// This script controls main menu functions, to include navigation et. al.

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    #region Singleton

    public static MenuManager current = null;

    #endregion

    #region Variables

    [Header("Menus")]
    [SerializeField] GameObject[] canvases;

    [Header("New Game Settings Fields")]
    [SerializeField] Slider AINumSlider;
    [SerializeField] Slider AIDifficultySlider;
    [SerializeField] TMP_InputField gameSeedInput;

    [Header("New Game Settings Values")]
    [SerializeField] int AINum;
    [SerializeField] Difficulty AIDifficulty;
    [SerializeField] string gameSeed;

    [Header("New Game Display Values")]
    [SerializeField] TMP_Text AINumDisplay;
    [SerializeField] TMP_Text AIDifficultyDisplay;

    #endregion

    #region General Methods

    void Awake() 
    {
        current = this;


    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AINum = (int)AINumSlider.value;
        AIDifficulty = (Difficulty)AIDifficultySlider.value;
        gameSeed = gameSeedInput.text;

        AINumDisplay.text = AINum.ToString();
        AIDifficultyDisplay.text = AIDifficulty.ToString();
    }

    #endregion

    #region Specific Methods

    void LoadMenu(int menuID) 
    {
        if (menuID >= canvases.Length || menuID < 0) {
            Debug.LogError("ERROR! Invalid canvas ID in MenuManager.LoadMenu");

            return;
        }

        for (int i = 0; i < canvases.Length; i++) {
            if (i == menuID) {
                canvases[i].SetActive(true);
            }
            else {
                canvases[i].SetActive(false);
            }
        }
    }

    public void ToMainMenu() 
    {
        LoadMenu(0);
    }

    public void ToNewGameMenu() 
    {
        LoadMenu(1);
    }

    public void ToSettingsMenu() 
    {
        LoadMenu(2);
    }

    public void ToCredits() 
    {
        LoadMenu(3);
    }

    public void ToQuit() 
    {
        Application.Quit();
    }

    public void GameStart() 
    {
        GameManager.current.ToGame(AINum, AIDifficulty, gameSeed);
    }

    #endregion
}
