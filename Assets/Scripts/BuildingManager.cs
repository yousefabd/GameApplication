using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }
    private Camera mainCamera;
    [SerializeField] private List<BuildingSO> buildingSOList;
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
            //Cell cell = GridManager.Instance.gridMap.GetValue(GetMouseWorldPosition());
            bool built;

            //building.Spawn(cell,out built);               
        }
        
    }
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 MouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        MouseWorldPosition.z = 0f;
        return MouseWorldPosition;
    }
}
