using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    #region Singleton

    public static Timer current = null;

    #endregion

    #region Variables

    object test;

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

public class Timer
{
    #region Variables



    #endregion

    #region General Methods



    #endregion

    #region Specific Methods



    #endregion
}