using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModesUI : MonoBehaviour
{
    [SerializeField] private Button strategy;
    [SerializeField] private Button towerDefense;
    [SerializeField] private Button back;
    private void Start()
    {
        strategy.onClick.AddListener(() =>
        {
            MainMenuUI.Instance.LoadRealTimeStrategy();
        });
        towerDefense.onClick.AddListener(() =>
        {
            MainMenuUI.Instance.LoadTowerDefense();
        });
        back.onClick.AddListener(() =>
        {
            MainMenuUI.Instance.SelectMainMenuList();
        });
    }
}
