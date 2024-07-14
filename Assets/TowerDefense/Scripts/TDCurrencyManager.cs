using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TDCurrencyManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currencyText;
    int currentCurrency = 0;
    private void Awake()
    {
        UpdateVisual();
    }
    private void Start()
    {
        TDUnitSpawner.Instance.OnUnitDestroyed += UnitSpawner_OnUnitDestroyed;
    }

    private void UnitSpawner_OnUnitDestroyed(float health)
    {
        AddToCurrency((int)(health / 5f));
    }
    private void AddToCurrency(int amount)
    {
        currentCurrency += amount;
        UpdateVisual();
    }
    private void UpdateVisual()
    {
        currencyText.text=currentCurrency.ToString();
    }
}
