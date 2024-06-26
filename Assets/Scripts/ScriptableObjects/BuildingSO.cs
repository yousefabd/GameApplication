using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Building")]
public class BuildingSO : ScriptableObject 
{
    public string stringName;
    public Transform buildingPrefab;
    public int width,height;
    public Vector2 anchorPosition;
    public Building building { get { return buildingPrefab.GetComponent<Building>(); } }
    public ResourceGeneratorData resourceGeneratorData;
    private List<Cell> neighborCells = new List<Cell>();
    public List<Cell> NeighborCells
    {
        get { return neighborCells; }
        set { neighborCells = value; }

    }

}
