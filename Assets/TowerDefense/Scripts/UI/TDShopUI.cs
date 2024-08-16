using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TDShopUI : MonoBehaviour
{
    [SerializeField] private Transform shopItemTemplate;
    [SerializeField] private Transform items;
    [SerializeField] private Button backButton;
    private void Start()
    {
        CreateShopItems();
        TDPlayer.Instance.OnDisplayShop += Player_OnDisplayShop;
        shopItemTemplate.gameObject.SetActive(false);
        backButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1.0f;
            gameObject.SetActive(false);    
            TDGameManager.Instance.NextWave();
        });
        gameObject.SetActive(false);
    }

    private void Player_OnDisplayShop()
    {
        gameObject.SetActive(true);
    }

    private void CreateShopItems()
    {
        Button repair =CreateItem(100, "Repair Castle");
        repair.onClick.AddListener(() =>
        {  
            if (TDCastle.Instance.Repair(100))
            {
                TDCurrencyManager.Instance.Buy(100);
            }
        });
        Button fortify = CreateItem(1000, "Fortify Castle");
        fortify.onClick.AddListener(() =>
        {
            TDCastle.Instance.Fortify();
            TDCurrencyManager.Instance.Buy(1000);
        });
        Button cursor = CreateItem(1300, "Increase cursor damage",1);
        cursor.onClick.AddListener(() =>
        {
            TDPlayer.Instance.UpgradeCursor();
            TDCurrencyManager.Instance.Buy(1300);
        });
        Button multidamage = CreateItem(2500, "Cursor Multi-Damage", 1);
        multidamage.onClick.AddListener(() =>
        {
            TDPlayer.Instance.CursorMultiTarget();
            TDCurrencyManager.Instance.Buy(2500);
        });
        Button doubleincome = CreateItem(2500, "Double Income", 2);
        doubleincome.onClick.AddListener(() =>
        {
            TDWaveManager.Instance.DoubleIncome();
            TDCurrencyManager.Instance.Buy(2500);
        });
    }
    private Button CreateItem(int cost,string description,int allowedTimes = int.MaxValue)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, items);
        ShopItemButtonUI shopItem=shopItemTransform.GetComponent<ShopItemButtonUI>();
        return shopItem.SetButton(cost, description,allowedTimes);
    }
}
