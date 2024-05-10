using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<Character> selectedCharacters;
    private PathFinder pathFinder;
    private int movedCharacters = 0;
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
        if (selectedCharacters.Any())
        {
            movedCharacters = 0;
            Indices targetPosition;
            GridManager.Instance.WorldToGridPosition(targetWorldPosition, out targetPosition.I, out targetPosition.J);
            List<Vector3> originalPath = pathFinder.FindPath(selectedCharacters[0].GetCurrentPosition(), targetPosition);
            Indices newTarget;
            if (originalPath.Any())
            {
                GridManager.Instance.WorldToGridPosition(originalPath[^1], out newTarget.I, out newTarget.J);
            }
            else
            {
                newTarget = targetPosition;
            }
            selectedCharacters[0].SetPath(originalPath);
            List<Indices> targets = pathFinder.FindMultipleTargets(newTarget, selectedCharacters.Count);
            for (int i = 1; i < targets.Count; i++)
            {
                List<Vector3> path = pathFinder.FindPath(selectedCharacters[i].GetCurrentPosition(), targets[i]);
                selectedCharacters[i].SetPath(path);
            }
        }
    }

    public void AddSelectedCharacter(Character character)
    {
        if (character!=null)
        {
            selectedCharacters.Add(character);
            character.ToggleSelect(true);
        }
    }

    public void OnFinishedPath()
    {
        movedCharacters++;
        if (movedCharacters == selectedCharacters.Count)
        {
            for (int i = 0; i < selectedCharacters.Count; i++)
            {
                GridManager.Instance.SetEntity(selectedCharacters[i], selectedCharacters[i].GetCurrentPosition());
            }
        }
    }
    public void ClearSelectedCharacters()
    {
        for(int i = 0; i < selectedCharacters.Count; i++)
        {
            selectedCharacters[i].ToggleSelect(false);
        }
        selectedCharacters.Clear();
    }
}
