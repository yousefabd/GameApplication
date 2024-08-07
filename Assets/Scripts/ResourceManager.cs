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
        resourceDictionary = new Dictionary<string, int>();
        resourceDictionary.Add("WOOD", 0);
        resourceDictionary.Add("GOLD", 10000);
        resourceDictionary.Add("STONE", 0);
    }
    private static Dictionary<string, int> resourceDictionary ;
    private string wood = "WOOD", gold="GOLD", stone="STONE";

   
    

    public static Action resourceChanged;
    public void updateResource(string key,int amount)
    {
        int oldAmount;
        resourceDictionary.TryGetValue(key, out oldAmount);
        resourceDictionary[key] += amount;
        resourceChanged?.Invoke();
    }
    public int getGoldResource()
    {
        return resourceDictionary[gold];
    }
    public int getWoodResource()
    {
        return resourceDictionary[wood];
    }
    public int getStoneResource()
    {
        return resourceDictionary[stone];
    }

}
