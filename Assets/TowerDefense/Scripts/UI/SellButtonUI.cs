using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellButtonUI : MonoBehaviour
{
    [SerializeField] private Transform moneyBag;
    private void Start()
    {
        Button sellButton = GetComponent<Button>();
        moneyBag.gameObject.SetActive(false);
        sellButton.onClick.AddListener(() =>
        {
            ToggleSell();
        });
        ScreenInteractionManager.Instance.OnAreaSelected += OnAreaSelected;
    }

    private void OnAreaSelected(Vector3 arg1, Vector3 mousePosition)
    {
        if (moneyBag.gameObject.activeSelf)
        {
            Collider2D collider = Physics2D.OverlapPoint(mousePosition);
            if (collider != null)
            {
                if(collider.TryGetComponent(out TDTower tower))
                {
                    tower.Sell();
                }
            }
        }
    }

    private void Update()
    {
        moneyBag.transform.position = UtilsClass.GetMouseWorldPosition();
    }
    private void ToggleSell()
    {
        TDMouseObject.Instance.ToggleActive(moneyBag);
    }
}
