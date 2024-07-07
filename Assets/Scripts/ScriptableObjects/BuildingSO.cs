using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Building")]
public class BuildingSO : ScriptableObject
{
    public string stringName;
    public Transform buildingPrefab;
    public int width, height;
    public float health;
    public int goldStorage;

    public bool resourceGenerator = false;
    public ResourceGeneratorData resourceGeneratorData;

    public bool unitSpawner = false;
    public List<UnitSO> unitGenerationData = new List<UnitSO>();

    public bool unitEvolver = false;

    public bool defenseTower = false;

    public bool baseBuilding = false;

    public Building building { get { return buildingPrefab.GetComponent<Building>(); } }
    private List<Cell> neighborCells = new List<Cell>();
    public List<Cell> NeighborCells
    {
        get { return neighborCells; }
        set { neighborCells = value; }

    }

}
