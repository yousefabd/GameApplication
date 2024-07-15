using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TDPlayer : MonoBehaviour
{
    public static TDPlayer Instance { get; private set; }
    [SerializeField] private Transform slash;
    [SerializeField] private TowerSO towerSO;
    public event Action OnNextWave;
    public event Action<TowerSO> OnSelectTower;
    public event Action<TowerSO,Vector3> OnBuildTower;
    private float interactionRadius = 0.2f;
    private float damageValue = 20f;
    private TowerSO currentSelectedTower;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        ScreenInteractionManager.Instance.OnAreaSelected += ScreenInteractionManager_OnAreaSelected;
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
            OnBuildTower?.Invoke(currentSelectedTower,towerBase.transform.position);
            currentSelectedTower = null;
            return true;
        }
        return false;
    }
    private void DamageUnit(Vector3 position)
    {
        Instantiate(slash, position, Quaternion.identity);
        Collider2D collider=Physics2D.OverlapCircle(position, interactionRadius);
        if (collider == null)
            return;
        if(collider.TryGetComponent(out Unit unit))
        {
            if (unit.GetTeam() == Team.GOBLINS)
                unit.Damage(unit.transform.position, damageValue);
        }
    }
    private bool HasSelectedTower()
    {
        return currentSelectedTower != null;
    }
    public void DisplayShop()
    {
        Debug.Log("Display Shop");
    }

    public void NextWave()
    {
        OnNextWave?.Invoke();
    }

    public void SelectTower(TowerSO towerSO)
    {
        OnSelectTower?.Invoke(towerSO);
        currentSelectedTower = towerSO;
    }
}
