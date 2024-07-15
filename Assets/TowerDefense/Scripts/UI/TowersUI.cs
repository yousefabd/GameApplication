using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TowersUI : MonoBehaviour
{
    [SerializeField] private Button archer;
    [SerializeField] private TowerSO archerSO;
    [SerializeField] private Button wizard;
    [SerializeField] private TowerSO wizardSO;
    private int archerCost;
    private int wizardCost;
    private void Start()
    {
        archer.interactable = TDCurrencyManager.Instance.CanBuy(archerCost);
        wizard.interactable = TDCurrencyManager.Instance.CanBuy(wizardCost);
        archer.onClick.AddListener(() =>
        {
            TDPlayer.Instance.SelectTower(archerSO);
        });
        wizard.onClick.AddListener(() =>
        {
            TDPlayer.Instance.SelectTower(wizardSO);
        });
        archerCost = 100;
        wizardCost = 5000;
    }
    private void Update()
    {
        archer.interactable = TDCurrencyManager.Instance.CanBuy(archerCost);
        wizard.interactable = TDCurrencyManager.Instance.CanBuy(wizardCost);
    }
}
