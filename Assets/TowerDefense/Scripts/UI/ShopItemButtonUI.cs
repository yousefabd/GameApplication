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
    private int cost;
    private Button attachedButton;
    private void Awake()
    {
        attachedButton = GetComponent<Button>();
    }
    private void Update()
    {
        attachedButton.interactable = TDCurrencyManager.Instance.CanBuy(cost);
    }
    public Button SetButton(int cost,string description)
    {
        costText.text = cost.ToString();
        this.cost = cost;
        this.description.text = description;
        gameObject.SetActive(true);
        return attachedButton;
    }
}
