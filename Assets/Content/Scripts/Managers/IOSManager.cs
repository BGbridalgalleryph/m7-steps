using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeliefEngine.HealthKit;
using System;

public class IOSManager : Singleton<IOSManager>
{
    #region Public Variables
    public HealthKitDataTypes types;
    #endregion

    #region Private Variables

    private HealthStore healthStore;
    private bool reading = false;
    #endregion

    #region Public Functions
    public void ReadPedometer()
    {
        DateTimeOffset start = DateTimeOffset.UtcNow;

        if (!reading)
        {
            int steps = 0;
            this.healthStore.BeginReadingPedometerData(start, delegate (List<PedometerData> data, Error error) {
                foreach (PedometerData sample in data)
                {
                    steps += sample.numberOfSteps;
                }
                UIManager.Instance.pedometerText.text = steps.ToString();
                UIManager.Instance.pedometerTextButtonText.text = "Stop Pedometer";
                steps = 0;
                //this.resultsLabel.text = string.Format("{0}", steps);


            });
            //buttonLabel.text = "Stop reading";
            reading = true;
        }
        else
        {
            this.healthStore.StopReadingPedometerData();
            //buttonLabel.text = "Start reading";
            UIManager.Instance.pedometerTextButtonText.text = "Start Pedometer";
            reading = false;
        }
    }
    #endregion

    #region Private Fuctions
    private void Start()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            this.healthStore = this.GetComponent<HealthStore>();
            this.healthStore.Authorize(this.types);
        }
    }
    #endregion
}
