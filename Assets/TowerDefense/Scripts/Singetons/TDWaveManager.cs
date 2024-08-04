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
    private GameDifficulty gameDifficulty = GameDifficulty.MEDIUM;
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
    private WaveState waveState=WaveState.FINISHED;
    //events
    public event Action OnFinishedWave;
    public event Action OnStartedWave;
    public event Action OnRestart;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        TDGameManager.Instance.OnNextWave += TDGameManager_OnNextWave;
        Restart();
    }

    private void TDGameManager_OnNextWave()
    {
        waveState = WaveState.PLAYING;
        OnStartedWave?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        switch (waveState)
        {
            case WaveState.PLAYING:
                currentWaveTimerCount += Time.deltaTime;
                if(currentWaveTimerCount > currentWaveTimer && TDUnitSpawner.Instance.UnitsClear())
                {
                    NextWave();
                }
                break;
            case WaveState.FINISHED:
                break;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Time.timeScale = Time.timeScale == 2f ? 1f : 2f;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            NextWave();
        }
    }
    public void NextWave()
    {
        Debug.Log(currentUnitSpawnCooldown) ;
        OnFinishedWave?.Invoke();
        waveState = WaveState.FINISHED;
        currentWave++;
        currentWaveTimer += currentWaveTimer / (incrementFactor / 5f);
        currentUnitSpeed += currentUnitSpeed / (incrementFactor);
        currentUnitSpawnCooldown -= currentWave/(incrementFactor/2f);
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
                minUnitSpawnCooldown = 1.5f;
                currentUnitSpawnCooldown = 5f;
                maxUnitSpeed = 2.0f;
                currentUnitSpeed = 1f;
                maxUnitDamage = 50f;
                currentUnitDamage = 20f;
                break;
            case GameDifficulty.MEDIUM:
                incrementFactor = 45f;
                maxUnitHealthPoints = 2500f;
                minUnitSpawnCooldown = 1f;
                currentUnitSpawnCooldown = 3.5f;
                maxUnitSpeed = 4.0f;
                currentUnitSpeed = 2f;
                maxUnitDamage = 100f;
                currentUnitDamage = 45f;
                break;
            case GameDifficulty.HARD:
                incrementFactor = 40f;
                maxUnitHealthPoints = 5000f;
                minUnitSpawnCooldown = 0.5f;
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
    public bool IsOver()
    {
        return currentWaveTimerCount > currentWaveTimer;
    }
    public int GetUnitKillPrize(int unitHealth)
    {
        return (int)((0.008f * incrementFactor) * unitHealth);
    }
    public void Restart()
    {
        currentWave = 1;
        OnSetGameDifficulty(gameDifficulty);
        currentWaveTimer = 20f;
        currentWaveTimerCount = 0f;
        currentUnitHealthPoints = 100f;
        OnRestart?.Invoke();
    }
}
