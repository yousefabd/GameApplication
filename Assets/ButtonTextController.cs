using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonTextController : MonoBehaviour
{

    public TMP_Text counter;
    private int currentValue = 0;
    private int maxValue;
    public BuildingType buildingType;

    private void Start()
    {
        maxValue = Player.Instance.gameRules.buildingCount[buildingType];
        UpdateText();
        Building.built += Building_built;
    }

    private void Building_built(Building building)
    {
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
