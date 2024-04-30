using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Character : MonoBehaviour
{
    //selected is set to true temporarily for testing
    //private bool selected = true;
    [SerializeField] private  float moveSpeed = 3f;
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
    
    private void WalkPath()
    {
        Vector3 nextTarget = currentPath[currentPathIndex];
        transform.position = Vector3.MoveTowards(transform.position, nextTarget, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, nextTarget) < 0.05)
        {
            currentPathIndex++;
            Cell prevCell = gridMap.GetValue(currentGridPosition.I, currentGridPosition.J);
            prevCell.ClearCharacter();
            Cell newCell=gridMap.GetValue(transform.position);
            newCell.GetIndices(out currentGridPosition.I,out currentGridPosition.J);
            newCell.SetCharacter(this);
            gridMap.UpdateValues();
            if (currentPathIndex == currentPath.Count)
            {
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
    }
    public Indices GetCurrentPosition()
    {
        return currentGridPosition;
    }
}
