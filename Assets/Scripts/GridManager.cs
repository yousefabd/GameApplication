using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private Vector3 gridOriginPosition = new Vector3(-16f, -9f);
    private const int gridWidth = 32;
    private const int gridHeight = 18;
    private const float cellSize = 1f;
    private List<Vector3> initialUnitPosition;
    [SerializeField] private List<UnitSO> testUnitSOList;

    private Grid<Cell> gridMap;

    public static GridManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        //giving a static list of initial characters position temporarily in the future we should get that list from the Map
        initialUnitPosition = new List<Vector3> { new Vector3(0.5f, 0.5f, 0), new Vector3(1.5f, 0.5f, 0), new Vector3(-5.5f, 2.5f, 0f), new Vector3(-4.5f, 2.5f, 0f), new Vector3(-3.5f, 2.5f, 0f), new Vector3(5.5f, 2.5f, 0f), new Vector3(-5.5f, 5.5f, 0f) };
    }
    private void Start()
    {
        CreateGridMap();
        CreatePlayerBase();
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
    public void CreatePlayerBase()
    {
        for (int i = 0; i < initialUnitPosition.Count; i++)
        {
            Cell unitCell = gridMap.GetValue(initialUnitPosition[i]);
            Indices indices = unitCell.GetIndices();
            Unit unit;
            if (i < 1)
                unit = unitCell.SpawnUnit(testUnitSOList[0], GridToWorldPositionCentered(indices));
            else unit = unitCell.SpawnUnit(testUnitSOList[1], GridToWorldPositionCentered(indices));
            unitCell.SetEntity(unit);
            gridMap.UpdateValues();
        }
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
    public void MoveEntity(Indices oldPosition, Indices newPosition, Entity entity)
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
}