using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TDTower : MonoBehaviour
{
    [SerializeField] Transform passenger;
    [SerializeField] UnitSO unitSO;
    public static TDTower Build(Transform prefab,Vector3 position)
    {
        Transform towerTransform=Instantiate(prefab,position,Quaternion.identity);
        TDTower tower = towerTransform.GetComponent<TDTower>();
        return tower;
    }
    public void SpawnUnit()
    {
        Transform unitTransform =Instantiate(unitSO.prefab,passenger.position,Quaternion.identity);
        unitTransform.AddComponent<TDDefender>();
    }
}
