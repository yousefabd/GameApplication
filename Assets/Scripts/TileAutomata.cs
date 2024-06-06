using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class TileAutomata : MonoBehaviour
{

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
    [Header("Decoration Settings")]
    public Tilemap decorationMap;
    public Tile lutosLeafTile;
    public Tile flowerTile;
    public float decorationChance = 0.0001f;
    [Header("Gold Settings")]
    public Sprite goldSprite;
    public float goldChance = 0.1f;
    private List<Sprite> goldInstances = new List<Sprite>();
    [Header("Wood Settings")]
    public Sprite woodSprite;
    public float woodChance = 0.1f;
    private List<Sprite> woodInstances = new List<Sprite>();
    [Header("Rock Settings")]
    public Sprite rockSprite;
    public float rockChance = 0.1f;
    private List<Sprite> rockInstances = new List<Sprite>();

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
                    //if (grid != null)
                    //{
                    //    // It's safe to access members of grid here
                    //    //grid.GetValue(x, y).SetEntity(Entity.SAFE);
                    //}
                    if (grid != null)
                    {
                        // It's safe to access members of grid here
                        grid.GetValue(x, y).SetEntity(null);
                    }
                }
                else
                {
                    botMap.SetTile(position, botTile);
                    //if (grid != null)
                    //{
                    //    // It's safe to access members of grid here
                    //    //grid.GetValue(x, y).SetEntity(Entity.OBSTACLE);
                    //}
                    if (grid != null)
                    {
                        // It's safe to access members of grid here
                        //d.GetValue(x, y).SetEntity(test);
                    }
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
                int cell = Random.Range(1, 101) < iniChance ? 1 : 0;
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

                    else
                    {
                        newMap[x, y] = 1;

                    }
                }

                if (oldMap[x, y] == 0)
                {
                    if (neighb > birthLimit) newMap[x, y] = 1;

                    else
                    {
                        newMap[x, y] = 0;
                    }
                }

            }

        }



        return newMap;
    }

    public void distributeDecorations()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int position = new Vector3Int(-x + width / 2, -y + height / 2, 0);
                if (terrainMap[x, y] == 0 && Random.value < decorationChance)
                {
                    decorationMap.SetTile(position, lutosLeafTile);
                }
                else if (terrainMap[x, y] == 1 && Random.value < decorationChance)
                {
                    decorationMap.SetTile(position, flowerTile);
                }
            }
        }
    }

    private void Start()
    {

        doSim(numR);
        distributeResources();
        distributeDecorations();

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

    }

    public void ClearResource(List<Sprite> resourceInstances)
    {
        foreach (Sprite resourceInstance in resourceInstances)
        {
            Destroy(resourceInstance);
        }

        resourceInstances.Clear();
    }

    public void SaveAssetMap()
    {
        string saveName = "tmapXY_" + count;
        var mf = GameObject.Find("Grid");

        if (mf)
        {
            var savePath = "Assets/Prefabs/" + saveName + ".prefab";
            if (PrefabUtility.CreatePrefab(savePath, mf))
            {
                EditorUtility.DisplayDialog("Tilemap saved", "Your Tilemap was saved under" + savePath, "Continue");
            }
            else
            {
                EditorUtility.DisplayDialog("Tilemap NOT saved", "An ERROR occured while trying to saveTilemap under" + savePath, "Continue");
            }


        }


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


    public void distributeResources()
    {
        ClearResource(goldInstances);
        ClearResource(woodInstances);
        ClearResource(rockInstances);

        int bufferZoneSize = 6;

        for (int x = bufferZoneSize; x < width - bufferZoneSize; x++)
        {
            for (int y = bufferZoneSize; y < height - bufferZoneSize; y++)
            {
                bool isNearEdge = false;
                for (int dx = -bufferZoneSize; dx <= bufferZoneSize; dx++)
                {
                    for (int dy = -bufferZoneSize; dy <= bufferZoneSize; dy++)
                    {
                        if (terrainMap[x + dx, y + dy] == 0)
                        {
                            isNearEdge = true;
                            break;
                        }
                    }

                    if (isNearEdge)
                    {
                        break;
                    }
                }

                if (!isNearEdge && terrainMap[x, y] == 1 && x > 0 && y > 0 && x < width - 1 && y < height - 1)
                {
                    Vector3Int position = new Vector3Int(-x + width / 2, -y + height / 2, 0);
                    Vector3 worldPos = topMap.CellToWorld(position);
                    worldPos.x += 0.5f;
                    worldPos.y += 0.5f;

                    float size;
                    float rand = Random.value;
                    if (rand < 0.33f)
                    {
                        size = 0.5f; // Small
                    }
                    else if (rand < 0.66f)
                    {
                        size = 1.0f; // Medium
                    }
                    else
                    {
                        size = 1.5f; // Large
                    }
                    float resourceRand = Random.value;
                    if (resourceRand < rockChance)
                    {
                        distributeResource(rockSprite, rockInstances, worldPos, size);
                    }
                    else if (resourceRand < woodChance)
                    {
                        distributeResource(woodSprite, woodInstances, worldPos, size);
                    }
                    else if (resourceRand < goldChance)
                    {
                        distributeResource(goldSprite, goldInstances, worldPos, size);
                    }
                }
            }
        }
    }

    private void distributeResource(Sprite sprite, List<Sprite> instances, Vector3 worldPos, float size)
    {

        GameObject instance = new GameObject();
        SpriteRenderer renderer = instance.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sortingLayerName = "Resources";
        renderer.sortingOrder = 3;
        instance.transform.position = worldPos;
        instance.transform.localScale = new Vector3(size, size, size);
        instances.Add(sprite);
    }

}