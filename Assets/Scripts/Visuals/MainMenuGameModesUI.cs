using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuGameModesUI : MonoBehaviour
{
    [SerializeField] private Button StrategyBuilder;
    [SerializeField] private Button TowerDefense;
    [SerializeField] private Button Back;
    private void Start()
    {
        StrategyBuilder.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.test);
        });
        TowerDefense.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.TowerDefenseMode);
        });
        Back.onClick.AddListener(() =>
        {
            SwitchListUI.Instance.BackToMainMenu();
        });
    }
}
