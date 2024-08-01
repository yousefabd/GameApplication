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
    }
    private static Dictionary<string, int> resourceDictionary ;
    private string wood = "WOOD", gold="GOLD", stone="STONE";

   
    private void Start()
    {
        resourceDictionary = new Dictionary<string, int>();
        resourceDictionary.Add(wood, 0);
        resourceDictionary.Add(gold, 0);
        resourceDictionary.Add(stone, 0);
    }

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
