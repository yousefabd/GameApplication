using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Housing : MonoBehaviour
{
    private void Start()
    {
        if(TryGetComponent<Building>(out Building building) && building.buildingSO.buildingType == BuildingType.UnitEvolver)
        {
            Player.currentMaxCount[SoldierType.SWORDSMAN] += 10;
            Player.currentMaxCount[SoldierType.RANGER] += 10;

        }
    }
}
