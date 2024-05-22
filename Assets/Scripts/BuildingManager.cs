using System.Collections;
using System.Collections.Generic;
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
        Cell[,] gridArray = GameManager.Instance.gridMap.GetGridArray();

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
                Cell wantedCell = GameManager.Instance.gridMap.GetValue(i, j);
                wantedCell.SetEntity(building);
            }
        }
        GameManager.Instance.gridMap.UpdateValues();    
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

                Cell wantedCell = GameManager.Instance.gridMap.GetValue(i, j);
                if (wantedCell.isOccupied())
                {
                    return false;
                }

            }
        }
        return true;
    }

    

}
