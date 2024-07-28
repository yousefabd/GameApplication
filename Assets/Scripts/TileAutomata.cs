using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;




public class TileAutomata : MonoBehaviour
{
    public Recourses resources;

    [Header("Map Settings")]
    Obstacle test;

    [Range(0, 100)]
    public int iniChance;
    [Range(1, 8)]
    public int birthLimit;
    [Range(1, 8)]
    public int deathLimit;

    [Range(1, 10)]
    public int numR;
    private int count = 0;

    private int[,] terrainMap;
    public Vector3Int tmpSize;
    public Tilemap topMap;
    public Tilemap botMap;
    public RuleTile topTile;
    public Tile botTile;

    public Grid<Cell> grid;
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

    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        StartCoroutine(InitializeGrid());


    }
    private IEnumerator InitializeGrid()
    {
        while (gridManager == null || grid == null)
        {
            yield return null; 
        }

        DoSimulation();
    }

    private void DoSimulation()
    {
            doSim(numR);
            resources = new Recourses(10, 20, 30);
            distributeResources();
            distributeDecorations();
            

            PlaceGold();
            PlaceStone();
            PlaceWood();

    }

    void Update()
    {

        if (Input.GetKey(KeyCode.F))
        {
            doSim(numR);
            distributeResources();
            distributeDecorations();
        }

        if (Input.GetKey(KeyCode.H))
        {
            clearMap(true);
            decorationMap.ClearAllTiles();
        }

        if (Input.GetKey(KeyCode.J))
        {
            SaveAssetMap();
            count++;
        }
        //if (Input.GetKey(KeyCode.T))
        //{
        //    TestDamageFunction();
        //}
    }
    private void TestDamageFunction()
    {
        Vector3Int testPosition = new Vector3Int(0, 0, 0);
        float testSize = 2f;
        GameObject testResourceObject = Instantiate(stonePrefab, testPosition, Quaternion.identity);
        IRecourses testResource = testResourceObject.GetComponent<IRecourses>();
        testResource.Initialize(testPosition, testSize);

        IDestructibleObject destructible = testResource as IDestructibleObject;
        if (destructible != null)
        {
            Debug.Log("Initial Health Points: " + destructible.HealthPoints);
            destructible.Damage(testPosition, 5f);
            Debug.Log("Health Points after 5 damage: " + destructible.HealthPoints);
            destructible.Damage(testPosition, 5f);
            Debug.Log("Health Points after another 5 damage: " + destructible.HealthPoints);
        }
    }


    public void doSim(int nu)
    {
        clearMap(false);
        width = tmpSize.x;
        height = tmpSize.y;
        if (terrainMap == null)
        {
            terrainMap = new int[width, height];
            initPos();
        }
        for (int i = 0; i < nu; i++)
        {
            terrainMap = genTilePos(terrainMap);
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int position = new Vector3Int(-x + width / 2, -y + height / 2, 0);
                if (terrainMap[x, y] == 1)
                {
                    topMap.SetTile(position, topTile);
                }
                else
                {
                    botMap.SetTile(position, botTile);
                }
            }
        }
    }


    public void initPos()
    {
        for (int x = 0; x < width / 2; x++)
        {
            for (int y = 0; y < height / 2; y++)
            {
                int cell = UnityEngine.Random.Range(1, 101) < iniChance ? 1 : 0;
                terrainMap[x, y] = cell;
                terrainMap[width - x - 1, y] = cell;
                terrainMap[x, height - y - 1] = cell;
                terrainMap[width - x - 1, height - y - 1] = cell;
            }
        }
    }

    public int[,] genTilePos(int[,] oldMap)
    {
        int[,] newMap = new int[width, height];
        int neighb;
        BoundsInt myB = new BoundsInt(-1, -1, 0, 3, 3, 1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                neighb = 0;
                foreach (var b in myB.allPositionsWithin)
                {
                    if (b.x == 0 && b.y == 0) continue;
                    if (x + b.x >= 0 && x + b.x < width && y + b.y >= 0 && y + b.y < height)
                    {
                        neighb += oldMap[x + b.x, y + b.y];
                    }
                    else
                    {
                        neighb++;
                    }
                }

                if (oldMap[x, y] == 1)
                {
                    if (neighb < deathLimit) newMap[x, y] = 0;
                    else newMap[x, y] = 1;
                }

                if (oldMap[x, y] == 0)
                {
                    if (neighb > birthLimit) newMap[x, y] = 1;
                    else newMap[x, y] = 0;
                }
            }
        }
        return newMap;
    }

    public void clearMap(bool complete)
    {
        topMap.ClearAllTiles();
        botMap.ClearAllTiles();
        if (complete)
        {
            terrainMap = null;
        }
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
                else if (terrainMap[x, y] == 1 &&   UnityEngine.Random.value < decorationChance)
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

        for (int i = 0; i < goldCount; i++)
        {
            Vector3Int position = GetRandomValidPosition(occupiedPositions);
            float size = GetRandomSize();
            if (position != Vector3Int.zero)
            {
                occupiedPositions.Add(position);
                distributeResource(position, size, goldPrefab);
            }
        }

        for (int i = 0; i < woodCount; i++)
        {
            Vector3Int position = GetRandomValidPosition(occupiedPositions);
            float size = GetRandomSize();
            if (position != Vector3Int.zero)
            {
                occupiedPositions.Add(position);
                distributeResource(position, size, woodPrefab);
            }
        }

        for (int i = 0; i < stoneCount; i++)
        {
            Vector3Int position = GetRandomValidPosition(occupiedPositions);
            float size = GetRandomSize();
            if (position != Vector3Int.zero)
            {
                occupiedPositions.Add(position);
                distributeResource(position, size, stonePrefab);
            }
        }
    }
    //private Vector3Int GetRandomValidPosition(HashSet<Vector3Int> occupiedPositions)
    //{
    //    for (int attempt = 0; attempt < 100; attempt++)
    //    {
    //        int x = Random.Range(-34, 36); 
    //        int y = Random.Range(-34, 36); 
    //        Vector3Int position = new Vector3Int(x, y, 0);

    //        if (!occupiedPositions.Contains(position) && IsPositionValidForResource(position))
    //        {
    //            return position;
    //        }
    //    }
    //    return Vector3Int.zero;
    //}
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

    private bool IsPositionValidForResource(Vector3Int position)
    {
        Vector3 worldPosition = topMap.CellToWorld(position) + topMap.cellSize / 2;

        if (Physics2D.OverlapCircle(worldPosition, 0.5f))
        {
            return false;
        }

        int bufferZoneSize = 6; 
        if (IsNearHole(position, bufferZoneSize))
        {
            return false;
        }

        return true;
    }

    private Vector3Int GetRandomValidPosition(HashSet<Vector3Int> occupiedPositions)
    {
        for (int attempt = 0; attempt < 100; attempt++)
        {
            int x = UnityEngine.Random.Range(-width / 2, width / 2);
            int y = UnityEngine.Random.Range(-height / 2, height / 2);
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
        if (rand < 0.33f)
        {
            size = 1f; // Small
        }
        else if (rand < 0.66f)
        {
            size = 2f; // Medium
        }
        else
        {
            size = 4f; // Large
        }
        return size;
    }
    private void distributeResource(Vector3Int cellPosition, float size, GameObject prefab)
    {
        Vector3 position = topMap.CellToWorld(cellPosition) + topMap.cellSize / 2;
        GameObject resourceObject = Instantiate(prefab, position, Quaternion.identity);
        IRecourses resource = resourceObject.GetComponent<IRecourses>();
        resource.Initialize(cellPosition, size);

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

    public void SaveAssetMap()
    {
        string saveName = "tmapXY_" + count;
        var mf = GameObject.Find("Grid");

        if (mf)
        {
            var savePath = "Assets/Prefabs/" + saveName + ".prefab";
            PrefabUtility.SaveAsPrefabAsset(mf, savePath);
            EditorUtility.DisplayDialog("Tilemap saved", "Your Tilemap was saved under " + savePath, "Continue");
        }
    }
    private void PlaceGold()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int cellPosition = new Vector3Int(-x + width / 2, -y + height / 2, 0);
                Vector3 worldPos = topMap.CellToWorld(cellPosition) + topMap.cellSize / 2;

                Vector3Int adjustedCellPosition = new Vector3Int(cellPosition.x - 6, cellPosition.y - 8, 0);
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
                            if (overlappingColliders.Length > 1)
                            {
                                Destroy(gold.gameObject);
                                continue; 
                            }

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

                Vector3Int adjustedCellPosition = new Vector3Int(cellPosition.x - 5, cellPosition.y - 8, 0);
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
                            if (overlappingColliders.Length > 1)
                            {
                                Destroy(wood.gameObject);
                                continue;
                            }

                            wood.transform.position = adjustedWorldPos;
                            Vector3Int gridCellPosition = topMap.WorldToCell(adjustedWorldPos);
                            GridManager.Instance.SetEntity(wood, new Indices(gridCellPosition.x + width / 2 - 3, gridCellPosition.y + height / 2 - 4));
                            GridManager.Instance.SetEntity(wood, new Indices(gridCellPosition.x + width / 2-2, gridCellPosition.y + height / 2 -3));
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

                Vector3Int adjustedCellPosition = new Vector3Int(cellPosition.x - 5, cellPosition.y - 8, 0);
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
                            if (overlappingColliders.Length > 1)
                            {
                                Destroy(stone.gameObject);
                                continue;
                            }

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