using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDCastle : Entity,IDestructibleObject
{
    [SerializeField] private float castleHealth;

    public event Action<float> OnDamaged;
    public event Action OnDestroyed;

    public float HealthPoints { get; set; }

    private void Awake()
    {
        HealthPoints = castleHealth;
        team = Team.HUMANS;
    }
    private void Start()
    {
        TDUnitSpawner.Instance.OnUnitSpawned += OnUnitSpawned;
    }

    private void OnUnitSpawned(Unit unit)
    {
        (unit as Soldier).SetTarget(this);
        (unit as Soldier).OnAttack += Damage;
    }


    public void Damage(Vector3 position,float value)
    {
        if (position == transform.position)
        {
            OnDamaged?.Invoke(value);
        }
    }

    public void Destruct()
    {
        OnDestroyed?.Invoke();
        Destroy(gameObject);
    }
}
