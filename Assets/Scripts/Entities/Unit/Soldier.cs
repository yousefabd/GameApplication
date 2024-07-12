using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SoldierType { SWORDSMAN,RANGER}
public class Soldier : Unit
{
    private Entity currentTargetEnemy;
    private float attackRadius;
    private float attackDamage;
    [SerializeField] private ProjectileSO projectileSO;
    private System.Random random;
    protected SoldierAI soldierAI;
    private float attackCoolDown;
    private SoldierType soldierType;
    //events
    public event Action<Vector3> OnNormalAttack;
    public event Action OnStartAttacking;
    public event Action OnRangedAttack;
    protected override void Awake()
    {
        base.Awake();
        base.OnTakeAction += Soldier_OnTakeAction;
        attackRadius = base.unitSO.interactionRadius;
        attackDamage = unitSO.attackPower;
        attackCoolDown = unitSO.attackCooldown;
        soldierType = unitSO.soldierType;
        random = new System.Random();
    }
    protected override void Start()
    {
        base.Start();
        Player.Instance.OnSetTarget += Player_OnSetTarget;
        Player.Instance.OnClearTarget += Player_OnClearTarget;
        soldierAI = GetComponent<SoldierAI>(); 
        switch (soldierType)
        {
            case SoldierType.SWORDSMAN:
                soldierAI.OnAttack += NormalAttack;
                break;
            case SoldierType.RANGER:
                soldierAI.OnAttack += RangedAttack;
                break;
        }
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
        SetTarget(target);
    }
    public void SetTarget(Entity target)
    {
        currentTargetEnemy = target;
        Debug.Log("target enemy has been set");
    }
    private void NormalAttack()
    {
        ToIdle();
        Attack();
    }
    public void RangedAttack()
    {
        WeaponProjectile weaponProjectile = WeaponProjectile.Throw(transform.position, projectileSO.prefab, GetTargetWorldPosition());
        OnRangedAttack?.Invoke();
    }
    private void Soldier_OnTakeAction()
    {
        if(currentTargetEnemy!=null)
            LookForTargets(currentTargetEnemy);
    }
    public bool LookForTargets(Entity wantedTarget = null)
    {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        Debug.Log("looking for enemies");
        Entity closestEntity=null;
        float closestRange = float.MaxValue;
        foreach (Collider2D collider in collider2DArray)
        {
            Entity hitEntity = collider.GetComponent<Entity>();
            if (hitEntity == this || (hitEntity is not Unit && hitEntity is not Building))
            {
                continue;
            }
            if (wantedTarget != null)
            {
                if ((wantedTarget.Equals(hitEntity)) && wantedTarget.GetTeam() != this.GetTeam())
                {
                    OnStartAttacking?.Invoke();
                    return true;
                }
            }
            else
            {
                if(hitEntity.GetTeam() != this.GetTeam())
                {
                    if (Vector3.Distance(hitEntity.transform.position, transform.position) < closestRange)
                    {
                        closestRange = Vector3.Distance(hitEntity.transform.position, transform.position);
                        closestEntity = hitEntity;
                    }
                }
            }
        }
        if (closestEntity != null) 
        {
            SetTarget(closestEntity);
            return true;
        }
        return false;
    }
    public virtual void Attack()
    {    
        OnNormalAttack?.Invoke(Vector2.up);
        float luckyPoints = (float) (random.NextDouble()*(attackDamage/4f));
        Player.Instance.OnAttackCallback(currentTargetEnemy.transform.position, attackDamage+luckyPoints);
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
    public float GetAttackCoolDown()
    {
        return attackCoolDown;
    }

    public SoldierType GetSoldierType()
    {
        return soldierType;
    }
}
