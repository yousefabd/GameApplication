using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class Building : Entity
{
    public BuildingSO buildingSO;
    public override void Spawn(Cell cell, out bool built)
    {
        built = false;
        Indices indices = cell.GetIndices();
        Vector3 spawnPosition = GridManager.Instance.GridToWorldPosition(indices);
        bool canBuild = false;//GridManager.Instance.gridMap.CanBuild(cell, buildingSO.width, buildingSO.height);
        Debug.Log(canBuild);
        if (canBuild)
        {
            Debug.Log("before Build");
            //GridManager.Instance.gridMap.Build(cell, buildingSO.width, buildingSO.height, this);
            Instantiate(buildingSO.buildingPrefab, spawnPosition, Quaternion.identity);
            built = true;
        }
        else
        {
            built = false;
            Debug.Log("couldn't build");
        }
    }

}
