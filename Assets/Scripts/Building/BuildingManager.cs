using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    //singleton pattern
    public static BuildingManager Instance { get; private set; }


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
        instantiatedBuilding.buildingSO.NeighborCells.Clear();
        RecursiveCheck(I,J,Visited,instantiatedBuilding, out safe);
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
        if (I < 0 || J < 0 || I >= GridManager.Instance.GetWidth() || J >= GridManager.Instance.GetHeight() )
        {
            safe = false;
            return;
        }
        Visited[I, J] = true;
        // Function to check if cell is overlapping with building collider
        Collider2D[] colliders = GridManager.Instance.OverlapAll(new Indices(I, J));
        if(colliders.Length > 1)
        {
            safe=false; return;
        }else if(colliders.Length == 1) {
                colliders[0].TryGetComponent<Building>(out Building entity);
        if(entity != null && entity.buildingSO == instantiatedBuilding.buildingSO)
            {
                BuildingCells.Add(GridManager.Instance.GetValue(I, J));
                RecursiveCheck(I + 1, J, Visited, instantiatedBuilding, out safe);
                RecursiveCheck(I, J + 1, Visited, instantiatedBuilding, out safe);
                RecursiveCheck(I - 1, J, Visited, instantiatedBuilding, out safe);
                RecursiveCheck(I, J - 1, Visited, instantiatedBuilding, out safe);

            }
            else if(entity != null && entity.buildingSO != instantiatedBuilding.buildingSO)
            {
                safe = false; return;
            }
            else { Debug.Log("collision with non entity"); Debug.Log(colliders[0]); }
        }
        else
        {
            Cell neighborCell = GridManager.Instance.GetValue(I, J);
            instantiatedBuilding.buildingSO.NeighborCells.Add(neighborCell);
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
