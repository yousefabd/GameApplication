using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDTowerBase : MonoBehaviour
{
    private void Start()
    {
        TDPlayer.Instance.OnBuildTower += Player_OnBuild;
    }

    private void Player_OnBuild(TowerSO towerSO,Vector3 position)
    {
        if (position == transform.position)
        {
            Build(towerSO);
        }
    }
    private void Build(TowerSO towerSO)
    {
        TDTower tower =TDTower.Build(towerSO.prefab, transform.position);
        tower.SpawnUnit();
        gameObject.SetActive(false);
    }
}
