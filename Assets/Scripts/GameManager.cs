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

    [SerializeField] private BuildingSO goblinsMainBuilding;

    private Indices goblinsMainBuildingIndices = new Indices();
    private void Start()
    {
        Instance = this;

        goblinsMainBuildingIndices.I = 130;
        goblinsMainBuildingIndices.J = 130;

  
        BuildingManager.Instance.placeBuilding(goblinsMainBuilding, goblinsMainBuildingIndices, 100, 160, 100, 160);
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


}
