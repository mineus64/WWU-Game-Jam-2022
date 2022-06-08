// This script is used to control timing-related stuff for Sonisight

using System;
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
            if (timer.time > 0) {
                timer.Tick(Time.deltaTime);
            }
        }
    }

    void FixedUpdate() 
    {
        foreach (Timer timer in fixedUpdateTimers)
        {
            if (timer.time > 0) {
                timer.Tick(Time.fixedDeltaTime);
            }
        }
    }

    #endregion

    #region Specific Methods

    public Action Add(float time, bool type = true) 
    {
        Action output;

        if (type == true) {
            output = AddUpdate(time);
        }
        else {
            output = AddFixedUpdate(time);
        }

        return output;
    }

    public Action AddUpdate(float time)
    {
        Timer newTimer = new Timer(updateTimers.Count, time);
        updateTimers.Add(newTimer);
        return newTimer.callbackAction;
    }

    public Action AddFixedUpdate(float time)
    {
        Timer newTimer = new Timer(fixedUpdateTimers.Count, time);
        fixedUpdateTimers.Add(newTimer);
        return newTimer.callbackAction;
    }

    // Might create a memory leak, look into this more if we get memory problems
    // I'm not sure if GC will properly destroy this class instance
    public void Remove(int id, bool type = true)
    {
        if (type == true) {
            RemoveUpdate(id);
        }
        else {
            RemoveFixedUpdate(id);
        }
    }

    public void RemoveUpdate(int id)
    {
        Timer removeTimer = updateTimers[id];
        updateTimers.Remove(removeTimer);
    }

    public void RemoveFixedUpdate(int id)
    {
        Timer removeTimer = fixedUpdateTimers[id];
        fixedUpdateTimers.Remove(removeTimer);
    }

    public void Restart(int id, bool type = true) 
    {
        if (type == true) {
            RestartUpdate(id);
        }
        else {
            RestartFixedUpdate(id);
        }
    }

    public void RestartUpdate(int id) 
    {
        updateTimers[id].Restart();
    }

    public void RestartFixedUpdate(int id) 
    {
        fixedUpdateTimers[id].Restart();
    }

    #endregion
}

// The Timer is a data object that holds values for a single timer
public class Timer : MonoBehaviour
{
    #region Variables

    public int id {get; private set;}
    public float startTime {get; private set;}
    public float time {get; private set;}
    public Action callbackAction {get; private set;}
    public bool reuse {get; private set;}

    #endregion

    #region General Methods



    #endregion

    #region Specific Methods

    public void Tick(float tickAmount) 
    {
        time = Mathf.Max(time - tickAmount, 0);

        if (time <= 0) {
            callbackAction?.Invoke();

            if (reuse == false) {
                TimerController.current.Remove(id);
            }
        }
    }

    public void Restart() 
    {
        time = startTime;
    }

    #endregion

    #region Constructors

    public Timer(int id, float startTime, bool reuse = false) 
    {
        this.id = id;
        this.startTime = startTime;
        this.time = startTime;
        this.reuse = reuse;
    }

    #endregion
}