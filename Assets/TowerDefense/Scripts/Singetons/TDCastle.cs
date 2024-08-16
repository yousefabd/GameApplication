using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDCastle : Entity,IDestructibleObject
{
    [SerializeField] private float castleHealth;

    public event Action<float> OnDamaged;
    public event Action OnDestroyed;
    public event Action<int> OnGameOver;
    public event Action <float>OnFortify;
    public static TDCastle Instance { get; private set; }
    public float HealthPoints { get; set; }

    private void Awake()
    {
        HealthPoints = castleHealth;
        team = Team.HUMANS;
        Instance = this;
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


    public override void Damage(Vector3 position,float value)
    {
        if (position == transform.position)
        {
            HealthPoints -= value;
            OnDamaged?.Invoke(value);
            if(HealthPoints <= 0)
            {
                OnGameOver?.Invoke(TDWaveManager.Instance.GetCurrentWave());
                Debug.Log("game over");
            }
        }
    }

    public void Destruct()
    {
        OnDestroyed?.Invoke();
        Destroy(gameObject);
    }

    public bool Repair(int amount)
    {
        if (HealthPoints < castleHealth)
        {
            Damage(transform.position, -1.0f * amount);
            if(HealthPoints > castleHealth)
            {
                HealthPoints = castleHealth;
            }
            return true;
        }
        return false;
    }
    public void Fortify()
    {
        castleHealth += 1000;
        HealthPoints += 1000;
        OnFortify?.Invoke(castleHealth);
        OnDamaged?.Invoke(-1*HealthPoints);
    }
    public float GetMaxHealth()
    {
        return castleHealth;
    }
}
