using System;
using UnityEngine;

public class Soldier : Unit
{
    private Entity currentTargetEnemy;
    private float attackRadius;
    private float attackDamage;
    private System.Random random;
    private SoldierAI soldierAI;
    public event Action<Vector3> OnSoldierAttack;
    public event Action OnStartAttacking;
    public float attackCoolDown = 1f;
    protected override void Awake()
    {
        base.Awake();
        base.OnTakeAction += Soldier_OnTakeAction;
        attackRadius = base.unitSO.interactionRadius;
        attackDamage = unitSO.attackPower;
        random = new System.Random();
    }
    protected override void Start()
    {
        base.Start();
        Player.Instance.OnSetTarget += Player_OnSetTarget;
        Player.Instance.OnClearTarget += Player_OnClearTarget;
        soldierAI = GetComponent<SoldierAI>();
        soldierAI.OnAttack += Attack;
    }

    private void Player_OnClearTarget()
    {
        if (IsSelected())
        {
            currentTargetEnemy = null;
        }
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
                    OnStartAttacking?.Invoke();
                }
            }

        }
    }

    public void Attack()
    {
        ToIdle();
        OnSoldierAttack?.Invoke(Vector2.up);
        float luckyPoints = (float)(random.NextDouble() * (attackDamage / 4f));
        Player.Instance.OnAttackCallback(currentTargetEnemy.transform.position, attackDamage + luckyPoints);
    }

    public bool CanAttack(Vector3 standingPosition)
    {
        return Vector3.Distance(currentTargetEnemy.transform.position, standingPosition) <= attackRadius;
    }

    public bool HasTarget()
    {
        return currentTargetEnemy != null;
    }
    public Vector3 GetTargetWorldPosition()
    {
        return currentTargetEnemy.transform.position;
    }
}
