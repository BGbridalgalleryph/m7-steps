using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;

public class AndroidManager : Singleton<AndroidManager>
{
    #region Public Variables
    #endregion

    #region Private Variables
    #endregion

    #region Public Functions
    #endregion

    #region Private Fuctions
    private void Start()
    {
        if (!Permission.HasUserAuthorizedPermission("android.permission.ACTIVITY_RECOGNITION"))
        {
            Permission.RequestUserPermission("android.permission.ACTIVITY_RECOGNITION");
        }
        InputSystem.EnableDevice(StepCounter.current);

    }
    private void Update()
    {
        try
        {
            if (StepCounter.current.enabled)
            {
                StepCounter.current.samplingFrequency = 1;
                var stepValue = StepCounter.current.stepCounter.ReadValue();
                //stepsValue += stepValue;
                //stepText.text = stepValue.ToString();
            }
            else
            {
                //stepText.text = "not enabled";
            }
        }
        catch (Exception e)
        {
            //stepText.text = $"error: {e.Message}";
        }
    }
    #endregion
}
