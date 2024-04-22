using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    private Vector2 currentPosition = Vector2.zero;
    private void Update()
    {
        //debug
        Vector2 position = GameManager.Instance.gridMap.GetIndices(UtilsClass.GetMouseWorldPosition());
        if (position != currentPosition)
        {
            currentPosition = position;
            Debug.Log(GameManager.Instance.gridMap.GetValue((int)position.x,(int)position.y));
        }
    }
}
