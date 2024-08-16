using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonTextController : MonoBehaviour
{

    public TMP_Text counter;
    public TMP_Text price;
    public TMP_Text wood;
    public TMP_Text stone;
    private int currentValue;
    private int maxValue;
    public BuildingType buildingType;
    public int buildingPrice;
    public int buildingWood;
    public int buildingStone;
    private void Start()
    {
       // Debug.Log(buildingType);
        //Debug.Log(Player.Instance.gameRules.buildingCount[buildingType]);
        maxValue = Player.Instance.gameRules.buildingCount[buildingType];
        //Debug.Log(maxValue);
        currentValue = Player.Instance.currentBuildingCount[buildingType];
        UpdateText();
        price.text = buildingPrice.ToString();
        wood.text = buildingWood.ToString();
        stone.text = buildingStone.ToString();
        BuildingManager.Instance.built += Building_built;
    }

    private void Update()
    {
        if(buildingPrice > ResourceManager.Instance.getGoldResource() && buildingWood > ResourceManager.Instance.getWoodResource() && buildingStone > ResourceManager.Instance.getStoneResource())
        {
            price.color = Color.red;
            wood.color = Color.red;
            stone.color = Color.red;
        }
        else
        {
            price.color = Color.black;
            wood.color = Color.black;
            stone.color = Color.black;  
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
