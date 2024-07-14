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
    private float damageRadius = 0.2f;
    private float damageValue = 20f;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        ScreenInteractionManager.Instance.OnAreaSelected += ScreenInteractionManager_OnAreaSelected;
        TDTower.Build(towerSO.prefab, Vector3.zero);
    }

    private void ScreenInteractionManager_OnAreaSelected(Vector3 arg1, Vector3 mousePosition)
    {
        DamageUnit(mousePosition);
    }

    private void DamageUnit(Vector3 position)
    {
        Instantiate(slash, position, Quaternion.identity);
        Collider2D collider=Physics2D.OverlapCircle(position, damageRadius);
        if (collider == null)
            return;
        if(collider.TryGetComponent(out Unit unit))
        {
            if (unit.GetTeam() == Team.GOBLINS)
                unit.Damage(unit.transform.position, damageValue);
        }
    }
    public void DisplayShop()
    {
        Debug.Log("Display Shop");
    }

    public void NextWave()
    {
        OnNextWave?.Invoke();
    }
}
