using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class CreateGrid : MonoBehaviour
{
    public enum TileType { TopMap, BotMap }

    [Header("Map Settings")]
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
    private TileType[,] tileArray;  // Array to save the grid map type
    public Vector3Int tmpSize;
    public Tilemap topMap;
    public Tilemap botMap;
    public RuleTile topTile;
    public Tile botTile;
    int width;
    int height;

    private void Start()
    {
        doSim(numR);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            doSim(numR);
        }

        if (Input.GetKey(KeyCode.H))
        {
            clearMap(true);
        }

        if (Input.GetKey(KeyCode.J))
        {
            SaveAssetMap();
            count++;
        }
    }

    public void doSim(int nu)
    {
        clearMap(false);
        width = tmpSize.x;
        height = tmpSize.y;

        terrainMap = new int[width, height];
        tileArray = new TileType[width, height]; // Initialize the array

        initPos();

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
                    tileArray[x, y] = TileType.TopMap;  // Mark as topMap
                }
                else
                {
                    botMap.SetTile(position, botTile);
                    tileArray[x, y] = TileType.BotMap;  // Mark as botMap
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
            tileArray = null; // Reset the tileArray
        }
    }

    public void SaveAssetMap()
    {
        string saveName = "tilemapXY_" + count;
        var mf = GameObject.Find("Grid");

        if (mf)
        {
            var savePath = "Assets/Prefabs/" + saveName + ".prefab";
            PrefabUtility.SaveAsPrefabAsset(mf, savePath);
            SaveTileArray(saveName);
            EditorUtility.DisplayDialog("Tilemap saved", "Your Tilemap was saved under " + savePath, "Continue");
        }
        count++;
    }

    private void SaveTileArray(string saveName)
    {
        string savePath = "Assets/Prefabs/" + saveName + "_grid.txt";
        using (StreamWriter writer = new StreamWriter(savePath))
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    writer.Write((int)tileArray[x, y] + " ");
                }
                writer.WriteLine();
            }
        }
    }
}
