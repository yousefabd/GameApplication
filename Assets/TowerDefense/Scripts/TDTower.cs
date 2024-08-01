using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TDTower : MonoBehaviour
{
    [SerializeField] Transform passengerTransform;
    private Transform passenger;
    [SerializeField] UnitSO unitSO;
    [SerializeField] private TowerSO towerSO;
    public static TDTower Build(Transform prefab,Vector3 position)
    {
        Transform towerTransform=Instantiate(prefab,position,Quaternion.identity);
        TDTower tower = towerTransform.GetComponent<TDTower>();
        return tower;
    }
    public void SpawnUnit()
    {
        passenger = Instantiate(unitSO.prefab,passengerTransform.position,Quaternion.identity);
        passenger.AddComponent<TDDefender>();
    }
    public void Sell()
    {
        TDCurrencyManager.Instance.Buy(-1 * (towerSO.cost/5));
        Destroy(passenger.gameObject);
        Destroy(gameObject);
    }
}
