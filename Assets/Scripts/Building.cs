using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;
using static UnityEngine.ParticleSystem;

public class Building : Entity
{
    public BuildingSO buildingSO;
    public override Entity Spawn(Cell cell, out bool built)
    {
        built = false;
        Indices indices = cell.GetIndices();
        Vector3 spawnPosition = GameManager.Instance.GridToWorldPosition(indices);
        built = SpawnBuilding(cell);
        if(built)
        {
            Instantiate(buildingSO.buildingPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("did build" + this);
            return this;
        }
        else
        {
            Debug.Log("didn't build");
            return null;
        }


    }
    public bool SpawnBuilding(Cell cell)
    {
        Vector3 pivotPosition = GameManager.Instance.GridToWorldPositionCentered(cell.GetIndices());
        Cell anchorCell = GameManager.Instance.gridMap.GetValue((int)(pivotPosition.x - buildingSO.anchorPosition.x), (int)(pivotPosition.y - buildingSO.anchorPosition.y));
        bool canBuild = BuildingManager.Instance.isOccupied(anchorCell, buildingSO.width, buildingSO.height);
        if (canBuild)
        {
            Debug.Log("Could Build");
            BuildingManager.Instance.Build(anchorCell, buildingSO.width, buildingSO.height, this);
            return true;
        }
        else
        {
            Debug.Log("couldn't build");
            return false;
        }
    }

}
    