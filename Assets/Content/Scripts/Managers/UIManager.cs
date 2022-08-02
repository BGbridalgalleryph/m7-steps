using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    #region Public Variables
    [Header("Main Pages")]
    public GameObject LoadingScene;
    public GameObject HomePage;
    public GameObject GameplayPage;

    [Header("Loading Scene UI Contents")]
    public Image splashScreenLoadingGuage;

    [Header("Gameplay Page UI Contents")]
    public GameObject GameplayOverlay;
    public Image GameplayOverlayCountdown;
    public GameObject PauseStateContent;
    public GameObject PauseButton;
    public GameObject SummaryContent;
    public GameObject SummaryButton;

    [Header("Prefabs and other External Assets")]
    public Sprite GameplayCountdownGo;
    public Sprite GameplayCountdown1;
    public Sprite GameplayCountdown2;
    public Sprite GameplayCountdown3;

    [Header("pedometer")]
    public Text pedometerText;
    public Text pedometerTextButtonText;
    #endregion

    #region Private Variables
    private float timer;
    #region splash Screen Loading Variables
    private bool _splashLoading = false;
    private float _splashLoadingWaitTime = 4.0f;
    #endregion

    #region start Gameplay Loading Variables
    private bool _startGamePlayLoading = false;
    private float _startGamePlayWaitTime = 4.0f;
    private bool _starGameplayCountdown0 = false;
    private bool _starGameplayCountdown1 = false;
    private bool _starGameplayCountdown2 = false;
    private bool _starGameplayCountdown3 = false;
    #endregion

    #endregion

    #region Public Functions

    #region Page Switching Scripts
    public void GotoHome()
    {
        LoadingScene.SetActive(false);
        HomePage.SetActive(true);
        GameplayPage.SetActive(false);
    }

    public void GotoGamePlay(bool startTheGame = false)
    {
        LoadingScene.SetActive(false);
        HomePage.SetActive(false);
        GameplayPage.SetActive(true);

        if(startTheGame)
        {
            GameplayOverlay.SetActive(true);
            _starGameplayCountdown0 = true;
            _starGameplayCountdown1 = true;
            _starGameplayCountdown2 = true;
            _starGameplayCountdown3 = true;
            _startGamePlayLoading = true;
        }
    }
    #endregion

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

    public void PauseCurrentGame()
    {
        //pause pedometer logic
        PauseStateContent.SetActive(true);
        PauseButton.SetActive(false);
    }

    public void StopCurrentGame()
    {
        //stop pedometer logic
        PauseStateContent.SetActive(false);
        PauseButton.SetActive(false);
        SummaryContent.SetActive(true);
        SummaryButton.SetActive(true);
    }
    #endregion

    #endregion

    #region Private Fuctions
    private void Start()
    {
        startSplashScreenLoading();
    }
    private void Update()
    {
        if(_splashLoading)
        {
            splashScreenLoadingGuage.fillAmount = timer / _splashLoadingWaitTime;
            timer += Time.deltaTime;
            if (timer > _splashLoadingWaitTime)
            {
                _splashLoading = false;
                timer = 0.0f;
                GotoHome();
            }
        }

        if(_startGamePlayLoading)
        {
            timer += Time.deltaTime;
            if(_starGameplayCountdown0)
            {
                _starGameplayCountdown0 = false;
                GameplayOverlayCountdown.sprite = GameplayCountdown3;
            }
            if (timer > 1 && _starGameplayCountdown1)
            {
                _starGameplayCountdown1 = false;
                GameplayOverlayCountdown.sprite = GameplayCountdown2;

            }
            if (timer > 2 && _starGameplayCountdown2)
            {
                _starGameplayCountdown2 = false;
                GameplayOverlayCountdown.sprite = GameplayCountdown1;

            }
            if (timer > 3 && _starGameplayCountdown3)
            {
                _starGameplayCountdown3 = false;
                GameplayOverlayCountdown.sprite = GameplayCountdownGo;

            }
            if (timer > _startGamePlayWaitTime)
            {
                _startGamePlayLoading = false;
                GameplayOverlayCountdown.sprite = null;
                GameplayOverlay.SetActive(false);

            }
        }
    }

    private void startSplashScreenLoading()
    {
        _splashLoading = true;
    }

    #endregion
}
