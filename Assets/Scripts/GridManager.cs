using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    //private Vector3 gridOriginPosition = new Vector3(-16f, -9f);
    //private const int gridWidth = 32;
    //private const int gridHeight = 18;
    private Vector3 gridOriginPosition = new Vector3(-99f,-99f);
    private const int gridWidth = 160;
    private const int gridHeight = 160;
    private const float cellSize = 1f;
    private List<Vector3> initialUnitPosition;
    private List<Vector3> initialUnitPosition1;
    [SerializeField] private List<UnitSO> testUnitSOList;

    private Grid<Cell> gridMap;

    public static GridManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        //giving a static list of initial characters position temporarily in the future we should get that list from the Map
        CreateGridMap();
        initialUnitPosition = new List<Vector3> 
        { new Vector3(0.5f, 0.5f, 0), new Vector3(1.5f, 0.5f, 0), new Vector3(-5.5f, 2.5f, 0f), 
            new Vector3(-4.5f, 2.5f, 0f), new Vector3(-3.5f, 2.5f, 0f), new Vector3(5.5f, 2.5f, 0f), new Vector3(-5.5f, 5.5f, 0f) };
        initialUnitPosition1 = new List<Vector3>
        { new Vector3(1.5f, 1.5f, 0), new Vector3(2.5f, 1.5f, 0), new Vector3(-4.5f, 3.5f, 0f),
            new Vector3(-3.5f, 3.5f, 0f), new Vector3(-3.5f, 5.5f, 0f), new Vector3(5.5f, 6.5f, 0f), new Vector3(-6.5f, 8.5f, 0f) };
    }
    public Grid<Cell> GetgridMap() { return gridMap; }
    private void Start()
    {
     //InvokeRepeating("CreatePlayerBase", 1, 20);
     //InvokeRepeating("CreatePlayerBase1", 5, 3);
     //InvokeRepeating("CreatePlayerBase2", 8, 10);
     //InvokeRepeating("CreatePlayerBase3", 10, 15);
        //InvokeRepeating("GG", 2, 2);
    }


    public void CreateGridMap()
    {
        gridMap = new Grid<Cell>(gridWidth, gridHeight, cellSize, gridOriginPosition, (int x, int y) =>
        {
            Cell cell = new Cell(x, y);
            cell.SetEntity(null);
            return cell;
        });
    }
    /*public void GG() 
    {
        GameObject[] k = GameObject.FindGameObjectsWithTag("Goblin");
        foreach (GameObject g in k) 
        {
            if (g.TryGetComponent<CharacterOpponentAI>(out CharacterOpponentAI c)) 
            {
                if (c == null) { g.AddComponent<CharacterOpponentAI>(); }
            }
        }
            }*/
   public void CreatePlayerBase()
    {

        GameObject goblinBase = GameObject.FindGameObjectWithTag("Goblin Base");
        Vector3 position = new Vector3(goblinBase.transform.position.x - 2, goblinBase.transform.position.y - 2);
        Cell unitCell = gridMap.GetValue(position);
        Indices indices = unitCell.GetIndices();
            Unit unit;
            unit = unitCell.SpawnUnit(testUnitSOList[0], GridToWorldPositionCentered(indices));
            unitCell.SetEntity(unit);
        unit.gameObject.AddComponent<CharacterOpponentAI>();
        gridMap.UpdateValues();
 
    }
    public void CreatePlayerBase1()
    {
        GameObject goblinBase = GameObject.FindGameObjectWithTag("Goblin Base");
        Vector3 position = new Vector3(goblinBase.transform.position.x - 2, goblinBase.transform.position.y - 2);
        Cell unitCell = gridMap.GetValue(position);
        Indices indices = unitCell.GetIndices();
            Unit unit;
            unit = unitCell.SpawnUnit(testUnitSOList[1], GridToWorldPositionCentered(indices));
            unitCell.SetEntity(unit);
            unit.gameObject.AddComponent<CharacterOpponentAI>();
            gridMap.UpdateValues();


    }
    public void CreatePlayerBase2()
    {

        GameObject goblinBase = GameObject.FindGameObjectWithTag("Goblin Base");
        Vector3 position = new Vector3(goblinBase.transform.position.x - 2, goblinBase.transform.position.y - 2);
        Cell unitCell = gridMap.GetValue(position);
        Indices indices = unitCell.GetIndices();
        Unit unit;
        unit = unitCell.SpawnUnit(testUnitSOList[2], GridToWorldPositionCentered(indices));
        unitCell.SetEntity(unit);
        unit.gameObject.AddComponent<CharacterOpponentAI>();
        gridMap.UpdateValues();

    }


    public void UpdateValues()
    {
        gridMap.UpdateValues();
    }
    public Cell GetValue(int I, int J)
    {
        return gridMap.GetValue(I, J);
    }
    public Cell GetValue(Vector3 worldPosition)
    {
        return gridMap.GetValue(worldPosition);
    }
    public void WorldToGridPosition(Vector3 worldPosition, out int i, out int j)
    {
        gridMap.GetIndices(worldPosition, out i, out j);
    }
    public Vector3 GridToWorldPosition(Indices indices)
    {
        return gridMap.GetWorldPosition(indices.I, indices.J);
    }
    public Vector3 GridToWorldPositionFixed(Indices indices)
    {
        return gridMap.GetWorldPositionCentered(indices.I, indices.J);
    }
    public Vector3 GridToWorldPositionCentered(Indices indices)
    {
        return gridMap.GetWorldPositionCentered(indices.I, indices.J);
    }
    public Vector3 GetWorldPositionFixed(Vector3 worldPosition)
    {
        Indices indices;
        WorldToGridPosition(worldPosition, out indices.I, out indices.J);
        return GridToWorldPositionCentered(indices);
    }
    public Vector3 GetWorldPosition(Vector3 worldPosition)
    {
        Indices indices;
        WorldToGridPosition(worldPosition, out indices.I, out indices.J);
        return GridToWorldPosition(indices);
    }
    public int GetWidth()
    {
        return gridWidth;
    }
    public int GetHeight()
    {
        return gridHeight;
    }
    public float GetCellSize()
    {
        return cellSize;
    }
    public Entity GetEntity(Vector3 worldPosition)
    {
        return gridMap.GetValue(worldPosition)?.GetEntity();
    }
    public Entity GetEntity(int i, int j)
    {
        return gridMap.GetValue(i, j).GetEntity();
    }
    public void SetEntity(Entity entity, Vector3 worldPosition)
    {
        gridMap.GetValue(worldPosition).SetEntity(entity);
        gridMap.UpdateValues();
    }
    public void SetEntity(Entity entity, Indices indices)
    {
        gridMap.GetValue(indices.I, indices.J).SetEntity(entity);   
        gridMap.UpdateValues();
    }


   
    public void MoveEntity(Indices oldPosition,Indices newPosition,Entity entity)
    {
        gridMap.GetValue(oldPosition.I, oldPosition.J).ClearEntity();
        gridMap.GetValue(newPosition.I, newPosition.J).SetEntity(entity);
        gridMap.UpdateValues();
    }
    public void MoveEntity(Vector3 oldPosition, Vector3 newPosition, Entity entity)
    {
        gridMap.GetValue(oldPosition).ClearEntity();
        gridMap.GetValue(newPosition).SetEntity(entity);
        gridMap.UpdateValues();
    }
    public bool IsOccupied(Vector3 worldPosition)
    {
        return gridMap.GetValue(worldPosition).IsOccupied();
    }
    public bool IsOccupied(int I, int J)
    {
        return gridMap.GetValue(I, J).IsOccupied();
    }
    public Collider2D Overlap(Vector3 cellWorldPosition)
    {
        Vector3 startPosition = GetWorldPosition(cellWorldPosition);
        Vector3 endPosition = new Vector3(startPosition.x + GetCellSize(), startPosition.y + GetCellSize(), 0);
        return Physics2D.OverlapArea(startPosition, endPosition);
    }
    public Collider2D Overlap(Indices cellIndices)
    {
        Vector3 startPosition = GridToWorldPosition(cellIndices);
        Vector3 endPosition = new Vector3(startPosition.x + GetCellSize(), startPosition.y + GetCellSize(), 0);
        return Physics2D.OverlapArea(startPosition, endPosition);
    }

    public Collider2D[] OverlapAll(Indices cellIndices)
{
    Vector3 startPosition = GridToWorldPosition(cellIndices);
    Vector3 endPosition = new Vector3(startPosition.x + GetCellSize(), startPosition.y + GetCellSize(), 0);

    // Debugging lines to visualize the overlap area
    Debug.DrawLine(startPosition, new Vector3(startPosition.x, endPosition.y, 0), Color.red, 1.0f);
    Debug.DrawLine(startPosition, new Vector3(endPosition.x, startPosition.y, 0), Color.red, 1.0f);
    Debug.DrawLine(endPosition, new Vector3(startPosition.x, endPosition.y, 0), Color.red, 1.0f);
    Debug.DrawLine(endPosition, new Vector3(endPosition.x, startPosition.y, 0), Color.red, 1.0f);

    Collider2D[] colliders = Physics2D.OverlapAreaAll(startPosition, endPosition);


    return colliders; 
}

}