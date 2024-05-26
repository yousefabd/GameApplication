using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;
using static UnityEngine.ParticleSystem;

public class Building : Entity
{
    public BuildingSO buildingSO;

    public override Entity Spawn(Vector3 position)
    {
        GridManager.Instance.GetValue(position).GetIndices(out int I, out int J);
        bool[,] Visited = new bool[GridManager.Instance.GetWidth(), GridManager.Instance.GetHeight()];
        BuildingManager.Instance.RecursiveCheck(I, J, Visited, this, out bool safe);

        // Instantiate the building prefab and get the instantiated GameObject
        GameObject instantiatedObject = Instantiate(buildingSO.buildingPrefab, position, Quaternion.identity).gameObject;

        // Modify material properties based on safety check
        Material material = instantiatedObject.GetComponent<Renderer>().material; // Access existing material

        material.SetFloat("_Color.a", safe ? 0.5f : 0.2f); // Adjust opacity values as needed

        // Optional red tint if unsafe
        if (!safe)
        {
            material.SetColor("_Color", Color.red * 0.7f); // Adjust red tint intensity (0.7f for 70% opacity)
        }
        return this;
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
