using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TowersUI : MonoBehaviour
{
    [SerializeField] private Button archer;
    [SerializeField] private TowerSO archerSO;
    [SerializeField] private TextMeshProUGUI archerCost;
    [SerializeField] private Button wizard;
    [SerializeField] private TowerSO wizardSO;
    [SerializeField] private TextMeshProUGUI wizardCost;
    [SerializeField] private Button sniper;
    [SerializeField] private TowerSO sniperSO;
    [SerializeField] private TextMeshProUGUI sniperCost;
    private void Start()
    {
        SetCostText();
        archer.onClick.AddListener(() =>
        {
            TDPlayer.Instance.SelectTower(archerSO);
        });
        wizard.onClick.AddListener(() =>
        {
            TDPlayer.Instance.SelectTower(wizardSO);
        });
        sniper.onClick.AddListener(() =>
        {
            TDPlayer.Instance.SelectTower(sniperSO);
        });
    }
    private void SetCostText()
    {
        archerCost.text = archerSO.cost.ToString();
        wizardCost.text = wizardSO.cost.ToString();
        sniperCost.text = sniperSO.cost.ToString();
    }
    private void Update()
    {
        UpdateButton();
    }
    private void UpdateButton()
    {
        archer.interactable = TDCurrencyManager.Instance.CanBuy(archerSO.cost);
        wizard.interactable = TDCurrencyManager.Instance.CanBuy(wizardSO.cost);
        sniper.interactable = TDCurrencyManager.Instance.CanBuy(sniperSO.cost);
    }
}
