using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Unit
{
    private Entity currentTargetEnemy;
    private float attackRadius;
    private float attackDamage;
    public event Action<Vector3> OnSoldierAttack;
    private void Awake()
    {
        base.OnTakeAction += Soldier_OnTakeAction;
        attackRadius = base.unitSO.interactionRadius;
        Player.Instance.OnSetTarget += Player_OnSetTarget;
        Player.Instance.OnClearTarget += Player_OnClearTarget;
        attackDamage = unitSO.attackPower;
        team = unitSO.team;
    }

    private void Player_OnClearTarget()
    {
        currentTargetEnemy = null;
    }

    private void Player_OnSetTarget(Entity target)
    {
        currentTargetEnemy = target;
    }

    private void Soldier_OnTakeAction()
    {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        foreach (Collider2D collider in collider2DArray)
        {
            Entity hitEntity = collider.GetComponent<Entity>();
            if (hitEntity == this)
            {
                continue;
            }
            if (currentTargetEnemy != null)
            {
                if ((currentTargetEnemy.Equals(hitEntity)) && currentTargetEnemy.GetTeam() != this.GetTeam())
                {
                    Attack();
                }
            }

        }
    }

    private void Attack()
    {
        ToIdle();
        OnSoldierAttack?.Invoke(Vector2.up);
        Player.Instance.OnAttackCallback(currentTargetEnemy.transform.position, attackDamage);
    }

    public bool CanAttack(Vector3 standingPosition)
    {
        return Vector3.Distance(currentTargetEnemy.transform.position, standingPosition) <= attackRadius;
    }
}
