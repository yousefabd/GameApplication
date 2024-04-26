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
        initialCharacterPositions = new List<Vector3> { new Vector3(0.5f, 0.5f, 0) };
        CreateGridMap();
        CreatePlayerBase();
    }

    private void CreateGridMap()
    {
        gridMap = new Grid<Cell>(gridWidth, gridHeight, cellSize, gridOriginPosition, (int x, int y) => new Cell(x, y));
    }
    private void CreatePlayerBase()
    {
        for (int i = 0; i < initialCharacterPositions.Count; i++)
        {
            gridMap.SetValue(
                initialCharacterPositions[i],
                (int x, int y) =>
                {
                    Cell cell = new Cell(x, y);
                    cell.SetEntity(Entity.CHARACTER);
                    Vector3 worldPosition = gridMap.GetWorldPositionCentered(x, y);
                    cell.SpawnCharacter(characterSO, worldPosition);
                    return new Cell(x, y);
                }
            );
        }
    }
}