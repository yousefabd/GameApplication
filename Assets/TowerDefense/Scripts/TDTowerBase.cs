using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDTowerBase : MonoBehaviour
{
    private void Start()
    {
        TDPlayer.Instance.OnBuildTower += Player_OnBuild;
    }

    private void Player_OnBuild(TowerSO towerSO,Transform towerBaseTransform)
    {
        if (towerBaseTransform.Equals(transform))
        {
            Build(towerSO);
        }
    }
    private void Build(TowerSO towerSO)
    {
        TDTower tower =TDTower.Build(towerSO.prefab, transform);
        tower.SpawnUnit();
        gameObject.SetActive(false);
    }
}
