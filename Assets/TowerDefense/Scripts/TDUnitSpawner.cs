using Assets.HeroEditor4D.Common.Scripts.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TDUnitSpawner : MonoBehaviour
{
    [SerializeField] UnitSO unitSO;
    [SerializeField] TowerSO towerSO;
    [SerializeField] TDCastle castle;
    private float maxSpawnCooldown = 3f;
    private float currentSpawnCooldown;
    private float currentUnitSpeed;
    private float currentUnitDamage;
    private float currentUnitHealth;
    private System.Random random;
    private int currentUnitCount = 0;
    private enum UnitSpawnState { IDLE,SPAWNING}
    private UnitSpawnState currentSpawnState=UnitSpawnState.SPAWNING;
    public event Action<Unit> OnUnitSpawned;
    public event Action<float> OnUnitDestroyed;
    public static TDUnitSpawner Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        currentSpawnCooldown = 0f;
        random = new System.Random();
    }
    private void Start()
    {
        TDWaveManager.Instance.GetUnitVariables(out currentUnitSpeed, out currentUnitDamage, out maxSpawnCooldown, out currentUnitHealth);
        TDWaveManager.Instance.OnFinishedWave += WaveManager_OnFinishedWave;
        TDWaveManager.Instance.OnStartedWave += WaveManager_OnStartedWave;
    }

    private void WaveManager_OnStartedWave()
    {
        currentSpawnState = UnitSpawnState.SPAWNING;
        TDWaveManager.Instance.GetUnitVariables(out currentUnitSpeed, out currentUnitDamage, out maxSpawnCooldown, out currentUnitHealth);
    }

    private void WaveManager_OnFinishedWave()
    {
        currentSpawnState = UnitSpawnState.IDLE;
    }

    private void Update()
    {
        switch (currentSpawnState)
        {
            case UnitSpawnState.IDLE:
                break;
            case UnitSpawnState.SPAWNING:
                currentSpawnCooldown -= Time.deltaTime;
                if (currentSpawnCooldown < 0 &&!TDWaveManager.Instance.IsOver())
                {
                    SpawnUnit();
                    currentSpawnCooldown = GetUnitSpawnCooldown();
                }
                break;
        }
    }
    private void SpawnUnit()
    {
        currentUnitCount++;
        Vector3 spawnPoint = WayPointPath.Instance.GetRandomPath(out List<Vector3> path);
        Transform unitTransform = Instantiate(unitSO.prefab, spawnPoint, Quaternion.identity);
        Unit unit = unitTransform.GetComponent<Unit>();
        unit.SetPath(path);
        unit.ToggleSelect(true);
        unit.SetSpeed(GetUnitSpeed());
        (unit as Soldier).SetAttackDamage(currentUnitDamage);
        unit.SetMaxHealth(currentUnitHealth);
        unit.OnDestroyed += Unit_OnDestroyed;
        OnUnitSpawned?.Invoke(unit);
        Transform selectedTransform = unitTransform.Find("Selected");
        Transform highlightedCellTransform = selectedTransform.Find("HighlightedCell");
        Destroy(highlightedCellTransform.gameObject);
    }

    private void Unit_OnDestroyed()
    {
        currentUnitCount--;
        OnUnitDestroyed?.Invoke(currentUnitHealth);
    }

    private float GetUnitSpeed()
    {
        return currentUnitSpeed + ((((float)random.NextDouble()) * currentUnitSpeed) - (currentUnitSpeed / 2f));
    }
    private float GetUnitSpawnCooldown()
    {
        return maxSpawnCooldown + ((((float)random.NextDouble()) * maxSpawnCooldown) - (maxSpawnCooldown / 2f));
    }
    public bool UnitsClear()
    {
        return currentUnitCount == 0;
    }
}
