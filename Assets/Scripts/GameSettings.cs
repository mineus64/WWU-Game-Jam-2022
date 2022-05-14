using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    #region Singleton

    public static GameSettings current = null;

    #endregion

    #region Variables



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
        
    }

    #endregion

    #region Specific Methods



    #endregion
}

#region Enums

public enum Difficulty 
{
    Easy,
    Normal,
    Hard,
    Insane
}

#endregion