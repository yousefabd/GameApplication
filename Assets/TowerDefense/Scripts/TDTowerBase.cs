using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDTowerBase : MonoBehaviour
{
    private void Start()
    {
        TDPlayer.Instance.OnBuildTower += Player_OnBuild;
        SellButtonUI.Instance.OnSell += Instance_OnSell;
    }

    private void Instance_OnSell(Vector3 position)
    {
        if (position.Equals(transform.position))
        {
            gameObject.SetActive(true);
        }
    }

    private void Player_OnBuild(TowerSO towerSO,TDTowerBase towerBase)
    {
        if (towerBase.Equals(this))
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
