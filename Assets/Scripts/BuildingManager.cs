using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }
    private Camera mainCamera;
    [SerializeField] private List<BuildingSO> buildingSOList;
    private Building building;
    Cell[,] gridArray;
    private List<Cell> BuildingCells =  new List<Cell>();

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            BuildingSO buildingSO = buildingSOList[0];
            building = buildingSO.building;
            Debug.Log(buildingSO.name);
            Debug.Log(building.ToString());
            building.Spawn(GetMouseWorldPosition());
        }
           
        
    }
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = mainCamera.nearClipPlane; // Ensure the Z position is set correctly
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f; // Set Z to 0 to align with 2D plane
        return mouseWorldPosition;
    }

    /*public void Build(Cell cell, int width, int height, Building building)
    {
        Debug.Log(building);
        Debug.Log(cell);
        int I, J;
        cell.GetIndices(out I, out J);
        Debug.Log(I + J);
        for (int i = I; i <= (I + width); i++)
        {
            for (int j = J; j <= (J + height); j++)
            {
                Cell wantedCell = GridManager.Instance.GetValue(i, j);
                wantedCell.SetEntity(building);
            }
        }
        return;
    }*/
    /*public bool isOccupied(Cell cell, int width, int height)                       
    {
        int I, J;
        cell.GetIndices(out I, out J);

        for (int i = I; i <= (I + width); i++)
        {
            for (int j = J; j <= (J + height); j++)
            {

                Cell wantedCell = GridManager.Instance.GetValue(i, j);
                if (wantedCell.IsOccupied())
                {
                    return false;
                }

            }
        }
        return true;
    }*/

    void BuildAfterCheck(Building instantiatedBuilding)
    {
        for(int i =0;i <BuildingCells.Count;i++)
        {
            BuildingCells[i].SetEntity(instantiatedBuilding);
        }
        BuildingCells.Clear();
    }
    public void RecursiveCheck(int I, int J, bool[,] Visited, Building instantiatedBuilding, out bool safe)
    {
        Debug.Log("inside recursion");
        safe = true;
        if (I < 0 || J < 0 || I >= GridManager.Instance.GetWidth() || J >= GridManager.Instance.GetHeight() || Visited[I, J])
        {
            // Indices are out of bounds or cell has already been visited, return without doing anything
            return;
        }

        Visited[I, J] = true;
        Debug.Log("inside recursion 2");
        // Function to check if cell is overlapping with building collider
        Collider2D collider = GridManager.Instance.Overlap(new Indices(I, J));
        Debug.Log("collider is " + collider);   
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
                RecursiveCheck(I, J + 1, Visited, instantiatedBuilding, out safe);
                RecursiveCheck(I - 1, J, Visited, instantiatedBuilding, out safe);
                RecursiveCheck(I, J - 1, Visited, instantiatedBuilding, out safe);
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

}
