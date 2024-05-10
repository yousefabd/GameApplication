using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Character : Entity
{
    [SerializeField] private  float moveSpeed = 3f;
    private PathFinder pathFinder;
    //State related variables
    private enum CharacterState
    {
        IDLE,WALKING
    };
    private List<Vector3> currentPath;
    private int currentPathIndex = 0;
    private CharacterState currentCharacterState = CharacterState.IDLE;
    private Indices currentGridPosition;
    private bool selected = false;
    //events
    public event Action <bool> OnSelect;

    private void Start()
    {
        pathFinder = new PathFinder();
        GridManager.Instance.WorldToGridPosition(transform.position, out currentGridPosition.I, out currentGridPosition.J);
    
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
    
    private void WalkPath()
    {
        Vector3 nextTarget = currentPath[currentPathIndex];
        transform.position = Vector3.MoveTowards(transform.position, nextTarget, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, nextTarget) < 0.05)
        {
            currentPathIndex++;
            Indices previousPosition = currentGridPosition;
            GridManager.Instance.WorldToGridPosition(
                transform.position,
                out currentGridPosition.I,
                out currentGridPosition.J
            );
            GridManager.Instance.MoveEntity(previousPosition, currentGridPosition, this);
            if (currentPathIndex == currentPath.Count)
            {
                Player.Instance.OnFinishedPath();
                ToIdle();
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
    public void SetPath(List<Vector3> path)
    {
        ToIdle();
        currentPath = path;
        if (currentPath.Any())
        {
            currentCharacterState = CharacterState.WALKING;
        }
        else
        {
            Player.Instance.OnFinishedPath();
        }
    }
    public Indices GetCurrentPosition()
    {
        return currentGridPosition;
    }
    public void ToggleSelect(bool selected)
    {
        this.selected = selected;
        OnSelect?.Invoke(selected);

    }
}
