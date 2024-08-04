using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[Serializable]
public enum Team
{
    HUMANS, GOBLINS
}
public class Player : MonoBehaviour
{
    public static int goldStorage;

    private List<Unit> selectedUnits;
    private PathFinder pathFinder;
    private Team pickedTeam { get; set; }
    public event Action<Vector3, float> OnAttacked;
    public event Action<Entity> OnSetTarget;
    public event Action OnClearTarget;
    public static Player Instance { get; private set; }

    public Dictionary<BuildingType,int> currentBuildingCount;

    public GameRules gameRules;

    private void Awake()
    {
        Instance = this;
        selectedUnits = new List<Unit>();
        pathFinder = new PathFinder();
        pickedTeam = Team.HUMANS;
    }
    private void Start()
    {
        currentBuildingCount = new Dictionary<BuildingType, int>(gameRules.buildingCount);
        var keys = currentBuildingCount.Keys.ToList();
        for (int i = 0; i < keys.Count; i++)
        {
            currentBuildingCount[keys[i]] = 0;
        }
        Debug.Log("after");
        ScreenInteractionManager.Instance.OnRightMouseButtonClicked += Player_HandleInteraction;
        ScreenInteractionManager.Instance.OnAreaSelected += ScreenInteractionManager_OnAreaSelected;
        Unit.OnFinishedPath += Unit_OnFinishedPath;
        BuildingManager.Instance.built += BuildingManager_OnBuilt;
    }

    private void BuildingManager_OnBuilt(Building building)
    {
        Builder[] buildersList = FindObjectsByType<Builder>(FindObjectsSortMode.None);
        Debug.Log("bruh");
        if (buildersList != null)
        {
            Builder closestBuilder = buildersList[0];
            float minDistance = Mathf.Infinity;
            foreach (Builder builder in buildersList)
            {
                if (minDistance < Vector3.Distance(builder.transform.position, building.transform.position))
                {
                    minDistance = Vector3.Distance(builder.transform.position, building.transform.position);
                    closestBuilder = builder;
                }
            }
            Indices buildingPosition = new Indices();
            Indices builderPosition = new Indices();
            GridManager.Instance.WorldToGridPosition(building.transform.position, out buildingPosition.I, out buildingPosition.J);
            GridManager.Instance.WorldToGridPosition(building.transform.position, out builderPosition.I, out builderPosition.J);
            List<Vector3>path = pathFinder.FindPath(builderPosition, buildingPosition);
            if (path != null)
            {
                closestBuilder.SetPath(path);
            }
        
        }
    }

    private void initializeBuildingDictionary()
    {

    }
    private void ScreenInteractionManager_OnAreaSelected(Vector3 start, Vector3 end)
    {
        ClearSelectedUnits();
        Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(start, end);
        foreach (Collider2D collider2D in collider2DArray)
        {
            Unit unit = collider2D.GetComponent<Unit>();
            if (unit != null)
            {
                if (unit.GetTeam() == pickedTeam)
                    AddSelectedUnit(unit);
            }
        }
    }

    private void Update()
    {
    }
    private List<List<Vector3>> FindMultiplePaths(List<Unit> units, Vector3 targetWorldPosition)
    {
        List<List<Vector3>> paths = new List<List<Vector3>>();
        if (units.Any())
        {
            Indices targetPosition;
            GridManager.Instance.WorldToGridPosition(targetWorldPosition, out targetPosition.I, out targetPosition.J);
            List<Vector3> originalPath = pathFinder.FindPath(units[0].GetCurrentPosition(), targetPosition);
            Indices newTarget;
            if (originalPath.Any())
            {
                GridManager.Instance.WorldToGridPosition(originalPath[^1], out newTarget.I, out newTarget.J);
            }
            else
            {
                newTarget = targetPosition;
            }
            paths.Add(originalPath);
            List<Indices> targets = pathFinder.FindMultipleTargets(newTarget, units.Count);
            for (int i = 1; i < targets.Count; i++)
            {
                List<Vector3> path = pathFinder.FindPath(units[i].GetCurrentPosition(), targets[i]);
                paths.Add(path);
            }
        }
        return paths;
    }

    private void Player_HandleInteraction(Vector3 targetPosition)
    {
        Entity target = GridManager.Instance.GetEntity(targetPosition);
        if (target == null)
        {
            OnClearTarget?.Invoke();
            Walk(targetPosition);
        }
        else if (target.GetTeam().Equals(pickedTeam))
        {
            OnClearTarget?.Invoke();
            Walk(targetPosition);
        }
        else
        {
            StartAttack(targetPosition, target);
        }
    }
    private void Walk(Vector3 targetWorldPosition)
    {
        List<List<Vector3>> paths = FindMultiplePaths(selectedUnits, targetWorldPosition);
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            selectedUnits[i].SetPath(paths[i]);
        }
    }
    private void StartAttack(Vector3 targetWorldPosition, Entity enemy)
    {
        Indices targetPosition;
        GridManager.Instance.WorldToGridPosition(targetWorldPosition, out targetPosition.I, out targetPosition.J);
        OnSetTarget?.Invoke(enemy);
        List<Unit> soldiers = new List<Unit>();
        List<Unit> other = new List<Unit>();

        if (selectedUnits.Any())
        {
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                if (selectedUnits[i] is Soldier)
                {
                    soldiers.Add(selectedUnits[i]);
                }
            }
        }
        if (soldiers.Any())
        {
            bool[,] visited = new bool[GridManager.Instance.GetWidth(), GridManager.Instance.GetHeight()];
            List<List<Vector3>> soldierPaths = FindMultiplePaths(soldiers, targetWorldPosition);
            for (int i = 0; i < soldierPaths.Count; i++)
            {
                List<Vector3> soldierPath = new List<Vector3>();
                if ((soldiers[i] as Soldier).CanAttack(soldiers[i].transform.position))
                {
                    soldiers[i].SetPath(soldierPath);
                    continue;
                }
                for (int j = 0; j < soldierPaths[i].Count; j++)
                {

                    soldierPath.Add(soldierPaths[i][j]);
                    if ((soldiers[i] as Soldier).CanAttack(soldierPaths[i][j]))
                    {
                        Indices pathCell;
                        GridManager.Instance.WorldToGridPosition(soldierPaths[i][j], out pathCell.I, out pathCell.J);
                        if (!visited[pathCell.I, pathCell.J])
                        {
                            visited[pathCell.I, pathCell.J] = true;
                            break;
                        }
                    }
                }
                soldiers[i].SetPath(soldierPath);
            }
        }
    }
    public void OnAttackCallback(Vector3 attackedPosition, float damage)
    {
        OnAttacked?.Invoke(attackedPosition, damage);
    }
    public void AddSelectedUnit(Unit unit)
    {
        if (unit != null)
        {
            selectedUnits.Add(unit);
            unit.ToggleSelect(true);
        }
    }

    public void Unit_OnFinishedPath(Unit unit)
    {
        GridManager.Instance.SetEntity(unit, unit.GetCurrentPosition());
    }
    public void ClearSelectedUnits()
    {
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            if (selectedUnits[i] != null)
            {
                selectedUnits[i].ToggleSelect(false);
            }
            else
            {
                selectedUnits.RemoveAt(i);
            }
        }
        selectedUnits.Clear();
    }
    public Team GetTeam()
    {
        return pickedTeam;
    }
}
