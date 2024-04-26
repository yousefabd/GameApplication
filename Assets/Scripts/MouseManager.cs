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

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        //debug
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.gridMap.SetValue(
                mousePosition,
                (int x, int y) =>
                {
                    Cell newCell = new Cell(x, y);
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