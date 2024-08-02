using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "ScriptableObjects/Game Rules")]
public class GameRules : ScriptableObject
{
    public int NoneCount;
    public int ResourceGeneratorCount;
    public int UnitSpawnerCount;
    public int UnitEvolverCount;
    public int DefenseTowerCount;
    public int BaseBuildingCount;

    // Dictionary to hold the counts of each building type
    public Dictionary<BuildingType, int> buildingCount = new Dictionary<BuildingType, int>();

    private void OnEnable()
    {
        // Initialize the dictionary with the counts
        buildingCount[BuildingType.None] = NoneCount;
        buildingCount[BuildingType.ResourceGenerator] = ResourceGeneratorCount;
        buildingCount[BuildingType.UnitSpawner] = UnitSpawnerCount;
        buildingCount[BuildingType.UnitEvolver] = UnitEvolverCount;
        buildingCount[BuildingType.DefenseTower] = DefenseTowerCount;
        buildingCount[BuildingType.BaseBuilding] = BaseBuildingCount;
    }
}
