using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TDGameOverUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI waveCount;

    private void Start()
    {
        TDCastle.Instance.OnGameOver += Castle_OnGameOver;
        TDWaveManager.Instance.OnRestart += TDWaveManager_OnRestart;
        gameObject.SetActive(false);
    }
    private void Castle_OnGameOver(int waves)
    {
        waveCount.text = waves.ToString();
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
    private void TDWaveManager_OnRestart()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
