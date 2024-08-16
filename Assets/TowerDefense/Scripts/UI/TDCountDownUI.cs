using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

public class TDCountDownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private void Awake()
    {
        TDGameManager.Instance.OnCountDownStarted += GameManager_OnCountDownStarted;
    }

    private void GameManager_OnCountDownStarted()
    {
        Show();
    }
    private void Update()
    {
        CountDown();
        ScaleLerp();
    }
    private void ScaleLerp()
    {
        float countDownNum = TDGameManager.Instance.GetCountdownTimer();
        float scaleVolume = 1f - (countDownNum - (int)countDownNum);
        if (scaleVolume > 0.5)
            return;
        float scaleLevel = Mathf.Sin(Mathf.PI * scaleVolume*2f)/4f;
        Vector3 newScale = new Vector3(1f + scaleLevel, 1f + scaleLevel, 1f);
        transform.localScale =newScale;
    }
    private void CountDown()
    {
        float countDownNum = TDGameManager.Instance.GetCountdownTimer();
        if (countDownNum > 1)
        {
            countdownText.text = ((int)countDownNum).ToString();
        }
        else if (countDownNum >= 0.09f)
        {
            countdownText.text = "Start!";
        }
        else
        {
            Hide();
        }
    }
    private void Hide()
    {
        gameObject.SetActive(false); 
    }


    private void Show()
    {
        gameObject.SetActive(true);
    }

}
