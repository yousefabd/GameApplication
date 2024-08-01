using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TDCountDownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    private void Start()
    {
        TDGameManager.Instance.OnCountDownStarted += Instance_OnCountDownStarted;
    }

    private void Instance_OnCountDownStarted()
    {
        gameObject.SetActive(true);
    }

    private void Update()
    {
        float countDownTimer = TDGameManager.Instance.GetCountdownTimer();
        if (countDownTimer >= 1)
        {
            countdownText.text = ((int)countDownTimer).ToString();
        }
        else if(countDownTimer >= 0.1f )
        {
            countdownText.text = "Start!";
        }
        else
        {
            gameObject.SetActive(false);
        }
        
    }
}
