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

    List<IEnumerator> timers = new List<IEnumerator>();

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

    void FixedUpdate() 
    {
        
    }

    #endregion

    #region Specific Methods

    public int Create(float time, Action returnMethod = null)
    {
        IEnumerator timer = Timer(time, returnMethod);

        StartCoroutine(timer);

        timers.Add(timer);

        return (timers.Count - 1);
    }

    public void Remove(int id) 
    {
        StopCoroutine(timers[id]);
    }

    #endregion

    #region Coroutines

    IEnumerator Timer(float time, Action callbackAction = null) 
    {
        yield return new WaitForSeconds(time);

        callbackAction?.Invoke();

        yield return null;
    }

    #endregion
}