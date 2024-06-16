using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<Unit> selectedCharacters;
    private PathFinder pathFinder;
    private int movedCharacters = 0;
    public event Action<Indices, float> OnAttack;
    public static Player Instance {  get; private set; }

    private void Awake()
    {
        Instance = this;
        selectedCharacters = new List<Unit>();
        pathFinder = new PathFinder();
    }
    private void Start()
    {
        ScreenInteractionManager.Instance.OnRightMouseButtonClicked += Player_OnWalk;
        ScreenInteractionManager.Instance.OnAreaSelected += ScreenInteractionManager_OnAreaSelected;
    }

    private void ScreenInteractionManager_OnAreaSelected(Vector3 start, Vector3 end)
    {
        ClearSelectedUnits();
        Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(start, end);
        foreach (Collider2D collider2D in collider2DArray)
        {
            Unit unit = collider2D.GetComponent<Unit>();
            AddSelectedUnit(unit);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnAttack?.Invoke(new Indices(5, 5), 23f);
        }
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
                selectedCharacters[i]?.SetPath(path);
            }
        }
    }

    public void AddSelectedUnit(Unit character)
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
    public void ClearSelectedUnits()
    {
        for(int i = 0; i < selectedCharacters.Count; i++)
        {
            if (selectedCharacters[i] != null)
            {
                selectedCharacters[i].ToggleSelect(false);
            }
            else
            {
                selectedCharacters.RemoveAt(i);
            }
        }
        selectedCharacters.Clear();
    }
}
