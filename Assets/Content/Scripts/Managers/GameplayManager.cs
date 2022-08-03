using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{
    #region Public Variables
    [Header("User")]
    public float Energy = 100f;
    public float Luck = 10f;
    public int currentSteps;
    public double currentDistance;
    public float currentTime;

    #endregion

    #region Private Variables
    private bool gameCurrentlyStarting = false;
    private int temporarySteps;
    private double temporaryDistance;
    #endregion

    #region Public Functions
    public void StartGame(bool newGame = false)
    {
        if(newGame)
        {
            currentSteps = 0;
            currentDistance = 0;
            currentSteps = 0;
        }
        gameCurrentlyStarting = true;
        StopwatchManager.Instance.StartStopWatch();
        Pedometer();
    }

    public void StopGame()
    {
        currentSteps += temporarySteps;
        currentDistance += temporaryDistance;
        gameCurrentlyStarting = false;
        StopwatchManager.Instance.StopStopWatch();
        Pedometer();
    }
    #endregion

    #region Private Functions
    private void Pedometer()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidManager.Instance.ReadPedometer();
        }
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IOSManager.Instance.ReadPedometer();
        }
    }


    private void OnEnable()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidManager.Instance.OnPopulateSteps += populateStepsAndroid;
        }
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IOSManager.Instance.OnPopulateSteps += populateStepsIOS;
        }
    }

    private void OnDisable()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidManager.Instance.OnPopulateSteps -= populateStepsAndroid;
        }
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IOSManager.Instance.OnPopulateSteps -= populateStepsIOS;
        }
    }

    private void populateStepsAndroid(int steps, double distance)
    {
        temporarySteps = steps;
        temporaryDistance = distance;
        UIManager.Instance.PopulateGameplayDetails(temporarySteps + currentSteps, temporaryDistance + currentDistance);
    }
    private void populateStepsIOS(int steps, double distance)
    {
        temporarySteps = steps;
        double distanceAssumption = distance * 0.71;
        temporaryDistance = distanceAssumption;
        UIManager.Instance.PopulateGameplayDetails(temporarySteps + currentSteps, temporaryDistance + currentDistance);
    }

    private void Update()
    {
        if(gameCurrentlyStarting)
        {
            UIManager.Instance.gameplayTime.text = StopwatchManager.Instance.currentTime.ToString(@"mm\:ss");
        }
    }
    #endregion
}
