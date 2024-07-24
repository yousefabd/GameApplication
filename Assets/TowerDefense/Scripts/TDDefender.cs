using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDDefender : MonoBehaviour
{
    Unit unit;
    private float maxAttackCooldown = 1f;
    private float attackCooldown = 1f;
    private void Start()
    {
        unit = GetComponent<Unit>();
    }
    private void Update()
    {
        attackCooldown -= Time.deltaTime;
        Defend();
    }
    private void Defend()
    {
        if (!(unit as Soldier).HasTarget())
        {
            (unit as Soldier).LookForTargets();
        }
        else if (!(unit as Soldier).CanAttack(transform.position))
        {
            (unit as Soldier).LookForTargets();
        }
        else if(attackCooldown < 0)
        {
            (unit as Soldier).RangedAttack();
            attackCooldown = maxAttackCooldown;
        }
    }
}
