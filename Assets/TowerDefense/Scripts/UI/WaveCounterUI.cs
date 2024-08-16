using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveCounterUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI waveCounter;
    private void Start()
    {
        TDGameManager.Instance.OnNextWave += GameManager_OnNextWave;
        waveCounter.text = "Wave 1" ;
    }

    private void GameManager_OnNextWave()
    {
        int currentWave = TDWaveManager.Instance.GetCurrentWave();
        waveCounter.text = "Wave " + currentWave.ToString();
    }

}
