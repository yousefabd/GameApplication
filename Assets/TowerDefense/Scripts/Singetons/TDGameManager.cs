using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDGameManager : MonoBehaviour
{
    public static TDGameManager Instance;
    private enum TDGameState  {STARTING,COUNTDOWN,PLAYING};
    private TDGameState currentGameState;
    private float countdownTimer = 4f;
    public event Action OnNextWave;
    public event Action OnDisplayShop;
    public event Action OnCountDownStarted;
    private void Awake()
    {
        Instance = this;
        currentGameState = TDGameState.COUNTDOWN;
    }
    private void Update()
    {
        switch (currentGameState)
        {
            case TDGameState.STARTING:
                break;
            case TDGameState.COUNTDOWN:
                Countdown();
                break;
            case TDGameState.PLAYING:
                break;
        }
    }
    private void Countdown()
    {
        countdownTimer -= Time.deltaTime;
        if (countdownTimer < 0)
        {
            countdownTimer = 4f;
            OnNextWave?.Invoke();
            currentGameState = TDGameState.PLAYING;
        }
    }
    public void DisplayShop()
    {
        Debug.Log("Display Shop");
        OnDisplayShop?.Invoke();
    }

    public void NextWave()
    {
        currentGameState = TDGameState.COUNTDOWN;
        OnCountDownStarted?.Invoke();
    }

    public float GetCountdownTimer()
    {
        return countdownTimer;
    }
}
