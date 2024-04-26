using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Character : MonoBehaviour
{
    //selected is set to true temporarily for testing
    //private bool selected = true;
    [SerializeField] private const float moveSpeed = 2f;
    private PathFinder pathFinder;

    //State related variables
    private enum CharacterState
    {
        IDLE,WALKING
    };
    private List<Vector3> currentPath;
    private int currentPathIndex = -1;
    private CharacterState currentCharacterState = CharacterState.IDLE;
    private float currentMoveDistance = 0f;
    private float maxMoveDistance = 0f;
    private Vector3 currentMoveDirection=Vector3.zero;
    private void Start()
    {
        MouseManager.Instance.OnWalk += MouseManager_OnWalk;
        pathFinder = new PathFinder();
    }
    private void Update()
    {
        switch (currentCharacterState)
        {
            case CharacterState.IDLE:
                break;
            case CharacterState.WALKING:
                if (currentMoveDistance >= maxMoveDistance)
                {
                    WalkCell();
                }
                float moveDistance = moveSpeed * Time.deltaTime;
                currentMoveDistance += moveDistance;
                transform.position += currentMoveDirection * moveDistance;
                break;
        }
    }
    private void MouseManager_OnWalk(Vector3 targetPosition)
    {
        
        Cell characterCell = GameManager.Instance.gridMap.GetValue(transform.position);
        Cell targetCell = GameManager.Instance.gridMap.GetValue(targetPosition);
        currentPath = pathFinder.FindPath(characterCell, targetCell);
        Debug.Log(currentPath.Count);
        if (currentPath.Any())
        {
            currentCharacterState = CharacterState.WALKING;
        }
    }
    private void WalkCell()
    {
        currentMoveDistance = 0f;
        
        currentPathIndex++;
        if (currentPathIndex >= currentPath.Count)
        {
            ToIdle();
        }
        else
        {
            currentMoveDirection = currentPath[currentPathIndex];
            currentMoveDirection.Normalize();
            float cellSize = GameManager.Instance.gridMap.GetCellSize();
            if (currentMoveDirection.x != 0f && currentMoveDirection.y != 0f)
            {
                maxMoveDistance = Mathf.Sqrt(2 * cellSize * cellSize);
            }
            else
            {
                maxMoveDistance = cellSize;
            }
        }
    }
    private void ToIdle()
    {
        currentCharacterState=CharacterState.IDLE;
        currentPath.Clear();
        currentPathIndex = -1;
        currentMoveDistance = 0f;
        maxMoveDistance = 0f;
        currentMoveDirection=Vector3.zero;

    }
    public static Character Spawn(CharacterSO characterSO,Vector3 position)
    {
        Transform characterTransform = Instantiate(characterSO.prefab,position,Quaternion.identity);
        Character character = characterTransform.GetComponent<Character>();
        return character;
    }
}
