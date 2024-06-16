using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Unit : Entity, IDestructibleObject
{
    [SerializeField] private  float moveSpeed = 3f;
    [SerializeField] private UnitSO unitSO;
    private PathFinder pathFinder;
    //State related variables
    private enum UnitState
    {
        IDLE,WALKING,DYING
    };
    private List<Vector3> currentPath;
    private int currentPathIndex = 0;
    private UnitState currentUnitState = UnitState.IDLE;
    private Indices currentGridPosition;
    private bool selected = false;
    public float HealthPoints { get; set; }
    private float dieTimer=1.5f;
    //events
    public event Action <bool> OnSelect;
    public event Action<Vector3> OnMoveCell;
    public event Action OnSpawn;
    public event Action OnDie;
    public event Action <float>OnDamaged;
    private void Awake()
    {
        pathFinder = new PathFinder();
        HealthPoints = unitSO.maxHealth;
    }
    private void Start()
    {
        
        GridManager.Instance.WorldToGridPosition(transform.position, out currentGridPosition.I, out currentGridPosition.J);
        OnSpawn?.Invoke();
        Player.Instance.OnAttack += Damage;
    }
    private void Update()
    {
        switch (currentUnitState)
        {
            case UnitState.IDLE:
                break;
            case UnitState.WALKING:
                WalkPath();
                break;
            case UnitState.DYING:
                dieTimer -= Time.deltaTime;
                if(dieTimer <= 0)
                {
                    Destruct();
                }
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
            else
            {
                OnMoveCell?.Invoke(currentPath[currentPathIndex] - transform.position);
            }
        }
        
    }
    private void ToIdle()
    {
        currentUnitState=UnitState.IDLE;
        currentPath?.Clear();
        currentPathIndex = 0;
        OnMoveCell?.Invoke(Vector3.zero);

    }
    public static Unit Spawn(UnitSO unitSO,Vector3 position)
    {
        Transform UnitTransform = Instantiate(unitSO.prefab,position,Quaternion.identity);
        Unit unit = UnitTransform.GetComponent<Unit>();
        return unit;
    }
    public void SetPath(List<Vector3> path)
    {
        if (currentUnitState != UnitState.DYING)
        {
            ToIdle();
            currentPath = path;
            if (currentPath.Any())
            {
                OnMoveCell?.Invoke(currentPath[0] - transform.position);
                currentUnitState = UnitState.WALKING;
            }
            else
            {
                Player.Instance.OnFinishedPath();
            }
        }
    }
    public Indices GetCurrentPosition()
    {
        return currentGridPosition;
    }
    public void ToggleSelect(bool selected)
    {
        if (currentUnitState != UnitState.DYING)
        {
            this.selected = selected;
            OnSelect?.Invoke(selected);
        }

    }
    public void Damage(Indices position, float value)
    {
        if (position.I == currentGridPosition.I && position.J == currentGridPosition.J)
        {
            OnDamaged?.Invoke(value);
            HealthPoints -= value;
            if (HealthPoints < 0)
            {
                ToIdle();
                ToggleSelect(false);
                //unsubscribe from event
                Player.Instance.OnAttack -= Damage;
                currentUnitState = UnitState.DYING;
                OnDie?.Invoke();
            }
        }
    }
    public void Destruct()
    {
        GridManager.Instance.SetEntity(null, currentGridPosition);
        
        Destroy(gameObject);
    }
}
