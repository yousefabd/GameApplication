using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class Building : Entity, IDestructibleObject 
{   
    public BuildingSO buildingSO;
    private BuildingState buildingState;

   public void SetBuildingState(BuildingState newBuildingState) { buildingState = newBuildingState; }    
   public BuildingState GetBuildingState() { return buildingState; }

    private System.Random _random = new System.Random();

    public event Action<float> OnDamaged;
   // public static event Action<Building> built;
    public event Action OnDestroyed;
    
    public float HealthPoints { get; set; }

    
    private int processCompletion = 0;

    public void SetProcessCompletion(int newProcessCompletion)
    {
        processCompletion += newProcessCompletion; 
    }
    

    public List<Cell> neighborCellList;
    public List<Cell> builtCellList;

    protected virtual void Start()
    {
        neighborCellList = new List<Cell>();    
        builtCellList = new List<Cell>();

        buildingState = BuildingState.GHOST;

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


    public enum BuildingState
    {
        GHOST,BUILT
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
