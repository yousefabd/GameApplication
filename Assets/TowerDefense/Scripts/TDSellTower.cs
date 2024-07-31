using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDSellTower : MonoBehaviour
{
    private void Start()
    {
        ScreenInteractionManager.Instance.OnAreaSelected += Instance_OnAreaSelected;
        SellButtonUI.Instance.OnClickBag += SellButton_OnSell;
        gameObject.SetActive(false);
    }

    private void SellButton_OnSell()
    {
        gameObject.transform.position = ScreenInteractionManager.Instance.GetCurrentMousePosition();
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void Instance_OnAreaSelected(Vector3 arg1, Vector3 mousePosition)
    {
        Collider2D collider = Physics2D.OverlapPoint(mousePosition);
        if (collider == null) { }
        else if(collider.TryGetComponent(out TDTower tower) && gameObject.activeSelf)
        {
            SellButtonUI.Instance.Sell(tower.transform.position);
            tower.Sell();
            gameObject.SetActive(false); 
        }
    }

    private void Update()
    {
        transform.position = ScreenInteractionManager.Instance.GetCurrentMousePosition();
    }
}
