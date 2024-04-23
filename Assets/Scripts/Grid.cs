using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using static UnityEngine.Rendering.DebugUI;
using System;
public class Grid <TGridObject>
{
    private int width;
    private int height;
    private float cellSize;
    Vector3 originPosition;
    private TGridObject[,] gridArray;
    private TextMesh[,] worldTextRef;
    bool showDebug;
    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<int,int,TGridObject> CreateGridObject)
    {
        this.width = width; 
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        gridArray = new TGridObject[width, height];
        worldTextRef = new TextMesh[width, height];
        showDebug = true;
        //initialize the grid
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                gridArray[i, j] = CreateGridObject(i,j);
            }

        }
        if (showDebug)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    worldTextRef[i, j] = UtilsClass.CreateWorldText(gridArray[i, j]?.ToString(), null, GetWorldPosition(i, j) + new Vector3(cellSize, cellSize, 0f) / 2, 20, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.white, 100f);

                }

            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height));
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height));
        }
    }
    public Vector3 GetWorldPosition(int x,int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }
    public void SetValue(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            if (showDebug)
            {
                worldTextRef[x, y].text = value.ToString();
            }
        }
    }
    public void SetValue(Vector3 worldPosition, TGridObject value)
    {
        int x = (int)((worldPosition - originPosition).x / cellSize);
        int y = (int)((worldPosition - originPosition).y / cellSize);
        SetValue(x, y, value);
    }

    public TGridObject GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        Debug.Log(x + "," + y);
        return default;
    }
    public TGridObject GetValue(Vector3 worldPosition)
    {

        int x = (int)((worldPosition - originPosition).x / cellSize);
        int y = (int)((worldPosition - originPosition).y / cellSize);
        return GetValue(x, y);
    }
    public int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
    public float GetCellSize()
    {
        return cellSize;
    }
    public Vector3 GetOriginPosition()
    {
        return originPosition;
    }
    public void GetIndices(Vector3 worldPosition,out int x,out int y)
    {
        x = (int)((worldPosition - originPosition).x / cellSize);
        y = (int)((worldPosition - originPosition).y / cellSize);

    }
    public void PathFindTest(List<Vector3> directions,Vector2 start)
    {
        Debug.Log(directions.Count);
        foreach(Vector3 direction in directions)
        {
            string dir="";
            if(direction.x == 0)
            {
                if(direction.y == -1)
                {
                    dir = "D";
                }
                else if (direction.y == 1)
                {
                    dir = "U";
                }
            }
            else if (direction.x == -1)
            {
                if (direction.y == -1)
                {
                    dir = "DL";
                }
                else if (direction.y == 0)
                {
                    dir = "L";
                }
                else if (direction.y == 1)
                {
                    dir = "UL";
                }
            }
            else if (direction.x == 1)
            {
                if (direction.y == -1)
                {
                    dir = "DR";
                }
                else if (direction.y == 0)
                {
                    dir = "R";
                }
                else if (direction.y == 1)
                {
                    dir = "UR";
                }
            }
            worldTextRef[(int)start.x, (int)start.y].text = dir;
            worldTextRef[(int)start.x, (int)start.y].color = Color.green;
            start = new Vector2(start.x+direction.x, start.y+direction.y);
        }
    }
}
