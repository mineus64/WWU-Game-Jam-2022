// This script is used to control timing-related stuff for Sonisight

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The TimerController class gives overall control over timers in general
public class TimerController : MonoBehaviour
{
    #region Singleton

    public static TimerController current = null;

    #endregion

    #region Variables

    List<Timer> updateTimers = new List<Timer>();
    List<Timer> fixedUpdateTimers = new List<Timer>();

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
        foreach (Timer timer in updateTimers)
        {
            timer.Tick(Time.deltaTime);
        }
    }

    void FixedUpdate() 
    {
        foreach (Timer timer in fixedUpdateTimers)
        {
            timer.Tick(Time.fixedDeltaTime);
        }
    }

    #endregion

    #region Specific Methods



    #endregion
}

// The Timer is a data object that holds values for a single timer
public class Timer
{
    #region Variables

    public int id {get; private set;}
    public float time {get; private set;}

    #endregion

    #region General Methods



    #endregion

    #region Specific Methods

    public void Tick(float tickAmount) 
    {
        time = Mathf.Max(time - tickAmount, 0);

        if (time <= 0) {

        }
    }

    #endregion

    #region Constructors

    public Timer(int id, float startTime) 
    {
        this.id = id;
        this.time = startTime;
    }

    #endregion
}