using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhostVisual : MonoBehaviour
{
    public static BuildingGhostVisual Instance { get; private set; }
    private void Start()
    {
        Instance = this;
        TDPlayer.Instance.OnSelectTower += Player_OnSelectTower;
        TDPlayer.Instance.OnBuildTower += Player_OnBuildTower;
        gameObject.SetActive(false);
    }

    private void Player_OnBuildTower(TowerSO arg1, TDTowerBase arg2)
    {
        gameObject.SetActive(false);
    }
    private void Update()
    {
        transform.position = ScreenInteractionManager.Instance.GetCurrentMousePosition();
    }
    private void Player_OnSelectTower(TowerSO towerSO)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = towerSO.prefab.GetComponent<SpriteRenderer>().sprite;
        gameObject.transform.position = ScreenInteractionManager.Instance.GetCurrentMousePosition();
        gameObject.SetActive(true);
    }
}
