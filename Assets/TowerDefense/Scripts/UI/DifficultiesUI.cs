using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultiesUI : MonoBehaviour
{
    [SerializeField] private Button easy;
    [SerializeField] private Button medium;
    [SerializeField] private Button hard;

    private void Start()
    {
        easy.onClick.AddListener(() =>
        {
            TDWaveManager.Instance.OnSetGameDifficulty(GameDifficulty.EASY);
            TDGameManager.Instance.StartGame();
            gameObject.SetActive(false);
        });
        medium.onClick.AddListener(() =>
        {
            TDWaveManager.Instance.OnSetGameDifficulty(GameDifficulty.MEDIUM);
            TDGameManager.Instance.StartGame();
            gameObject.SetActive(false);
        });
        hard.onClick.AddListener(() =>
        {
            TDWaveManager.Instance.OnSetGameDifficulty(GameDifficulty.HARD);
            TDGameManager.Instance.StartGame();
            gameObject.SetActive(false);
        });
    }
}
