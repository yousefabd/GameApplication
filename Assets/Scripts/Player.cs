using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<Character> selectedCharacters;
    private PathFinder pathFinder;
    public static Player Instance {  get; private set; }

    private void Awake()
    {
        Instance = this;
        selectedCharacters = new List<Character>();
        pathFinder = new PathFinder();
    }
    private void Start()
    {
        MouseManager.Instance.OnWalk += Player_OnWalk;
    }

    private void Player_OnWalk(Vector3 targetWorldPosition)
    {
        Indices targetPosition;
        Debug.Log(selectedCharacters.Count);
        GameManager.Instance.WorldToGridPosition(targetWorldPosition,out targetPosition.I,out targetPosition.J);
        List<Indices> targets=pathFinder.FindMultipleTargets(targetPosition, selectedCharacters.Count);
        Debug.Log(targets.Count);
        for (int i = 0; i < targets.Count; i++)
        {
            Debug.Log(targets[i].I + "," + targets[i].J);
            List<Vector3> path = pathFinder.FindPath(selectedCharacters[i].GetCurrentPosition(), targets[i]);
            selectedCharacters[i].SetPath(path);
        }
    }

    public void AddSelectedCharacter(Character character)
    {
        selectedCharacters.Add(character);
    }
}
