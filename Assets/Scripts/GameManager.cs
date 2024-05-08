using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Vector3 gridOriginPosition = new Vector3(-16f, -9f);
    private const int gridWidth = 32;
    private const int gridHeight = 18;
    private const float cellSize = 1f;
    private List<Vector3> initialCharacterPositions;
    [SerializeField] private CharacterSO characterSO;

    public Grid<Cell> gridMap;

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        //giving a static list of initial characters position temporarily in the future we should get that list from the Map
        initialCharacterPositions = new List<Vector3> { new Vector3(0.5f, 0.5f, 0) , new Vector3(-5.5f,2.5f,0f), new Vector3(-4.5f, 2.5f, 0f), new Vector3(-3.5f, 2.5f, 0f), new Vector3(5.5f, 2.5f, 0f), new Vector3(-5.5f, 5.5f, 0f) };
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
        for (int i = 0; i < initialCharacterPositions.Count; i++)
        {
            Cell characterCell = gridMap.GetValue(initialCharacterPositions[i]);
            Indices indices = characterCell.GetIndices();
            Character character=characterCell.SpawnCharacter(characterSO, GridToWorldPositionCentered(indices));
            characterCell.SetEntity(character);
            gridMap.UpdateValues();
        }
    }
    public void WorldToGridPosition(Vector3 worldPosition,out int i,out int j)
    {
        gridMap.GetIndices(worldPosition, out i, out j);
    }
    public Vector3 GridToWorldPosition(Indices indices)
    {
        return gridMap.GetWorldPosition(indices.I, indices.J);
    }
    public Vector3 GridToWorldPositionCentered(Indices indices)
    {
        return gridMap.GetWorldPositionCentered(indices.I, indices.J);
    }
    public int GetWidth()
    {
        return gridWidth;
    }
    public int GetHeight()
    {
        return gridHeight;
    }
    public Entity GetEntity(Vector3 worldPosition)
    {
        return gridMap.GetValue(worldPosition).GetEntity();
    }
    public Entity GetEntity(int i,int j)
    {
        return gridMap.GetValue(i,j).GetEntity();
    }
    public void SetEntity(Entity entity,Vector3 worldPosition)
    {
        gridMap.GetValue(worldPosition).SetEntity(entity);
    }
    public void SetEntity(Entity entity,Indices indices)
    {
        gridMap.GetValue(indices.I,indices.J).SetEntity(entity);
    }
}