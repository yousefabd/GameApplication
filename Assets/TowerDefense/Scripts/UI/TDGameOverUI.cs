using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TDGameOverUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI waveCount;
    [SerializeField] private Button Restart;
    [SerializeField] private Button MainMenu;
    private void Start()
    {
        TDCastle.Instance.OnGameOver += Castle_OnGameOver;
        gameObject.SetActive(false);
        Restart.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            RestartGame();
        });
        MainMenu.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            ToMainMenu();
        });
    }
    private void Castle_OnGameOver(int waves)
    {
        waveCount.text = waves.ToString();
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
    private void RestartGame()
    {
        Loader.Load(Loader.Scene.TowerDefenseMode);
    }
    private void ToMainMenu()
    {
        Loader.Load(Loader.Scene.MainMenuScene);
    }
}
