using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TDPlayer : MonoBehaviour
{
    public static TDPlayer Instance { get; private set; }
    [SerializeField] private Transform normalSlash;
    [SerializeField] private Transform upgradedSlash;
    private Transform slash;
    [SerializeField] private TowerSO towerSO;
    public event Action OnNextWave;
    public event Action<TowerSO> OnSelectTower;
    public event Action<TowerSO,Transform> OnBuildTower;
    public event Action OnDisplayShop;
    public event Action OnDeselectTower;
    private float interactionRadius = 0.5f;
    private float damageValue = 20f;
    private TowerSO currentSelectedTower;
    private int damagedUnits = 1;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        ScreenInteractionManager.Instance.OnAreaSelected += ScreenInteractionManager_OnAreaSelected;
        slash = normalSlash;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TDCurrencyManager.Instance.Buy(-5000);
        }
    }

    private void ScreenInteractionManager_OnAreaSelected(Vector3 arg1, Vector3 mousePosition)
    {
        if (HasSelectedTower())
        {
            BuildTower(mousePosition);
        }
        else
        {
            DamageUnit(mousePosition);
        }
    }
    private bool BuildTower(Vector3 position)
    {
        Collider2D collider = Physics2D.OverlapCircle(position, interactionRadius);
        if (collider == null)
            return false;
        if(collider.TryGetComponent(out TDTowerBase towerBase))
        {
            OnBuildTower?.Invoke(currentSelectedTower,towerBase.transform);
            TDCurrencyManager.Instance.Buy(currentSelectedTower.cost);
            currentSelectedTower = null;
            return true;
        }
        return false;
    }
    private void DamageUnit(Vector3 position)
    {
        Instantiate(slash, position, Quaternion.identity);
        Collider2D []colliderArray=Physics2D.OverlapCircleAll(position, interactionRadius);
        if (colliderArray == null)
            return;
        int minIndex = Mathf.Min(colliderArray.Length, damagedUnits);
        for(int i = 0; i < minIndex; i++)
        {
            if (colliderArray[i].TryGetComponent(out Unit unit))
            {
                if (unit.GetTeam() == Team.GOBLINS)
                    unit.Damage(unit.transform.position, damageValue);
            }
        }
    }
    private bool HasSelectedTower()
    {
        return currentSelectedTower != null;
    }
    public void DisplayShop()
    {
        Debug.Log("Display Shop");
        OnDisplayShop?.Invoke();
    }

    public void NextWave()
    {
        OnNextWave?.Invoke();
    }

    public void SelectTower(TowerSO towerSO)
    {
        if (towerSO != currentSelectedTower)
        {
            OnSelectTower?.Invoke(towerSO);
            currentSelectedTower = towerSO;
        }
        else
        {
            OnDeselectTower?.Invoke();
            currentSelectedTower = null;
        }
    }
    public void UpgradeCursor()
    {
        damageValue = 60f;
    }

    public void CursorMultiTarget()
    {
        interactionRadius = 0.9f;
        damagedUnits = 3;
        slash = upgradedSlash;
    }
}
