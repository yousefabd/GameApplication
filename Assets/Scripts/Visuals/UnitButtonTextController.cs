using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitButtonTextController : MonoBehaviour
{
    public TMP_Text unitCountText;
    public TMP_Text unitPriceText;
    private int currentUnitCount;
    private int maxUnitCount;
    public SoldierType soldierType;
    public int unitPrice;
    private void Start()
    {
        maxUnitCount = Player.currentMaxCount[soldierType];
        currentUnitCount = Player.Instance.currentCount[soldierType];
        UpdateText();
        unitPriceText.text = unitPrice.ToString();
       
    }

   
    private void Update()
    {
        if (unitPrice > ResourceManager.Instance.getGoldResource())
        {
            unitPriceText.color = Color.red;
        }
        else
        {
            unitPriceText.color = Color.black;
        }
        maxUnitCount = Player.currentMaxCount[soldierType];
        currentUnitCount = Player.Instance.currentCount[soldierType];
        UpdateText();
    }

 

    private void UpdateText()
    {
        unitCountText.text = $"{currentUnitCount}/{maxUnitCount}";
    }

  




}
