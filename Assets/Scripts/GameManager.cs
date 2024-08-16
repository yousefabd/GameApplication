using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public enum STRATEGIES { NONE,RESOURCE_GATHERING, UNIT_PRODUCTION, COMBAT, DEFENSE }

public class GameManager : MonoBehaviour
{
    //singleton pattern
    public static GameManager Instance { get; private set; }

    [SerializeField] private BuildingSO humansMainBuilding;
    [SerializeField] private BuildingSO goblinsMainBuilding;

    private Indices humansMainBuildingIndices = new Indices();
    private Indices goblinsMainBuildingIndices = new Indices();
    private void Start()
    {
        Instance = this;

        humansMainBuildingIndices.I = 15;
        humansMainBuildingIndices.J = 15;
        goblinsMainBuildingIndices.I = 65;
        goblinsMainBuildingIndices.J = 65;

        BuildingManager.Instance.placeBuilding(humansMainBuilding, humansMainBuildingIndices, 0, 30, 0, 30);
        BuildingManager.Instance.placeBuilding(goblinsMainBuilding, goblinsMainBuildingIndices, 50, 80, 50, 80);
    }
    private void Update()
    {
        CheckForWinner();
    }
    private void CheckForWinner() { }

    public GameObject GetGoblinsMainBuildingAsGameObject() 
    {
        return GameObject.FindGameObjectWithTag("Goblin Base");
    }

    public BuildingSO GetGoblinsMainBuilding()
    {
        GetGoblinsMainBuildingAsGameObject().TryGetComponent<Building>(out Building building);

        return building.buildingSO; 
    }

    public BuildingSO GetHumansMainBuilding()
    {
        return humansMainBuilding;
    }
}
