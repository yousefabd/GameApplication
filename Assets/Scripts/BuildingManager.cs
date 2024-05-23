using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }
    private Camera mainCamera;
    [SerializeField] private List<BuildingSO> buildingSOList;
    private Building building;
    Cell[,] gridArray;

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
        }
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("mouse");
                Cell cell = GridManager.Instance.GetValue(GetMouseWorldPosition());
                //Here we got
            bool built;

            //building.Spawn(cell,out built);               
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

    public void Build(Cell cell, int width, int height, Building building)
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
    }
    public bool isOccupied(Cell cell, int width, int height)                       
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
    }

    void RecursiveCheck(int I, int J, List<Indices> Visited)
    {
        if (Visited.Contains(new Indices(I, J)))
        {
            return;
        }
        else
        {
            Visited.Add(new Indices(I, J));
            //function to check if cell is overlapping with building collider
            RecursiveCheck(I + 1, J, Visited);
            RecursiveCheck(I, J + 1, Visited);
            RecursiveCheck(I + 1, J + 1, Visited);
        }

    }

}
