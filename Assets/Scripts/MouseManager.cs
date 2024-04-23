using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    PathFinder pathFinder = new PathFinder();
    private Vector2 currentPosition = Vector2.zero;
    private void Update()
    {
        //debug
        GameManager.Instance.gridMap.GetIndices(UtilsClass.GetMouseWorldPosition(),out int x,out int y);
        Vector2 position = new Vector2(x, y);
        if (position != currentPosition)
        {
            currentPosition = position;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Cell newCell = new Cell((int)position.x, (int)position.y);
            newCell.entity=Entity.OBSTACLE;
            GameManager.Instance.gridMap.SetValue(UtilsClass.GetMouseWorldPosition(), newCell);
        }
        if (Input.GetMouseButtonDown(1))
        {
            List<Vector3> path=pathFinder.FindPath(GameManager.Instance.gridMap.GetValue(0, 0), GameManager.Instance.gridMap.GetValue((int)position.x,(int)position.y));
            GameManager.Instance.gridMap.PathFindTest(path, new Vector2(0, 0));
        }
    }
}
