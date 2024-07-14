using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
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
    public event Action<Vector3, float> OnAttack;
    public event Action OnClearTarget;
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
        if (Player.Instance != null)
        {
            Player.Instance.OnSetTarget += Player_OnSetTarget;
            Player.Instance.OnClearTarget += Player_OnClearTarget;
        }
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
            OnClearTarget?.Invoke();
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
    }
    private void NormalAttack()
    {
        ToIdle();
        Attack();
    }
    public void RangedAttack()
    {
        ToIdle();
        WeaponProjectile weaponProjectile = WeaponProjectile.Throw(transform.position, projectileSO.prefab, GetTargetWorldPosition());
        OnRangedAttack?.Invoke();
    }
    private void Soldier_OnTakeAction()
    {
        if (currentTargetEnemy != null)
        {
            if (LookForTargets(currentTargetEnemy))
                OnStartAttacking?.Invoke();
        }
    }
    public bool LookForTargets(Entity wantedTarget = null)
    {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, attackRadius);

        Entity closestEntity=null;
        float closestRange = float.MaxValue;
        foreach (Collider2D collider in collider2DArray)
        {
            Entity hitEntity = collider.GetComponent<Entity>();
            if (hitEntity == this || (hitEntity is not Unit && hitEntity is not Building && hitEntity is not TDCastle))
            {
                continue;
            }
            if (wantedTarget != null)
            {
                if ((wantedTarget.Equals(hitEntity)) && wantedTarget.GetTeam() != this.GetTeam())
                {
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
        OnAttack?.Invoke(currentTargetEnemy.transform.position, attackDamage + luckyPoints);
        if (Player.Instance != null)
        {
            Player.Instance.OnAttackCallback(currentTargetEnemy.transform.position, attackDamage + luckyPoints);
        }
    }

    public bool CanAttack(Vector3 standingPosition)
    {
        return LookForTargets(currentTargetEnemy);
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

    public void SetAttackDamage(float newAttackDamage)
    {
        attackDamage = newAttackDamage;
    }
}
