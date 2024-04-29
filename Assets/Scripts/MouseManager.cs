using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    PathFinder pathFinder = new PathFinder();
    private Vector2 currentPosition = Vector2.zero;
    public static MouseManager Instance { get; private set; }
    public event Action<Vector3> OnWalk;
    private Grid<Cell> gridMap;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        gridMap =GameManager.Instance.gridMap;
    }
    private void Update()
    {
        //debug
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(gridMap.GetValue(mousePosition).ToString());
        }
        if (Input.GetMouseButtonDown(0))
        {
            gridMap.SetValue(
                mousePosition,
                (int i, int j) =>
                {
                    Cell newCell = new Cell(i, j);
                    newCell.SetEntity(Entity.OBSTACLE);
                    return newCell;
            });
        }
        if (Input.GetMouseButtonDown(1))
        {
            OnWalk?.Invoke(mousePosition);
        }
    }
}