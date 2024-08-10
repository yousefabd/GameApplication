using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveCompleteChoices : MonoBehaviour
{
    [SerializeField] private Button shop;
    [SerializeField] private Button nextWave;

    private void Start()
    {
        TDWaveManager.Instance.OnFinishedWave += WaveManager_OnFinishedWave;
        shop.onClick.AddListener(() =>
        {
            TDPlayer.Instance.DisplayShop();
            gameObject.SetActive(false);
        });
        nextWave.onClick.AddListener(() =>
        {
            TDPlayer.Instance.NextWave();
            gameObject.SetActive(false);
        });
        gameObject.SetActive(false);
    }

    private void WaveManager_OnFinishedWave()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0.0f;
    }
}
