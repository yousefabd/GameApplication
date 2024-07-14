using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameDifficulty
{
    EASY,MEDIUM,HARD
}
public class TDWaveManager : MonoBehaviour
{
    public static TDWaveManager Instance { get; private set; }
    //game variables
    private float maxWaveTime=120f;
    private float maxUnitDamage;
    private float maxUnitSpeed;
    private float minUnitSpawnCooldown;
    private float maxUnitHealthPoints;
    private float incrementFactor;
    //state variables
    private int currentWave;
    private float currentUnitSpeed;
    private float currentUnitSpawnCooldown;
    private float currentUnitDamage;
    //---------------------------------//
    private float currentWaveTimer = 20f;
    private float currentWaveTimerCount = 0f;
    private float currentUnitHealthPoints=100f;
    private enum WaveState { PLAYING,FINISHED}
    private WaveState waveState = WaveState.PLAYING;
    //events
    public event Action OnFinishedWave;
    public event Action OnStartedWave;
    private void Awake()
    {
        Instance = this;
        currentWave = 1;
        OnSetGameDifficulty(GameDifficulty.EASY);
    }
    private void Start()
    {
        TDPlayer.Instance.OnNextWave += TDPlayer_OnNextWave;
    }

    private void TDPlayer_OnNextWave()
    {
        waveState = WaveState.PLAYING;
        Time.timeScale = 1.0f;
        OnStartedWave?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        switch (waveState)
        {
            case WaveState.PLAYING:
                currentWaveTimerCount += Time.deltaTime;
                if(currentWaveTimerCount > currentWaveTimer)
                {
                    NextWave();
                }
                break;
            case WaveState.FINISHED:
                break;
        }
    }
    public void NextWave()
    {
        Debug.Log("finished Wave");
        OnFinishedWave?.Invoke();
        waveState = WaveState.FINISHED;
        currentWave++;
        currentWaveTimer += currentWaveTimer / (incrementFactor / 5f);
        currentUnitSpeed += currentUnitSpeed / (incrementFactor);
        currentUnitSpawnCooldown -= (currentUnitSpawnCooldown / (incrementFactor/4f));
        currentUnitDamage += (currentUnitDamage / (incrementFactor / 5f));
        currentUnitHealthPoints += (currentUnitHealthPoints / (incrementFactor / 5f));

        if (currentUnitHealthPoints >= maxUnitHealthPoints)
            currentUnitHealthPoints = maxUnitHealthPoints;
        if (currentUnitDamage >= maxUnitDamage)
            currentUnitDamage = maxUnitDamage;
        if(currentUnitSpawnCooldown <=minUnitSpawnCooldown)
            currentUnitSpawnCooldown = minUnitSpawnCooldown;
        if(currentUnitSpeed >= maxUnitSpeed)
            currentUnitSpeed = maxUnitSpeed;
        if (currentWaveTimer >= maxWaveTime)
            currentWaveTimer = maxWaveTime;
        currentWaveTimerCount = 0f;
    }
    public void OnSetGameDifficulty(GameDifficulty difficulty)
    {
        switch (difficulty)
        {
            case GameDifficulty.EASY:
                incrementFactor = 50f;
                maxUnitHealthPoints = 1000f;
                minUnitSpawnCooldown = 0.5f;
                currentUnitSpawnCooldown = 5f;
                maxUnitSpeed = 2.0f;
                currentUnitSpeed = 1f;
                maxUnitDamage = 50f;
                currentUnitDamage = 20f;
                break;
            case GameDifficulty.MEDIUM:
                incrementFactor = 30f;
                maxUnitHealthPoints = 2500f;
                minUnitSpawnCooldown = 0.25f;
                currentUnitSpawnCooldown = 3.5f;
                maxUnitSpeed = 4.0f;
                currentUnitSpeed = 2f;
                maxUnitDamage = 100f;
                currentUnitDamage = 45f;
                break;
            case GameDifficulty.HARD:
                incrementFactor = 10f;
                maxUnitHealthPoints = 5000f;
                minUnitSpawnCooldown = 0.05f;
                currentUnitSpawnCooldown = 1.5f;
                maxUnitSpeed = 6.0f;
                currentUnitSpeed = 2.5f;
                maxUnitDamage = 150f;
                currentUnitDamage = 60f;
                break;
        }
    }
    public void GetUnitVariables(out float currentUnitSpeed, out float currentUnitDamage, out float currentUnitSpawnCoolDown,out float currentUnitHealthPoints)
    {
        currentUnitSpeed = this.currentUnitSpeed;
        currentUnitDamage = this.currentUnitDamage;
        currentUnitSpawnCoolDown = currentUnitSpawnCooldown;
        currentUnitHealthPoints = this.currentUnitHealthPoints;
    }
    public int GetCurrentWave()
    {
        return currentWave;
    }
}
