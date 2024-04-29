using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Character : MonoBehaviour
{
    //selected is set to true temporarily for testing
    //private bool selected = true;
    [SerializeField] private  float moveSpeed = 5f;
    private PathFinder pathFinder;
    private Grid<Cell> gridMap;
    //State related variables
    private enum CharacterState
    {
        IDLE,WALKING
    };
    private List<Vector3> currentPath;
    private int currentPathIndex = 0;
    private CharacterState currentCharacterState = CharacterState.IDLE;
    private Indices currentGridPosition;
    private void Start()
    {
        MouseManager.Instance.OnWalk += MouseManager_OnWalk;
        pathFinder = new PathFinder();
        gridMap = GameManager.Instance.gridMap;
        gridMap.GetIndices(transform.position, out currentGridPosition.I, out currentGridPosition.J);
    
    }
    private void Update()
    {
        switch (currentCharacterState)
        {
            case CharacterState.IDLE:
                break;
            case CharacterState.WALKING:
                WalkPath();
                break;
        }
    }
    private void MouseManager_OnWalk(Vector3 targetPosition)
    {
        ToIdle();
        Cell characterCell = gridMap.GetValue(transform.position);
        Cell targetCell = gridMap.GetValue(targetPosition);
        currentPath = pathFinder.FindPath(characterCell, targetCell);
        if (currentPath.Any())
        {
            currentCharacterState = CharacterState.WALKING;
        }
    }
    private bool Cast(Vector3 target)
    {
        return gridMap.GetValue(target).GetEntity() != Entity.SAFE;
    }
    private void WalkPath()
    {
        Vector3 nextTarget = currentPath[currentPathIndex];
        if (!Cast(nextTarget))
        {
            transform.position = Vector3.MoveTowards(transform.position, nextTarget, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, nextTarget) < 0.05)
            {
                currentPathIndex++;
                gridMap.SetValue(transform.position, (int I, int J) =>
                {
                    //move this character on the grid to another cell
                    Cell cell = new Cell(I, J);
                    cell.SetCharacter(this);
                    Cell prev = gridMap.GetValue(currentGridPosition.I, currentGridPosition.J);
                    prev.ClearCharacter();
                    currentGridPosition.I = I;
                    currentGridPosition.J = J;
                    return cell;
                });
                if (currentPathIndex == currentPath.Count)
                {
                    ToIdle();
                }
            }
        }
        else
        {
            Vector3 previousPosition = gridMap.GetWorldPositionCentered(currentGridPosition.I, currentGridPosition.J);
            transform.position = Vector3.MoveTowards(transform.position, previousPosition, moveSpeed * Time.deltaTime);
        }
    }
    private void ToIdle()
    {
        currentCharacterState=CharacterState.IDLE;
        currentPath?.Clear();
        currentPathIndex = 0;

    }
    public static Character Spawn(CharacterSO characterSO,Vector3 position)
    {
        Transform characterTransform = Instantiate(characterSO.prefab,position,Quaternion.identity);
        Character character = characterTransform.GetComponent<Character>();
        return character;
    }
}
