using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    #region Public Variables
    public Text pedometerText;
    public Text pedometerTextButtonText;
    #endregion

    #region Private Variables
    #endregion

    #region Public Functions
    #region UI Button Functions
    public void Pedometer()
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
    #endregion

    #endregion

    #region Private Fuctions
    private void Start()
    {
    }
    private void Update()
    {
    }
    #endregion
}
