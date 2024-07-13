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
    public event Action<Unit> OnUnitSpawned;
    public static TDUnitSpawner Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        currentSpawnCooldown = 0f;
    }
    private void Update()
    {
        currentSpawnCooldown -= Time.deltaTime;
        if (currentSpawnCooldown < 0)
        {
            SpawnUnit();
            currentSpawnCooldown = maxSpawnCooldown;
        }
    }
    private void SpawnUnit()
    {
        Vector3 spawnPoint = WayPointPath.Instance.GetRandomPath(out List<Vector3> path);
        Transform unitTransform = Instantiate(unitSO.prefab, spawnPoint, Quaternion.identity);
        unitTransform.localScale = new Vector3(0.6f,0.6f,1f);
        Unit unit = unitTransform.GetComponent<Unit>();
        unit.SetPath(path);
        unit.ToggleSelect(true);
        OnUnitSpawned(unit);
        Transform selectedTransform = unitTransform.Find("Selected");
        Transform highlightedCellTransform = selectedTransform.Find("HighlightedCell");
        Destroy(highlightedCellTransform.gameObject);
    }
}
