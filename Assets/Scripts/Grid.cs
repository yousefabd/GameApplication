using CodeMonkey.Utils;
using System;
using UnityEngine;
public struct Indices
{
    public int I;
    public int J;
    public Indices(int I, int J)
    {
        this.I = I;
        this.J = J;
    }
}
[System.Serializable]
public class Grid<TGridObject>
{
    private int width;
    private int height;
    private float cellSize;
    Vector3 originPosition;
    private TGridObject[,] gridArray;
    private TextMesh[,] worldTextRef;
    bool showDebug = false;
    public void Awake()
    {

    }

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<int, int, TGridObject> CreateGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        // Initialize gridArray before the loop
        gridArray = new TGridObject[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                gridArray[i, j] = CreateGridObject(i, j);
            }
        }
        showDebug = true;
        if (showDebug)
        {
            worldTextRef = new TextMesh[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    worldTextRef[i, j] = UtilsClass.CreateWorldText(gridArray[i, j]?.ToString(), null, GetWorldPosition(i, j) + new Vector3(cellSize, cellSize, 0f) / 2, 15, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height));
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height));
        }

    }
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }
    public Vector3 GetWorldPositionCentered(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition + new Vector3(cellSize, cellSize, 0f) / 2;
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
    public void SetValue(Vector3 worldPosition, Func<int, int, TGridObject> createValue)
    {
        Debug.LogWarning("SetValue function should never be called!");
        int x = (int)((worldPosition - originPosition).x / cellSize);
        int y = (int)((worldPosition - originPosition).y / cellSize);
        TGridObject value = createValue(x, y);
        SetValue(x, y, value);
    }


    public TGridObject GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
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
    public void GetIndices(Vector3 worldPosition, out int x, out int y)
    {
        x = (int)((worldPosition - originPosition).x / cellSize);
        y = (int)((worldPosition - originPosition).y / cellSize);

    }
    public void UpdateValues()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {

                worldTextRef[i, j].text = gridArray[i, j].ToString();

            }
        }

    }


    public TGridObject[,] GetGridArray()
    {
        return gridArray;
    }
}


