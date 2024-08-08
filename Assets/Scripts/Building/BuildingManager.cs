using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Building;

public class BuildingManager : MonoBehaviour
{
    //singleton pattern
    public static BuildingManager Instance { get; private set; }


    private Camera mainCamera;
    private Building building;
    private List<Cell> BuildingCells = new List<Cell>();
    private Transform visualTransform;

    public event Action<Building> built;

    public void onBuilt(Building building)
    {
        built?.Invoke(building);
    }
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (building != null)
        {
            if (visualTransform != null)
            {
                visualTransform.position = GetMouseWorldPosition();
                CheckAndSpawn(visualTransform);
                //building = null;
            }

        }

    }

    // Functionality for UI
    public void UIHelper(BuildingSO buildingSO)
    {
        if (buildingSO.price <= ResourceManager.Instance.getGoldResource() && (Player.Instance.currentBuildingCount[buildingSO.buildingType] < Player.Instance.gameRules.buildingCount[buildingSO.buildingType]))
        {
            if (visualTransform != null && !(visualTransform.gameObject.IsDestroyed()))
            {
                Destroy(visualTransform.gameObject);
            }

            building = buildingSO.building;
            visualTransform = SpawnForCheck(GetMouseWorldPosition(), building);
        }

    }
    public Transform SpawnForCheck(Vector3 position, Building visualBuilding)
    {
        Transform visualTransform = Instantiate(visualBuilding.buildingSO.buildingPrefab, position, Quaternion.identity);
        return visualTransform;
    }
    public void CheckAndSpawn(Transform visualTransform)
    {
        GridManager.Instance.GetValue(visualTransform.position).GetIndices(out int I, out int J);
        bool[,] Visited = new bool[GridManager.Instance.GetWidth(), GridManager.Instance.GetHeight()];
        GameObject visualObject = visualTransform.gameObject;
        Check(I, J, Visited, visualObject.GetComponent<Building>(), out bool safe);
        //Debug.Log("area is" + safe);
        Material material = visualObject.GetComponent<Renderer>().material;

        if (!(Player.Instance.currentBuildingCount[building.buildingSO.buildingType] < Player.Instance.gameRules.buildingCount[building.buildingSO.buildingType]))
        {
            safe = false;
        }
           if (building.buildingSO.buildingType == BuildingType.ResourceGenerator)
        {
            AutoMiner autoMiner = visualObject.GetComponent<AutoMiner>();
            if (autoMiner != null)
            {
                Debug.Log(autoMiner.CanBuild());
                safe = autoMiner.CanBuild();
            }
        }



            if (!safe)
        {
            Color redColor = new Color(1f, 0f, 0f, 0.7f);
            material.color = redColor;
        }
       
        Debug.Log("what");
        Debug.Log("type count is " + Player.Instance.gameRules.buildingCount[building.buildingSO.buildingType]);
        if (safe && Player.Instance.currentBuildingCount[building.buildingSO.buildingType] < Player.Instance.gameRules.buildingCount[building.buildingSO.buildingType])
        {
            Color whiteColor = new Color(1f, 1f, 1f, 0.7f);
            material.color = whiteColor;

            if (Input.GetMouseButton(0))
            {
                
                Spawn(visualTransform.position);
                
                    Destroy(visualTransform.gameObject);
                    Destroy(visualTransform);
                visualTransform = null;
                
                return;
            }
        }


    }
    public Entity Spawn(Vector3 position)
    {
        Building instantiatedBuilding = Instantiate(building.buildingSO.buildingPrefab, position, Quaternion.identity).GetComponent<Building>();
        BuildAfterCheck(instantiatedBuilding);
        ResourceManager.Instance.updateResource(ResourceType.GOLD, -building.buildingSO.price);
        setNeighborCells(position,instantiatedBuilding);
        //built?.Invoke(this);
        onBuilt(instantiatedBuilding);
        instantiatedBuilding.SetBuildingState(BuildingState.BUILT);
        instantiatedBuilding.UpdateChildVisibility();
        BuildingCells.Clear();
        building = null;
        return instantiatedBuilding;
    }
    public void AIUIHelper(BuildingSO buildingSO, Vector3 position)
    {

        building = buildingSO.building;
        visualTransform = SpawnForCheck(position,building);


    }

    //Mouse position tracking
    public Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = mainCamera.nearClipPlane;
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f;
        return mouseWorldPosition;
    }
    public void Check(int I, int J, bool[,] Visited, Building instantiatedBuilding, out bool safe)
    {
        safe = true;
        RecursiveCheck(I, J, Visited, instantiatedBuilding, out safe);
    }
    //recursively checking that the current building we want to build is safe through overlapping collider with map and other game objects
    // and adds all the surrounding cells into neighbor cells
    public void RecursiveCheck(int I, int J, bool[,] Visited, Building instantiatedBuilding, out bool safe)
    {
        safe = true;
        if (Visited[I, J])
        {
            return;
        }
        if (I < 0 || J < 0 || I >= GridManager.Instance.GetWidth() || J >= GridManager.Instance.GetHeight())
        {
            safe = false;
            return;
        }
        Visited[I, J] = true;
        // Function to check if cell is overlapping with building collider
        Collider2D[] colliders = GridManager.Instance.OverlapAll(new Indices(I, J));
        if (colliders.Length > 1)
        {
            safe = false; return;
        }
        else if (colliders.Length == 1)
        {
            colliders[0].TryGetComponent<Entity>(out Entity entity);
            if (entity != null && entity is Building building && building.buildingSO == instantiatedBuilding.buildingSO)
            {
                BuildingCells.Add(GridManager.Instance.GetValue(I, J));
                RecursiveCheck(I + 1, J, Visited, instantiatedBuilding, out safe);
                RecursiveCheck(I, J + 1, Visited, instantiatedBuilding, out safe);
                RecursiveCheck(I - 1, J, Visited, instantiatedBuilding, out safe);
                RecursiveCheck(I, J - 1, Visited, instantiatedBuilding, out safe);

            }
            else if (entity != null && entity is Building otherBuilding && otherBuilding.buildingSO != instantiatedBuilding.buildingSO)
            {
                safe = false; return;
            }
            else
            {
                if (entity)
                {
                    // Debug.Log("collision with entity"); Debug.Log(colliders[0]);
                }
                else
                {
                    // Debug.Log("collision with non entity"); Debug.Log(colliders[0]); 
                }

            }
        }
        else
        {

            // Cell neighborCell = GridManager.Instance.GetValue(I, J);
            // Debug.Log("neighbor cell is" + neighborCell);
            //  instantiatedBuilding.neighborCellList.Add(neighborCell);
            //return;
        }

    }
    public void NeighborRecursiveCheck(int I, int J, bool[,] Visited, Building instantiatedBuilding, out bool safe)
    {
        safe = true;

        // Check boundaries first
        if (I < 0 || J < 0 || I >= GridManager.Instance.GetWidth() || J >= GridManager.Instance.GetHeight())
        {
            safe = false;
            return;
        }

        // Check if already visited
        if (Visited[I, J])
        {
            return;
        }

        Visited[I, J] = true;

        // Check for colliders
        Collider2D[] colliders = GridManager.Instance.OverlapAll(new Indices(I, J));
        if (colliders.Length > 1)
        {
            safe = false;
            return;
        }

        if (colliders.Length == 1)
        {
            colliders[0].TryGetComponent<Entity>(out Entity entity);
            if (entity != null && entity is Building building && building.buildingSO == instantiatedBuilding.buildingSO)
            {
                BuildingCells.Add(GridManager.Instance.GetValue(I, J));
                // Recursively check neighbors
                NeighborRecursiveCheck(I + 1, J, Visited, instantiatedBuilding, out safe);
                NeighborRecursiveCheck(I, J + 1, Visited, instantiatedBuilding, out safe);
                NeighborRecursiveCheck(I - 1, J, Visited, instantiatedBuilding, out safe);
                NeighborRecursiveCheck(I, J - 1, Visited, instantiatedBuilding, out safe);
            }
            else if (entity != null && entity is Building otherBuilding && otherBuilding.buildingSO != instantiatedBuilding.buildingSO)
            {
                safe = false;
                return;
            }
        }
        else if (colliders.Length == 0)
        {
            Cell neighborCell = GridManager.Instance.GetValue(I, J);
            Debug.Log(neighborCell.ToSafeString());
            instantiatedBuilding.neighborCellList.Add(neighborCell);
        }
    }

    //setting cells to house the entity
    public void BuildAfterCheck(Building instantiatedBuilding)
    {
        BuildingCells.Clear();
        GridManager.Instance.GetValue(instantiatedBuilding.transform.position).GetIndices(out int I, out int J);
        bool[,] Visited = new bool[GridManager.Instance.GetWidth(), GridManager.Instance.GetHeight()];
        Check(I, J, Visited, instantiatedBuilding, out bool safe);
        NeighborRecursiveCheck(I, J, Visited, instantiatedBuilding, out bool safe1);
        for (int i = 0; i < BuildingCells.Count; i++)
        {
            BuildingCells[i].SetEntity(instantiatedBuilding);
        }
        instantiatedBuilding.builtCellList = BuildingCells;
        BuildingCells.Clear();
    }


    private void setNeighborCells(Vector3 position,Building instantiatedBuilding)
    {
        GridManager.Instance.GetValue(position).GetIndices(out int I, out int J);
        bool[,] Visited = new bool[GridManager.Instance.GetWidth(), GridManager.Instance.GetHeight()];
        BuildingManager.Instance.NeighborRecursiveCheck(I, J, Visited, instantiatedBuilding, out bool safe);

    }
}