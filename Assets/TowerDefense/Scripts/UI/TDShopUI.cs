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
        TDGameManager.Instance.OnDisplayShop += Player_OnDisplayShop;
        shopItemTemplate.gameObject.SetActive(false);
        backButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            Time.timeScale = 1.0f;
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
        Button repair =CreateItem(500, "Repair Castle");
        repair.onClick.AddListener(() =>
        {  
            if (TDCastle.Instance.Repair(100))
            {
                TDCurrencyManager.Instance.Buy(500);
            }
        });
        CreateItem(1000, "Fortify Castle");
        CreateItem(1300, "Increase cursor damage");
    }
    private Button CreateItem(int cost,string description)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, items);
        ShopItemButtonUI shopItem=shopItemTransform.GetComponent<ShopItemButtonUI>();
        return shopItem.SetButton(cost, description);
    }
}
