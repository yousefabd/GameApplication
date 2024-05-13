using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }
    private Camera mainCamera;
    [SerializeField] private List<BuildingSO> buildingSOList;
    Cell[,] gridArray = Grid<Cell>.GetGridArray();
    private Building building;

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
                Cell cell = GameManager.Instance.gridMap.GetValue(GetMouseWorldPosition());
            bool built;

            building.Spawn(cell,out built);               
            }
        
    }
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 MouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        MouseWorldPosition.z = 0f;
        return MouseWorldPosition;
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
                Debug.Log(" am in the loop");
                if (gridArray[i, j] is Cell gridCell)
                {
                    gridCell.SetEntity(building);
                }
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


                if (gridArray[i, j] is Cell gridCell)
                {
                    Debug.Log(gridCell);
                    if (gridCell.isOccupied())
                    {
                        return false;
                    }
                }

            }
        }
        return true;
    }

    

}
