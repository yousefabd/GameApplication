using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAI : MonoBehaviour
{
    private Soldier soldierController;
    private enum SoldierAIState { IDLE, ATTACKING }
    private PathFinder autoPathFinder;
    //state related variables
    private SoldierAIState currentSoldierState=SoldierAIState.IDLE;
    private float currentAttackCooldown = 0f;
    //events
    public event Action OnAttack;
    private void Start()
    {
        soldierController = GetComponent<Soldier>();
        if(soldierController != null)
        {
            soldierController.OnStartAttacking += Soldier_OnStartAttacking;
        }
        autoPathFinder = new PathFinder();
    }
    private void Update()
    {
        switch (currentSoldierState)
        {
            case SoldierAIState.IDLE:
                if (currentAttackCooldown < 0f)
                    currentAttackCooldown = 0f;
                break;
            case SoldierAIState.ATTACKING:
                FollowTarget();
                break;
        }
        currentAttackCooldown -= Time.deltaTime;
    }
    private void Soldier_OnStartAttacking()
    {
        currentSoldierState = SoldierAIState.ATTACKING;
    }
    private void FollowTarget()
    {
        if (soldierController.HasTarget())
        {
            if (soldierController.CanAttack(transform.position))
            {
                if (currentAttackCooldown <= 0f)
                {
                    OnAttack?.Invoke();
                    currentAttackCooldown = soldierController.attackCoolDown;
                }
            }
            else if(!soldierController.IsWalking())
            {
                Debug.Log("walk to target");
                Indices start;
                Indices target;
                GridManager.Instance.WorldToGridPosition(transform.position, out start.I, out start.J);
                GridManager.Instance.WorldToGridPosition(soldierController.GetTargetWorldPosition(), out target.I, out target.J);
                List<Vector3> newPath=autoPathFinder.FindPath(start, target);
                soldierController.SetPath(newPath);
            }
        }
    }
}
