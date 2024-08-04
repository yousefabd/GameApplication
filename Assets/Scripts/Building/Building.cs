using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class Building : Entity, IDestructibleObject 
{   
    public BuildingSO buildingSO;
    private System.Random _random = new System.Random();

    public event Action<float> OnDamaged;
    public static event Action<Building> built;
    public event Action OnDestroyed;
    
    public float HealthPoints { get; set; }

    
    private int processCompletion = 0;

    public void SetProcessCompletion(int newProcessCompletion)
    {
        processCompletion += newProcessCompletion; 
    }
    

    public List<Cell> neighborCellList;
    public List<Cell> builtCellList;

    private void Start()
    {
        neighborCellList = new List<Cell>();    
        builtCellList = new List<Cell>();   

        HealthPoints = buildingSO.health;
        
        HealthPoints = 0;

        if (buildingSO.resourceGenerator == true )
        {
            raiseCapacity(this);
        }
    }
    private void BuildProcess()
    {
       
        Renderer renderer = buildingSO.buildingPrefab.gameObject.GetComponent<Renderer>();
        Material material = renderer.material;
        float opacity = Mathf.Lerp(0.1f, 1f, (float)processCompletion / 100f);
        Color color = material.color;
        color.a = opacity;
        material.color = color;

        HealthPoints = Mathf.Lerp(0f, buildingSO.health, (float)processCompletion / 100f);
    }

    private void Update()
    {
        BuildProcess();
    }

    //raises the capacity of Gold Storage using Events (Called when a specific building is initizialized)
    private void raiseCapacity(Building building)
    {
        Player.goldStorage += building.buildingSO.goldStorage;
    }

    //Initialize object that floats with mouse
    public Transform SpawnForCheck(Vector3 position)
    {
        Transform visualTransform = Instantiate(buildingSO.buildingPrefab, position, Quaternion.identity);
        return visualTransform;
    }

    //checks that the current object form SpawnForCheck is Safe to Build
    public void CheckAndSpawn(Transform visualTransform)
    {
        GridManager.Instance.GetValue(visualTransform.position).GetIndices(out int I, out int J);
        bool[,] Visited = new bool[GridManager.Instance.GetWidth(), GridManager.Instance.GetHeight()];
        GameObject instantiatedObject = visualTransform.gameObject;
        BuildingManager.Instance.Check(I, J, Visited, this, out bool safe);
        //Debug.Log("area is" + safe);
        Material material = instantiatedObject.GetComponent<Renderer>().material;

        
        if (!safe)
        {
            material.SetColor("_Color", Color.red * 0.7f);
        }
        Debug.Log("what");
        Debug.Log("type count is " + Player.Instance.gameRules.buildingCount[buildingSO.buildingType]);
        if (safe && Player.Instance.currentBuildingCount[buildingSO.buildingType] < Player.Instance.gameRules.buildingCount[buildingSO.buildingType])
        {
            material.SetFloat("_Color.a", safe ? 0.5f : 0.2f);
            if (Input.GetMouseButton(0)) {
                Spawn(visualTransform.position);
                Destroy(visualTransform.gameObject);
                return;
            }
        }


    }

    //Initializes safe Object
    public override Entity Spawn(Vector3 position)
    {
        Instantiate(buildingSO.buildingPrefab, position, Quaternion.identity);
        BuildingManager.Instance.BuildAfterCheck(this);
        setNeighborCells(position);
        built?.Invoke(this);
        return this;
    }
    //function for spawning units around the building is neighbor cells
    public void Spawner(UnitSO unitSO)
    {
        if (unitSO != null && buildingSO.NeighborCells.Count > 0)
        {
            int randomIndex = _random.Next(0, buildingSO.NeighborCells.Count);
            Cell cell = neighborCellList[randomIndex];
            unitSO.unit.Spawn(GridManager.Instance.GridToWorldPositionCentered(cell.GetIndices()));
        }
    }
    
    //logic to damage building
    public void Damage(Vector3 position, float value)
    {
        if (GridManager.Instance.GetValue(position).GetEntity() == this)
        {
            OnDamaged?.Invoke(value);
            HealthPoints -= value;
            if (HealthPoints < 0) { 
                Destruct();
                OnDestroyed?.Invoke();

            }
        }
    }
    //logic to destroy building after damage
    public void Destruct()
    {
        setCellsNull();
        Destroy(buildingSO.buildingPrefab.gameObject);
    }

    private void setCellsNull()
    {
        for(int i = 0; i < builtCellList.Count; i++)
        {
            builtCellList[i].SetEntity(null);
        }
    }

    private void OnMouseDown()
    {
        //Debug.Log(buildingSO.unitGenerationData);
        //Debug.Log(this);
        UIUnitDisplay.Instance.createButtons(buildingSO.unitGenerationData, this);
    }
   
private void setNeighborCells(Vector3 position)
    {
        GridManager.Instance.GetValue(position).GetIndices(out int I, out int J);
        bool[,] Visited = new bool[GridManager.Instance.GetWidth(), GridManager.Instance.GetHeight()];
        BuildingManager.Instance.NeighborRecursiveCheck(I, J, Visited, this, out bool safe);

    }
private void Healing()
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, 3 * GridManager.Instance.GetCellSize());
        foreach(Collider2D collision in collisions)
        {
            if ((collision.gameObject.TryGetComponent<Unit>(out Unit unit)) || unit.GetTeam() == Player.Instance.GetTeam()) {

                if(!(unit.HealthPoints + 10 > unit.GetMaxHealth()))
                unit.HealthPoints += 10;

            }
                

            
            
            
        }
    }



}
public enum BuildingType
{
    None,
    ResourceGenerator,
    UnitSpawner,
    UnitEvolver,
    DefenseTower,
    BaseBuilding
}
