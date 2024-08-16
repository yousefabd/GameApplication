using Assets.HeroEditor4D.Common.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI max;
    private int cost;
    private int allowedTimes;
    private Button attachedButton;
    private void Awake()
    {
        attachedButton = GetComponent<Button>();
        attachedButton.onClick.AddListener(() =>
        {
            Trigger();
        });
    }
    private void Start()
    {
        max.gameObject.SetActive(false);
    }
    private void Update()
    {
        attachedButton.interactable = TDCurrencyManager.Instance.CanBuy(cost) && allowedTimes > 0;
        if(allowedTimes == 0)
        {
            max.gameObject.SetActive(true);
        }
    }
    public Button SetButton(int cost,string description,int allowedTimes = int.MaxValue)
    {
        costText.text = cost.ToString();
        this.cost = cost;
        this.description.text = description;
        this.allowedTimes = allowedTimes;
        gameObject.SetActive(true);
        return attachedButton;
    }
    public void Trigger()
    {
        if (allowedTimes != int.MaxValue)
        {
            allowedTimes--;
        }
    }
}
