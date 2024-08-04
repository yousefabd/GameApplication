using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class TDCurrencyManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currencyText;
    public static TDCurrencyManager Instance {  get; private set; }
    int currentCurrency = 0;
    private void Awake()
    {
        Instance = this;
        UpdateVisual();
    }
    private void Start()
    {
        TDUnitSpawner.Instance.OnUnitDestroyed += UnitSpawner_OnUnitDestroyed;
    }

    private void UnitSpawner_OnUnitDestroyed(float health)
    {
        AddToCurrency(TDWaveManager.Instance.GetUnitKillPrize((int)health));
    }
    private void AddToCurrency(int amount)
    {
        currentCurrency += amount;
        UpdateVisual();
    }
    private void UpdateVisual()
    {
        if (currentCurrency > 999)
            currencyText.fontSize = 36f;
        else currencyText.fontSize = 50f;
        currencyText.text=currentCurrency.ToString();
    }
    public bool CanBuy(int amount)
    {
        return amount <= currentCurrency;
    }
    public void Buy(int amount)
    {
        currentCurrency -= amount;
        UpdateVisual();
    }
}
