using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverOptionsUI : MonoBehaviour
{
    [SerializeField] private Button Restart;
    [SerializeField] private Button MainMenu;

    private void Start()
    {
        Restart.onClick.AddListener(() =>
        {
            TDWaveManager.Instance.Restart();
            Loader.Load(Loader.Scene.TowerDefenseMode);
        });
        MainMenu.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }
}
