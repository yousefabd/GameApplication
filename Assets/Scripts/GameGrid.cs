using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameGrid : MonoBehaviour
{
    public Recourses resources;

    public GameObject tilemapPrefab; // Reference to the tilemap prefab to be set in the Inspector
    public Tilemap topMap;
    public Tilemap botMap;
    public RuleTile topTile;
    public Tile botTile;

    int width;
    int height;
    private GridManager gridManager;


    [Header("Decoration Settings")]
    public Tilemap decorationMap;
    public Tile lutosLeafTile;
    public Tile flowerTile;
    public float decorationChance = 0.0001f;

    [Header("Gold Settings")]
    public GameObject goldPrefab;
    public float goldChance = 0.1f;
    private List<Entity> goldInstances = new List<Entity>();

    [Header("Wood Settings")]
    public GameObject woodPrefab;
    public float woodChance = 0.1f;
    private List<Entity> woodInstances = new List<Entity>();

    [Header("Stone Settings")]
    public GameObject stonePrefab;
    public float stoneChance = 0.1f;
    private List<Entity> stoneInstances = new List<Entity>();

    [Header("Obstacle Settings")]
    public GameObject obstaclePrefab;

    private int[,] terrainMap;

    private void InitializeTerrainMap()
    {
        terrainMap = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int cellPosition = new Vector3Int(-x + width / 2, -y + height / 2, 0);
                TileBase tile = topMap.GetTile(cellPosition);

                // Assuming 0 represents one type of terrain, and 1 another
                if (tile == topTile)
                {
                    terrainMap[x, y] = 0; // Assign terrain type 0
                }
                else if (tile == botTile)
                {
                    terrainMap[x, y] = 1; // Assign terrain type 1
                }
                else
                {
                    terrainMap[x, y] = -1; // Unknown or empty tile
                }
            }
        }
    }
    private void Start()
    {
        InitializeTerrainMap();
        if (tilemapPrefab != null)
        {
            // Instantiate the tilemap prefab

            // Assuming the prefab contains the necessary Tilemaps with the exact names "TopMap" and "BotMap"
  

            // Process the grid with the assigned topMap and botMap
            ProcessGrid();

            // Optionally, destroy the instantiated grid if it's no longer needed
            // Destroy(instantiatedGrid);
        }
    }

    private void ProcessGrid()
    {
        // Traverse through all positions of the grid
        BoundsInt bounds = topMap.cellBounds;
        TileBase[] allTiles = topMap.GetTilesBlock(bounds);

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int localPosition = new Vector3Int(x, y, 0);
                TileBase tile = topMap.GetTile(localPosition);

                if (tile == topTile)
                {
                    HandleTopTile(localPosition);
                }
                else if (botMap.GetTile(localPosition) == botTile)
                {
                    HandleBotTile(localPosition);
                }
            }
        }
    }

    private void HandleTopTile(Vector3Int position)
    {
        // Call functions for topTile
        resources = new Recourses(4, 24, 16);
        distributeResources();
        distributeDecorations();
    }

    private void HandleBotTile(Vector3Int position)
    {
        // Call functions for botTile
        gridManager = FindObjectOfType<GridManager>();
        RemoveOverlappingObjects();
        PlaceGold();
        PlaceStone();
        PlaceWood();
    }

    public void distributeDecorations()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int position = new Vector3Int(-x + width / 2, -y + height / 2, 0);
                if (terrainMap[x, y] == 0 && UnityEngine.Random.value < decorationChance)
                {
                    decorationMap.SetTile(position, lutosLeafTile);
                }
                else if (terrainMap[x, y] == 1 && UnityEngine.Random.value < decorationChance)
                {
                    decorationMap.SetTile(position, flowerTile);
                }
            }
        }
    }





    public void distributeResources()
    {
        ClearResource(goldInstances);
        ClearResource(stoneInstances);
        ClearResource(woodInstances);

        int goldCount = resources.goldCount;
        int woodCount = resources.woodCount;
        int stoneCount = resources.stoneCount;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int cellPosition = new Vector3Int(-x + width / 2, -y + height / 2, 0);
                Vector3 worldPos = topMap.CellToWorld(cellPosition) + topMap.cellSize / 2;

                Vector3Int adjustedCellPosition = new Vector3Int(cellPosition.x, cellPosition.y, 0);
                Vector3 adjustedWorldPos = topMap.CellToWorld(adjustedCellPosition) + topMap.cellSize / 2;

                GameObject obstaclePrefab = topMap.GetInstantiatedObject(adjustedCellPosition);
                if (obstaclePrefab != null)
                {
                    if (obstaclePrefab.TryGetComponent(out Obstacle obstacle))
                    {
                        obstaclePrefab.transform.position = adjustedWorldPos;
                        GridManager.Instance.SetEntity(obstacle, new Indices(x, y));
                        //Debug.Log("Obstacle placed at: " + adjustedWorldPos);
                    }
                }
            }
        }

        HashSet<Vector3Int> occupiedPositions = new HashSet<Vector3Int>();

        int goldPerQuadrant = goldCount / 4;
        int woodPerQuadrant = woodCount / 4;
        int stonePerQuadrant = stoneCount / 4;

        // Distribute gold
        DistributeInQuadrant(goldPerQuadrant, goldPrefab, occupiedPositions, -width / 2, 0, -height / 2, 0);
        DistributeInQuadrant(goldPerQuadrant, goldPrefab, occupiedPositions, 0, width / 2, -height / 2, 0);
        DistributeInQuadrant(goldPerQuadrant, goldPrefab, occupiedPositions, -width / 2, 0, 0, height / 2);
        DistributeInQuadrant(goldPerQuadrant, goldPrefab, occupiedPositions, 0, width / 2, 0, height / 2);

        // Distribute wood
        DistributeInQuadrant(woodPerQuadrant, woodPrefab, occupiedPositions, -width / 2, 0, -height / 2, 0);
        DistributeInQuadrant(woodPerQuadrant, woodPrefab, occupiedPositions, 0, width / 2, -height / 2, 0);
        DistributeInQuadrant(woodPerQuadrant, woodPrefab, occupiedPositions, -width / 2, 0, 0, height / 2);
        DistributeInQuadrant(woodPerQuadrant, woodPrefab, occupiedPositions, 0, width / 2, 0, height / 2);

        // Distribute stone
        DistributeInQuadrant(stonePerQuadrant, stonePrefab, occupiedPositions, -width / 2, 0, -height / 2, 0);
        DistributeInQuadrant(stonePerQuadrant, stonePrefab, occupiedPositions, 0, width / 2, -height / 2, 0);
        DistributeInQuadrant(stonePerQuadrant, stonePrefab, occupiedPositions, -width / 2, 0, 0, height / 2);
        DistributeInQuadrant(stonePerQuadrant, stonePrefab, occupiedPositions, 0, width / 2, 0, height / 2);

    }


    private bool IsNearHole(Vector3Int position, int bufferZoneSize)
    {
        int cellX = position.x + width / 2;
        int cellY = position.y + height / 2;

        for (int dx = -bufferZoneSize; dx <= bufferZoneSize; dx++)
        {
            for (int dy = -bufferZoneSize; dy <= bufferZoneSize; dy++)
            {
                int checkX = cellX + dx;
                int checkY = cellY + dy;

                if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                {
                    if (terrainMap[checkX, checkY] == 0)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    //private bool IsPositionValidForResource(Vector3Int position)
    //{
    //    Vector3 worldPosition = topMap.CellToWorld(position) + topMap.cellSize / 2;

    //    if (Physics2D.OverlapCircle(worldPosition, 0.5f))
    //    {
    //        return false;
    //    }

    //    int bufferZoneSize = 7;
    //    if (IsNearHole(position, bufferZoneSize))
    //    {
    //        return false;
    //    }

    //    return true;
    //}
    private void DistributeInQuadrant(int count, GameObject prefab, HashSet<Vector3Int> occupiedPositions, int xMin, int xMax, int yMin, int yMax)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3Int position = GetRandomValidPosition(occupiedPositions, xMin, xMax, yMin, yMax);
            float size = GetRandomSize();
            if (position != Vector3Int.zero)
            {
                occupiedPositions.Add(position);
                distributeResource(position, size, prefab);
            }
        }
    }
    private Vector3Int GetRandomValidPosition(HashSet<Vector3Int> occupiedPositions, int xMin, int xMax, int yMin, int yMax)
    {
        for (int attempt = 0; attempt < 100; attempt++)
        {
            int x = UnityEngine.Random.Range(xMin + 7, xMax - 7);
            int y = UnityEngine.Random.Range(yMin + 7, yMax - 7);
            Vector3Int position = new Vector3Int(x, y, 0);

            if (!occupiedPositions.Contains(position) && IsPositionValidForResource(position))
            {
                return position;
            }
        }
        return Vector3Int.zero;
    }

    private float GetRandomSize()
    {
        float size;
        float rand = UnityEngine.Random.value;
        if (rand < 0.7f)
        {
            size = 1f; // Small
        }
        else if (rand < 0.9f)
        {
            size = 1.5f; // Medium
        }
        else
        {
            size = 2f; // Large
        }
        return size;
    }
    private void distributeResource(Vector3Int cellPosition, float size, GameObject prefab)
    {
        Vector3 position = topMap.CellToWorld(cellPosition) + topMap.cellSize / 2;
        GameObject resourceObject = Instantiate(prefab, position, Quaternion.identity);
        IRecourses resource = resourceObject.GetComponent<IRecourses>();
        resource.Initialize(cellPosition, size);

        if (resource is Stone && size == 2f)
        {
            PolygonCollider2D polygonCollider = resourceObject.GetComponent<PolygonCollider2D>();
            if (polygonCollider != null)
            {
                Vector2[] stonePoints = new Vector2[]
                {
                new Vector2(0.803833544f/3, 0.988152266f/3),
                new Vector2(0f / 3, 2/3f),
                new Vector2(-0.663296282f/3, 0.834273696f / 3),
                new Vector2(-2.4507699f / 3, -0.810295224f / 3),
                new Vector2(-2.4507699f / 3 , -0.810295224f / 3),
                new Vector2(0.0516949892f / 3, -2.07405996f / 3),
                new Vector2(2.00738406f / 3, -1.47305143f / 3),
                new Vector2(2.62270093f / 3 , -0.406489909f / 3)
                };
                polygonCollider.points = stonePoints;
            }
        }

        if (resource is Gold)
        {
            goldInstances.Add((Gold)resource);


        }
        else if (resource is Wood)
        {
            woodInstances.Add((Wood)resource);
        }
        else if (resource is Stone)
        {
            stoneInstances.Add((Stone)resource);
        }
    }





    private void ClearResource<T>(List<T> resourceList) where T : MonoBehaviour
    {
        foreach (var resource in resourceList)
        {
            Destroy(resource.gameObject);
        }
        resourceList.Clear();
    }

  
    private void RemoveOverlappingObjects()
    {
        float overlapRadius = 0.5f;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int cellPosition = new Vector3Int(-x + width / 2, -y + height / 2, 0);
                Vector3 worldPos = topMap.CellToWorld(cellPosition) + topMap.cellSize / 2;

                Collider2D[] colliders = Physics2D.OverlapCircleAll(worldPos, overlapRadius);
                if (colliders.Length > 1)
                {
                    foreach (var collider in colliders)
                    {
                        Destroy(collider.gameObject);
                    }
                }
            }
        }
    }
    private bool IsPositionValidForResource(Vector3Int position)
    {
        Vector3 worldPosition = topMap.CellToWorld(position) + topMap.cellSize / 2;

        // Check for overlaps using a small radius
        float overlapRadius = 0.5f;
        if (Physics2D.OverlapCircle(worldPosition, overlapRadius))
        {
            return false;
        }

        int bufferZoneSize = 6; // Adjust the buffer zone size as needed
        if (IsNearHole(position, bufferZoneSize))
        {
            return false;
        }

        return true;
    }
    private void PlaceGold()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int cellPosition = new Vector3Int(-x + width / 2, -y + height / 2, 0);
                Vector3 worldPos = topMap.CellToWorld(cellPosition) + topMap.cellSize / 2;

                Vector3Int adjustedCellPosition = new Vector3Int(cellPosition.x, cellPosition.y, 0);
                Vector3 adjustedWorldPos = topMap.CellToWorld(adjustedCellPosition) + topMap.cellSize / 2;

                Collider2D collider = gridManager.Overlap(adjustedWorldPos);
                if (collider != null)
                {
                    GameObject colliderGameObject = collider.gameObject;

                    if (colliderGameObject.TryGetComponent(out Gold gold))
                    {
                        try
                        {
                            Collider2D[] overlappingColliders = Physics2D.OverlapCircleAll(adjustedWorldPos, 0.5f);



                            gold.transform.position = adjustedWorldPos;
                            Vector3Int gridCellPosition = topMap.WorldToCell(adjustedWorldPos);
                            GridManager.Instance.SetEntity(gold, new Indices(gridCellPosition.x + width / 2 - 2, gridCellPosition.y + height / 2 - 2));
                            Debug.Log("Gold placed at: " + adjustedWorldPos);
                        }
                        catch (NullReferenceException ex)
                        {
                            continue;
                        }
                    }
                }
            }
        }
    }

    private void PlaceWood()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int cellPosition = new Vector3Int(-x + width / 2, -y + height / 2, 0);
                Vector3 worldPos = topMap.CellToWorld(cellPosition) + topMap.cellSize / 2;

                Vector3Int adjustedCellPosition = new Vector3Int(cellPosition.x, cellPosition.y, 0);
                Vector3 adjustedWorldPos = topMap.CellToWorld(adjustedCellPosition) + topMap.cellSize / 2;

                Collider2D collider = gridManager.Overlap(adjustedWorldPos);
                if (collider != null)
                {
                    GameObject colliderGameObject = collider.gameObject;

                    if (colliderGameObject.TryGetComponent(out Wood wood))
                    {
                        try
                        {
                            Collider2D[] overlappingColliders = Physics2D.OverlapCircleAll(adjustedWorldPos, 0.5f);



                            wood.transform.position = adjustedWorldPos;
                            Vector3Int gridCellPosition = topMap.WorldToCell(adjustedWorldPos);
                            GridManager.Instance.SetEntity(wood, new Indices(gridCellPosition.x + width / 2 - 2, gridCellPosition.y + height / 2 - 1));
                            Debug.Log("Wood placed at: " + adjustedWorldPos);
                        }
                        catch (NullReferenceException ex)
                        {
                            continue;
                        }
                    }
                }
            }
        }
    }

    private void PlaceStone()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int cellPosition = new Vector3Int(-x + width / 2, -y + height / 2, 0);
                Vector3 worldPos = topMap.CellToWorld(cellPosition) + topMap.cellSize / 2;

                Vector3Int adjustedCellPosition = new Vector3Int(cellPosition.x, cellPosition.y, 0);
                Vector3 adjustedWorldPos = topMap.CellToWorld(adjustedCellPosition) + topMap.cellSize / 2;

                Collider2D collider = gridManager.Overlap(adjustedWorldPos);
                if (collider != null)
                {
                    GameObject colliderGameObject = collider.gameObject;

                    if (colliderGameObject.TryGetComponent(out Stone stone))
                    {
                        try
                        {
                            Collider2D[] overlappingColliders = Physics2D.OverlapCircleAll(adjustedWorldPos, 0.5f);



                            stone.transform.position = adjustedWorldPos;
                            Vector3Int gridCellPosition = topMap.WorldToCell(adjustedWorldPos);
                            GridManager.Instance.SetEntity(stone, new Indices(gridCellPosition.x + width / 2 - 2, gridCellPosition.y + height / 2 - 2));
                            Debug.Log("Stone placed at: " + adjustedWorldPos);
                        }
                        catch (NullReferenceException ex)
                        {
                            continue;
                        }
                    }
                }
            }
        }
    }
}
