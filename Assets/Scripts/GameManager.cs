using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Vector3 gridOriginPosition = new Vector3(-16f, -9f);
    private const int gridWidth = 32;
    private const int gridHeight = 18;
    private const float cellSize = 1f;

    public Grid<Cell> gridMap;

    public static GameManager Instance { get; private set; }
    void Awake()
    {
        Instance= this;
        gridMap = new Grid<Cell>(gridWidth, gridHeight, cellSize, gridOriginPosition,(int x,int y)=>new Cell(x,y));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
