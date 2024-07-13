using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDTower : MonoBehaviour
{
    public static TDTower Build(Transform prefab,Vector3 position)
    {
        Transform towerTransform=Instantiate(prefab,position,Quaternion.identity);
        TDTower tower = towerTransform.GetComponent<TDTower>();
        return tower;
    }
}
