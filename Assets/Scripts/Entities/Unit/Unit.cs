using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Unit : Entity, IDestructibleObject
{
    private float moveSpeed = 3f;
    [SerializeField] public UnitSO unitSO;
    private PathFinder pathFinder;
    //State related variables
    private enum UnitState
    {
        IDLE, WALKING, DYING
    };
    private List<Vector3> currentPath;
    private int currentPathIndex = 0;
    private UnitState currentUnitState = UnitState.IDLE;
    private Indices currentGridPosition;
    private bool selected = false;
    public float HealthPoints { get; set; }
    private float dieTimer = 1.5f;
    private bool movementPaused = false;
    private Vector3 currentMoveDir=Vector3.zero;
    //events
    public event Action<bool> OnSelect;
    public event Action<Vector3> OnMoveCell;
    public event Action OnSpawn;
    public event Action OnDestroyed;
    public event Action <float>OnDamaged;
    public event Action OnTakeAction;
    public event Action<float> OnMaxHealthChanged;
    public static event Action<Unit> OnFinishedPath;
    protected virtual void Awake()
    {
        team = unitSO.team;
        HealthPoints = unitSO.maxHealth;
        moveSpeed = unitSO.moveSpeed;
    }
    protected virtual void Start()
    {

        GridManager.Instance?.WorldToGridPosition(transform.position, out currentGridPosition.I, out currentGridPosition.J);
        OnSpawn?.Invoke();
        if (Player.Instance != null)
        {
            Player.Instance.OnAttacked += Damage;
        }
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
                if (dieTimer <= 0)
                {
                    Destruct();
                }
                break;

        }
    }

    private void WalkPath()
    {
        Vector3 nextTarget = currentPath[currentPathIndex];
        currentMoveDir = nextTarget - transform.position;
        if (!movementPaused)
            transform.position = Vector3.MoveTowards(transform.position, nextTarget, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, nextTarget) < 0.05)
        {
            currentPathIndex++;
            Indices previousPosition = currentGridPosition;
            if (GridManager.Instance != null)
            {
                GridManager.Instance?.WorldToGridPosition(
                    transform.position,
                    out currentGridPosition.I,
                    out currentGridPosition.J
                );
                GridManager.Instance?.MoveEntity(previousPosition, currentGridPosition, this);
            }
            if (currentPathIndex == currentPath.Count)
            {
                OnFinishedPath?.Invoke(this);
                OnTakeAction?.Invoke();
                ToIdle();
            }
            else
            {
                OnMoveCell?.Invoke(currentPath[currentPathIndex] - transform.position);
                OnTakeAction?.Invoke();
            }
        }

    }
    protected void ToIdle()
    {
        currentUnitState = UnitState.IDLE;
        currentPath?.Clear();
        currentPathIndex = 0;
        OnMoveCell?.Invoke(Vector3.zero);

    }
    public static Unit Spawn(UnitSO unitSO, Vector3 position)
    {
        Transform UnitTransform = Instantiate(unitSO.prefab, position, Quaternion.identity);
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
                OnFinishedPath?.Invoke(this);
            }
            OnTakeAction?.Invoke();
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
    public override void Damage(Vector3 position, float value)
    {
        if (currentUnitState == UnitState.DYING)
            return;
        if (position == transform.position)
        {
            OnDamaged?.Invoke(value);
            HealthPoints -= value;
            if (HealthPoints < 0)
            {
                ToIdle();
                ToggleSelect(false);
                //unsubscribe from event
                if (Player.Instance != null) 
                    Player.Instance.OnAttacked -= Damage;
                currentUnitState = UnitState.DYING;
                OnDestroyed?.Invoke();
            }
        }
    }
    public void Destruct()
    {
        if(GridManager.Instance != null)
            GridManager.Instance.SetEntity(null, currentGridPosition);

        Destroy(gameObject);
    }

    public bool CheckNextPathCell()
    {
        if (currentPath.Any())
        {
            if (currentPathIndex < currentPath.Count - 1)
            {
                return (GridManager.Instance.Overlap(currentPath[currentPathIndex + 1]) == null);
            }
        }
        return false;
    }
    public bool IsWalking()
    {
        return currentUnitState == UnitState.WALKING;
    }

    public void TogglePauseMovement(bool value)
    {
        movementPaused = value;
    }

    public bool IsSelected()
    {
        return selected;
    }

    public void SetSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    public float GetSpeed() 
    {  return moveSpeed; }

    public void SetMaxHealth(float newMaxHealth)
    {
        OnMaxHealthChanged?.Invoke(newMaxHealth);
        HealthPoints = newMaxHealth;
    }

    public float GetMaxHealth()
    {
        return unitSO.maxHealth;
    }
    public float GetMoveSpeed()
    {
        if (!IsWalking())
        {
            return 0f;
        }
        return moveSpeed;
    }
    public Vector3 GetMoveDir()
    {
        return currentMoveDir.normalized;
    }
    public bool IsDying()
    {
        return currentUnitState == UnitState.DYING;
    }
}
