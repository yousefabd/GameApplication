using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreenUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameoverText;
    [SerializeField] private Button Restart;
    [SerializeField] private Button MainMenu;

    private void Start()
    {
        BuildingManager.Instance.lost += Instance_lost;
        Restart.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            Loader.Load(Loader.Scene.test);
        });
        MainMenu.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        gameObject.SetActive(false);
        
    }

    private void Instance_lost(Team lostTeam)
    {
        if(lostTeam == Team.HUMANS)
        {
            gameoverText.text = "You Lost!";
        }
        else
        {
            gameoverText.text = "You Won!";
        }
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
}
