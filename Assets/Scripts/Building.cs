using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.VFX;
using static UnityEngine.ParticleSystem;
using UnityEngine.UI;
using System;

public class Building : Entity
{
    public BuildingSO buildingSO;
    private System.Random _random = new System.Random();

    public Transform SpawnForCheck(Vector3 position)
    {
        Transform visualTransform = Instantiate(buildingSO.buildingPrefab, position, Quaternion.identity);
        return visualTransform;
    }
    public void CheckAndSpawn(Transform visualTransform){

        GridManager.Instance.GetValue(visualTransform.position).GetIndices(out int I, out int J);
        bool[,] Visited = new bool[GridManager.Instance.GetWidth(), GridManager.Instance.GetHeight()];
        GameObject instantiatedObject = visualTransform.gameObject;
        BuildingManager.Instance.RecursiveCheck(I, J, Visited, this, out bool safe);
        Debug.Log("area is" + safe);
        Material material = instantiatedObject.GetComponent<Renderer>().material; // Access existing material

        // Adjust opacity values as needed

        material.SetColor("_Color", Color.red * 0.7f);
        if (!safe)
        {
            material.SetFloat("_Color.a", safe ? 0.5f : 0.2f);
        }
        if(!safe && Input.GetMouseButton (0))
        {

            Spawn(visualTransform.position);
            return;
        }


    }
    public override Entity Spawn(Vector3 position)
    {
        Instantiate(buildingSO.buildingPrefab, position, Quaternion.identity);
        BuildingManager.Instance.BuildAfterCheck(this);
        return this;
    }

public void Spawner(UnitSO unitSO)
    {
        if (unitSO != null && buildingSO.NeighborCells.Count > 0)
        {
            int randomIndex = _random.Next(0, buildingSO.NeighborCells.Count);
            Cell cell = buildingSO.NeighborCells[randomIndex];
            unitSO.unit.Spawn(GridManager.Instance.GridToWorldPositionCentered(cell.GetIndices()));
        }
    }

    public void SpawnerUI()
    {
        for (int i = 0; i < buildingSO.units.Count; i++)
        {
            GameObject unitButton = Instantiate(buildingSO.units.ToArray()[i].prefab.gameObject, buildingSO.buildingPrefab.position, Quaternion.identity);
            unitButton.GetComponent<Button>().onClick.AddListener(() => Spawner(buildingSO.units.ToArray()[i]));
        }
    }






    /* public override Entity Spawn(Cell cell, out bool built)
    {
        built = false;
        Indices indices = cell.GetIndices();
        Vector3 spawnPosition = GridManager.Instance.GridToWorldPosition(indices);
        Vector3 fixedSpawnPosition = new Vector3(
            spawnPosition.x + buildingSO.width,
            spawnPosition.y + buildingSO.height,
            0
        );
        built = SpawnBuilding(cell);
        if(built)
        {
            Instantiate(buildingSO.buildingPrefab, fixedSpawnPosition, Quaternion.identity);
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
        Vector3 pivotPosition = GridManager.Instance.GridToWorldPositionCentered(cell.GetIndices());
        Vector3 anchorPosition = new Vector3((int)(pivotPosition.x - buildingSO.anchorPosition.x), (int)(pivotPosition.y - buildingSO.anchorPosition.y), 0);
        //here we got the pivot position
        //position in calling achor cell are right
        //anchor cell is not getting called
        Cell anchorCell = GridManager.Instance.GetValue(anchorPosition);
        Debug.Log("anchor cell,w,h" + anchorCell + buildingSO.width + buildingSO.height);
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
    }*/

}
