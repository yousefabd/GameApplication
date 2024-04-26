using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Character : MonoBehaviour
{
    //selected is set to true temporarily for testing
    //private bool selected = true;
    [SerializeField] private const float moveSpeed = 5f;
    private PathFinder pathFinder;

    //State related variables
    private enum CharacterState
    {
        IDLE,WALKING
    };
    private List<Vector3> currentPath;
    private int currentPathIndex = 0;
    private CharacterState currentCharacterState = CharacterState.IDLE;
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
                WalkPath();
                break;
        }
    }
    private void MouseManager_OnWalk(Vector3 targetPosition)
    {
        ToIdle();
        Cell characterCell = GameManager.Instance.gridMap.GetValue(transform.position);
        Cell targetCell = GameManager.Instance.gridMap.GetValue(targetPosition);
        currentPath = pathFinder.FindPath(characterCell, targetCell);
        if (currentPath.Any())
        {
            currentCharacterState = CharacterState.WALKING;
        }
    }
    private void WalkPath()
    {
        if(currentPathIndex == currentPath.Count)
        {
            ToIdle();
        }
        else
        {
            Vector3 nextTarget = currentPath[currentPathIndex];
            transform.position = Vector3.MoveTowards(transform.position, nextTarget, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, nextTarget) < 0.05)
            {
                currentPathIndex++;
            }
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
