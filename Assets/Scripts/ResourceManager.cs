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
        resourceDictionary.Add(ResourceType.WOOD, 200);
        resourceDictionary.Add(ResourceType.GOLD, 10000);
        resourceDictionary.Add(ResourceType.STONE, 50);

        goblinResourceDictionary = new Dictionary<ResourceType, int>();
        goblinResourceDictionary.Add(ResourceType.WOOD, 200);
        goblinResourceDictionary.Add(ResourceType.GOLD,10000);
        goblinResourceDictionary.Add(ResourceType.STONE, 50); 
    }
    private static Dictionary<ResourceType, int> resourceDictionary ;
    private static Dictionary<ResourceType, int> goblinResourceDictionary ;

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
    public Dictionary<ResourceType, int> GetGoblinsResources() 
    { 
       return goblinResourceDictionary;
    }
    public int getGoblinGoldResource()
    {
        return goblinResourceDictionary[ResourceType.GOLD];
    }
    public int getGoblinWoodResource()
    {
        return goblinResourceDictionary[ResourceType.WOOD];
    }
    public int getGoblinStoneResource()
    {
        return goblinResourceDictionary[ResourceType.STONE];
    }

}
