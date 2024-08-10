using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonTextController : MonoBehaviour
{

    public TMP_Text counter;
    public TMP_Text price;
    private int currentValue;
    private int maxValue;
    public BuildingType buildingType;
    public int buildingPrice;
    private void Start()
    {
       // Debug.Log(buildingType);
        //Debug.Log(Player.Instance.gameRules.buildingCount[buildingType]);
        maxValue = Player.Instance.gameRules.buildingCount[buildingType];
        //Debug.Log(maxValue);
        currentValue = Player.Instance.currentBuildingCount[buildingType];
        UpdateText();
        price.text = buildingPrice.ToString();
        BuildingManager.Instance.built += Building_built;
    }

    private void Update()
    {
        if(buildingPrice > ResourceManager.Instance.getGoldResource())
        {
            price.color = Color.red;
        }
        else
        {
            price.color = Color.black;
        }
    }

    private void Building_built(Building building)
    {
        Debug.Log("buttons works");
     if(building.buildingSO.buildingType == buildingType)
        {
            IncrementCounter();
        }
    }

    private void UpdateText()
    {
        counter.text = $"{currentValue}/{maxValue}";
    }

    public void IncrementCounter()
    {
        if (currentValue < maxValue)
        {
            currentValue++;
            Player.Instance.currentBuildingCount[buildingType] = currentValue;
            UpdateText();
        }
        else
        {
            Debug.Log("Maximum building count reached!");
        }
    }

    public void ResetCounter()
    {
        currentValue = 0;
        UpdateText();
    }

}
