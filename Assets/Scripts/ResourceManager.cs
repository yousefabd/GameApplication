using System;
using System.Collections.Generic;
using UnityEngine;
using static Resource;
public class ResourceManager : MonoBehaviour
{

    public static ResourceManager Instance { get; private set; }

    public event EventHandler OnResourceAmountChanged;

    private Dictionary<ResourceTypeSO, int> resourceAmountDictionary;
    private ResourceTypeListSO resourceTypeList;

  

    private void Awake()
    {
        Instance = this;

        resourceAmountDictionary = new Dictionary<ResourceTypeSO, int>();

        resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        foreach (ResourceTypeSO resourceTypeSO in resourceTypeList.list)
        {
            resourceAmountDictionary[resourceTypeSO] = 0;

        }


    }
    private void Update()
    {
        foreach (ResourceTypeSO resourceType in resourceAmountDictionary.Keys)
        {
            Debug.Log(resourceAmountDictionary[resourceType]);
        }
    }

    public void AddResource(ResourceTypeSO resourceType, int amount)
    {
        resourceAmountDictionary[resourceType] += amount;
        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);

    }
    public int GetResourceAmount(ResourceTypeSO resourceType)
    {
        return resourceAmountDictionary[resourceType];

    }
    public int GetGoldAmount()
    {
        return resourceAmountDictionary[resourceTypeList.list[0]];

    }
    public int GetWoodAmount()
    {   
        return resourceAmountDictionary[resourceTypeList.list[1]];

    }

    public int GetStoneAmount()
    {
        return resourceAmountDictionary[resourceTypeList.list[2]];

    }

}


