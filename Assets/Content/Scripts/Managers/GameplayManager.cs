using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{
    #region Public Variables
    /// <summary>
    /// the M7 step coins earnings per minute
    /// </summary>
    public float earningsPerMinute = 0.2f;

    /// <summary>
    /// the multiplier we use with steps for m7 steps earnings
    /// </summary>
    public float stepsEarningsMultiplier = 0.1f;

    /// <summary>
    /// the multiplier we use with the luck for m7 steps earnings
    /// </summary>
    public float luckEarningsMultiplier = 0.1f;

    /// <summary>
    /// current earnings
    /// </summary>
    public double currentEarnings;

    [Header("User")]
    public double currentEnergy = 15;
    public float Luck = 10f;
    public int currentSteps;
    public double currentDistance;
    public float currentTime;
    #endregion

    #region Private Variables
    private bool gameCurrentlyStarting = false;
    private int temporarySteps;
    private double temporaryDistance;
    private double originalEnergy;
    #endregion

    #region Public Functions
    public void StartGame(bool newGame = false)
    {
        if (newGame)
        {
            originalEnergy = currentEnergy;
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
        Debug.Log("gameCurrentlyStarting: " + gameCurrentlyStarting);
        gameCurrentlyStarting = false;
        Debug.Log("gameCurrentlyStarting: " + gameCurrentlyStarting);
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
        StopwatchManager.Instance.onAchievedMinuteThreshold += decreaseEnergy;
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
        StopwatchManager.Instance.onAchievedMinuteThreshold -= decreaseEnergy;
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
        double distanceAssumption = steps * 0.71;
        temporaryDistance = distanceAssumption;
        UIManager.Instance.PopulateGameplayDetails(temporarySteps + currentSteps, temporaryDistance + currentDistance);
    }

    private void decreaseEnergy()
    {
        currentEnergy--;
        UIManager.Instance.currentEnergy.text = $"{currentEnergy} / {originalEnergy}";
        if (currentEnergy <= 0)
        {
            StopGame();
            UIManager.Instance.StopCurrentGame();
        }
    }

    private void Update()
    {
        if (gameCurrentlyStarting)
        {
            UIManager.Instance.gameplayTime.text = StopwatchManager.Instance.currentTime.ToString(@"mm\:ss");
            UIManager.Instance.currentM7StepCoinEarnings.text = $"+ {earningsAlgorithm()}";
            //earningsAlgorithm();
        }
    }

    private string earningsAlgorithm()
    {
        double steps = temporarySteps + currentSteps;
        double distance = temporaryDistance + currentDistance;

        //current steps earning is steps * 0.1
        double stepsEarnings = steps * stepsEarningsMultiplier;

        //current earnings is 0.2 coins per minute
        double minuteEarningsEnergy = (earningsPerMinute / 60) * StopwatchManager.Instance.currentTimeFloat;

        //current luck earnings is luck * 0.1
        double luckEarnings = Luck * luckEarningsMultiplier;

        currentEarnings = stepsEarnings * minuteEarningsEnergy * luckEarnings;
        //Debug.Log("currentEarnings: " + currentEarnings + " gameCurrentlyStarting: " + gameCurrentlyStarting);
        if(currentEarnings >= 0.01)
        {
            return currentEarnings.ToString("#.##");
        }
        else
        {
            return "0.0";
        }
    }
    #endregion
}
