using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    private void Awake()
    {
        Instance = this;
        resourceDictionary = new Dictionary<ResourceType, int>();
        resourceDictionary.Add(ResourceType.WOOD, 0);
        resourceDictionary.Add(ResourceType.GOLD, 10000);
        resourceDictionary.Add(ResourceType.STONE, 0);
    }
    private static Dictionary<ResourceType, int> resourceDictionary ;

   
    

    public static Action resourceChanged;
    public void updateResource(ResourceType key,int amount)
    {
        int oldAmount;
        resourceDictionary.TryGetValue(key, out oldAmount);
        resourceDictionary[key] += amount;
        resourceChanged?.Invoke();
    }
    public int getGoldResource()
    {
        return resourceDictionary[ResourceType.GOLD];
    }
    public int getWoodResource()
    {
        return resourceDictionary[ResourceType.WOOD];
    }
    public int getStoneResource()
    {
        return resourceDictionary[ResourceType.STONE];
    }

}
