using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    //singleton pattern
    public static BuildingManager Instance { get; private set; }

    [SerializeField] private List<BuildingSO> buildingSOList;

    private Camera mainCamera;
    private Building building;
    private List<Cell> BuildingCells = new List<Cell>();
    private Transform visualTransform;

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
        if (building != null) {
            if (visualTransform != null)
            {
                visualTransform.position = GetMouseWorldPosition();
                building.CheckAndSpawn(visualTransform);
                //building = null;
            }

        }
    }

    // Functionality for UI
    public void UIHelper(BuildingSO buildingSO)
    {

        building = buildingSO.building;
        visualTransform = building.SpawnForCheck(GetMouseWorldPosition());
       
       
    }
    
    //Mouse position tracking
    private Vector3 GetMouseWorldPosition()
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
        instantiatedBuilding.buildingSO.NeighborCells.Clear();
        RecursiveCheck(I,J,Visited,instantiatedBuilding, out safe);
    }
    //recursively checking that the current building we want to build is safe through overlapping collider with map and other game objects
    // and adds all the surrounding cells into neighbor cells
    public void RecursiveCheck(int I, int J, bool[,] Visited, Building instantiatedBuilding, out bool safe)
    {
        safe = true;
        if (I < 0 || J < 0 || I >= GridManager.Instance.GetWidth() || J >= GridManager.Instance.GetHeight() || Visited[I, J])
        {
            // Indices are out of bounds or cell has already been visited, return without doing anything
            return;
        }

        Visited[I, J] = true;
        // Function to check if cell is overlapping with building collider
        Collider2D collider = GridManager.Instance.Overlap(new Indices(I, J));
        Debug.Log(collider);
        if (collider != null)
        {
            collider.TryGetComponent<Entity>(out Entity entity);
            if (entity != null && entity != instantiatedBuilding)
            {
                safe = false;
                return;
            }

            if (entity == instantiatedBuilding)
            {
                BuildingCells.Add(GridManager.Instance.GetValue(I, J));

                RecursiveCheck(I + 1, J, Visited, instantiatedBuilding, out safe);
                if (!safe) return; 
                RecursiveCheck(I, J + 1, Visited, instantiatedBuilding, out safe);
                if (!safe) return;
                RecursiveCheck(I - 1, J, Visited, instantiatedBuilding, out safe);
                if (!safe) return;
                RecursiveCheck(I, J - 1, Visited, instantiatedBuilding, out safe);
                if (!safe) return;
            }
        }


        if (collider == null)
        {
            Cell neighborCell = GridManager.Instance.GetValue(I, J);
            Debug.Log(neighborCell.ToString());
            instantiatedBuilding.buildingSO.NeighborCells.Add(neighborCell);
            Debug.Log("fixed");
            return;
        }
    }

    //setting cells to house the entity
    public void BuildAfterCheck(Building instantiatedBuilding)
    {
        for (int i = 0; i < BuildingCells.Count; i++)
        {
            BuildingCells[i].SetEntity(instantiatedBuilding);
        }
        BuildingCells.Clear();
    }

}
