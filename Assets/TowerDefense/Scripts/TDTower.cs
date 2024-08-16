using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TDTower : MonoBehaviour
{
    [SerializeField] Transform passenger;
    [SerializeField] UnitSO unitSO;
    [SerializeField] TowerSO towerSO;
    public Transform towerBaseTransform;
    public Transform unitTransform;
    public event Action OnBeginDestroy;
    public static TDTower Build(Transform prefab,Transform towerBaseTransform)
    {
        Transform towerTransform=Instantiate(prefab,towerBaseTransform.position,Quaternion.identity);
        TDTower tower = towerTransform.GetComponent<TDTower>();
        tower.towerBaseTransform = towerBaseTransform;
        return tower;
    }
    public void SpawnUnit()
    {
        Transform unitTransform =Instantiate(unitSO.prefab,passenger.position,Quaternion.identity);
        unitTransform.AddComponent<TDDefender>();
        this.unitTransform = unitTransform;
    }
    public void Sell()
    {
        TDCurrencyManager.Instance.Buy(-1 * (towerSO.cost/4));
        Destroy(gameObject);
        Destroy(unitTransform.gameObject);
        OnBeginDestroy?.Invoke();
        towerBaseTransform.gameObject.SetActive(true);
    }
}
