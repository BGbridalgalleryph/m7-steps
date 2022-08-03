using System;
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
    public Text gameplaySteps;
    public Text gameplayDistance;
    public Text gameplayTime;
    public GameObject RunningState;
    public GameObject PauseState;
    public GameObject SummaryState;

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
    private float _startGamePlayWaitTime = 4.0f;
    private bool _startGamePlayLoading = false;
    private bool _starGameplayCountdown0 = false;
    private bool _starGameplayCountdown1 = false;
    private bool _starGameplayCountdown2 = false;
    private bool _starGameplayCountdown3 = false;

    #region new Game
    private bool newGame = false;
    #endregion
    #endregion

    #endregion

    #region Public Functions

    #region Page Switching Functions
    public void GotoHome()
    {
        LoadingScene.SetActive(false);
        HomePage.SetActive(true);
        GameplayPage.SetActive(false);
    }

    public void GotoGamePlay()
    {
        ResetGamePlay();
        LoadingScene.SetActive(false);
        HomePage.SetActive(false);
        GameplayPage.SetActive(true);
        newGame = true;
        startActualGameplay();
    }

    #endregion

    #region UI Button Functions
    //public void Pedometer()
    //{
    //}

    public void PauseCurrentGame()
    {

        SetGameplayState(2);

        //pause pedometer logic
        PauseStateContent.SetActive(true);
        PauseButton.SetActive(false);

        //stop pedometer logic
        GameplayManager.Instance.StopGame();
    }

    public void StopCurrentGame()
    {
        PauseStateContent.SetActive(false);
        PauseButton.SetActive(false);
        SummaryContent.SetActive(true);
        SummaryButton.SetActive(true);
        SetGameplayState(3);

    }

    public void ResumeCurrentGame()
    {
        PauseStateContent.SetActive(false);
        PauseButton.SetActive(true);
        startActualGameplay();
    }
    public void ResetGamePlay()
    {
        gameplaySteps.text = "0";
        gameplayDistance.text = "0";
        gameplayTime.text = "00:00";
        StopwatchManager.Instance.ResetStopWatch();
        SetGameplayState(0);
        PauseButton.SetActive(true);
        SummaryButton.SetActive(false);
        SummaryContent.SetActive(false);
    }
    #endregion

    #region UI Update Functions
    public void PopulateGameplayDetails(int steps, double distance)
    {
        gameplaySteps.text = steps.ToString();
        double ftToKm = distance * 0.0003048;
        gameplayDistance.text = Math.Round(ftToKm, 2).ToString();
    }

    /// <summary>
    /// state 0 = hide all state;
    /// state 1 = running;
    /// state 2 = pause;
    /// state 3 = summary;
    /// </summary>
    /// <param name="state"></param>
    public void SetGameplayState(int state)
    {
        if(state == 0)
        {
            RunningState.SetActive(false);
            PauseState.SetActive(false);
            SummaryState.SetActive(false);
        }
        else if(state == 1)
        {
            RunningState.SetActive(true);
            PauseState.SetActive(false);
            SummaryState.SetActive(false);
        }
        else if (state == 2)
        {
            RunningState.SetActive(false);
            PauseState.SetActive(true);
            SummaryState.SetActive(false);
        }
        else if (state == 3)
        {
            RunningState.SetActive(false);
            PauseState.SetActive(false);
            SummaryState.SetActive(true);
        }
    }

    public void FinalizeGamePlay()
    {

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
            if (timer > (_splashLoadingWaitTime + 1))
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
                SetGameplayState(1);
                GameplayOverlayCountdown.sprite = null;
                timer = 0;
                GameplayOverlay.SetActive(false);
                GameplayManager.Instance.StartGame(newGame);
                newGame = false;
            }
        }
    }

    private void startSplashScreenLoading()
    {
        _splashLoading = true;
    }

    private void startActualGameplay()
    {
        GameplayOverlay.SetActive(true);
        _starGameplayCountdown0 = true;
        _starGameplayCountdown1 = true;
        _starGameplayCountdown2 = true;
        _starGameplayCountdown3 = true;
        _startGamePlayLoading = true;
    }

    #endregion
}
