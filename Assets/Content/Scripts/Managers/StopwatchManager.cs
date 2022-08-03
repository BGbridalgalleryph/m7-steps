using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopwatchManager : Singleton<StopwatchManager>
{
    #region Public Variables
    [HideInInspector]
    public TimeSpan currentTime;
    #endregion

    #region Private Variables
    private bool stopWatchEnabled = false;
    private float currentTimeFloat;
    #endregion

    #region Public Functions
    public void StartStopWatch()
    {
        stopWatchEnabled = true;
    }

    public void StopStopWatch()
    {
        stopWatchEnabled = false;
    }

    public void ResetStopWatch()
    {
        currentTimeFloat = 0f;
        currentTime = TimeSpan.Zero;
    }
    #endregion

    #region Private Functions
    private void Update()
    {
        if(stopWatchEnabled)
        {
            currentTimeFloat += Time.deltaTime;
            currentTime = TimeSpan.FromSeconds(currentTimeFloat);
        }

    }
    #endregion
}
