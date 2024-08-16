using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class Building : Entity, IDestructibleObject 
{   
    public  BuildingSO buildingSO;
    private BuildingState buildingState;

    public List<Cell> neighborCellList;
    public List<Cell> builtCellList;


   public void SetBuildingState(BuildingState newBuildingState) { buildingState = newBuildingState; }    
   public BuildingState GetBuildingState() { return buildingState; }

    private void Awake()
    {
        neighborCellList = new List<Cell>();
        builtCellList = new List<Cell>();

        buildingState = BuildingState.GHOST;
    }

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
    

 
    protected virtual void Start()
    {
        UpdateChildVisibility();

        
        if (buildingState == BuildingState.BUILT)
        {
            HealthPoints = buildingSO.health;
            HealthPoints = 0;
        }
    }
    public void UpdateChildVisibility()
    {
        Transform childTransform = transform.GetChild(0);   

        if (buildingState == BuildingState.GHOST)
        {
            childTransform.gameObject.SetActive(false);
        }
        else
        {
            childTransform.gameObject.SetActive(true);
        }
    }
    private void BuildProcess()
    {
       
        Renderer renderer = transform.GetComponent<Renderer>();
        Material material = renderer.material;
        float opacity = Mathf.Lerp(0.1f, 1f, (float)processCompletion / 100f);
        Color color = material.color;
        color.a = opacity;
        material.color = color;

        HealthPoints = Mathf.Lerp(0f, buildingSO.health, (float)processCompletion / 100f);
    }

    private void Update()
    {
        if (buildingState == BuildingState.BUILT)
        {
            processCompletion += 1;
            BuildProcess();
        }
       // Healing();
    }

  

    
    //function for spawning units around the building is neighbor cells
    public void Spawner(UnitSO unitSO)
    {
        Debug.Log(neighborCellList.Count);
       if(neighborCellList.Count == 0)
        {
            bool[,] visited = new bool[GridManager.Instance.GetWidth(), GridManager.Instance.GetHeight()];
            bool safe;
            GridManager.Instance.WorldToGridPosition(transform.position,out int i, out int j);
            BuildingManager.Instance.NeighborRecursiveCheck(i, j, visited, this, out safe);
        }
        Debug.Log(neighborCellList.Count);
        if (unitSO != null && neighborCellList.Count > 0)
        {
            int randomIndex = _random.Next(0, neighborCellList.Count);
            Cell cell = neighborCellList[randomIndex];
            Unit.Spawn(unitSO,GridManager.Instance.GridToWorldPositionCentered(cell.GetIndices()));
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
        if (buildingSO.buildingType == BuildingType.UnitSpawner)
        {
            UIManager.Instance.SwitchContent(true);
            UIUnitDisplay.Instance.createButtons(buildingSO.unitGenerationData, this);
        }
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
