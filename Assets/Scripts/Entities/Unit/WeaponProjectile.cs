using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum ProjectileType { SINGLE_TARGET, MULTI_TARGET, INCREASE_BY_DISTANCE}
public class WeaponProjectile : MonoBehaviour
{
    [SerializeField] private ProjectileSO projectileSO;
    float moveSpeed;
    float attackDamage;
    private float timePassed = 0f;
    static float yOffset = 0.5f;
    private float effectRadius;
    public ProjectileType projectileType;
    private Team team;
    Vector3 targetPosition;
    Entity target;
    System.Random random;
    private void Awake()
    {
        moveSpeed = projectileSO.speed;
        attackDamage = projectileSO.damage;
        effectRadius = projectileSO.effectRadius;
        projectileType = projectileSO.projectileType;
        random = new System.Random();
        
    }
    private void Update()
    {
        timePassed += Time.deltaTime;
        if (Vector3.Distance(transform.position,targetPosition)<=0.05f)
        {
            switch (projectileType)
            {
                case ProjectileType.SINGLE_TARGET:
                    Attack();
                    break;
                case ProjectileType.MULTI_TARGET:
                    MultiAttack();
                    break;
                case ProjectileType.INCREASE_BY_DISTANCE:
                    IncreaseByDistanceAttack();
                    break;
            }
        }
        Vector3 moveDir = targetPosition - transform.position;
        transform.position = Vector3.MoveTowards(transform.position,targetPosition,moveSpeed*Time.deltaTime);
    }
    public static WeaponProjectile Throw(Vector3 originPosition, Transform prefab,Entity target,Team team)
    {
        if (target == null)
        {
            return null;
        }
        Vector3 offset = new Vector3(0f, yOffset, 0f);
        Transform projectileTransform = Instantiate(prefab, originPosition+offset, Quaternion.identity);
        WeaponProjectile projectile = projectileTransform.GetComponent<WeaponProjectile>();
        projectile.targetPosition = target.transform.position+offset;
        projectile.target = target;
        projectile.RotateToTarget();
        projectile.team = team;
        return projectile;
    }
    public static WeaponProjectile SmartThrow(Vector3 originPosition, Transform prefab, Entity target, Team team)
    {
        if (target == null)
        {
            return null;
        }
        else if (target is not Unit)
        {
            return Throw(originPosition, prefab, target, team);
        }
        Vector3 offset = new Vector3(0f, yOffset, 0f);
        Transform projectileTransform = Instantiate(prefab, originPosition + offset, Quaternion.identity);
        WeaponProjectile projectile = projectileTransform.GetComponent<WeaponProjectile>();
        projectile.targetPosition =
            CalculateInterceptionPoint(originPosition,
                                       target.transform.position + offset,
                                       projectile.projectileSO.speed,
                                       (target as Unit).GetMoveSpeed(),
                                       (target as Unit).GetMoveDir());
        projectile.target = target;
        projectile.RotateToTarget();
        projectile.team = team;
        return projectile;
    }
    private static Vector3 CalculateInterceptionPoint(Vector3 originPosition,Vector3 enemyPosition,float arrowSpeed,float enemySpeed,Vector3 enemyMoveDir)
    {

        Vector3 left = enemyPosition;
        Vector3 right = enemyPosition + enemyMoveDir * 10;
        Vector3 interceptionPoint = enemyPosition;
        float projectileTime = 1000, enemyTime = 0;
        int precision = 36;
        while (precision>0)
        {
            Vector3 m = (left + right) / 2;
            float p_t = Vector3.Distance(originPosition, m) / arrowSpeed;
            float e_t = Vector3.Distance(enemyPosition, m) / enemySpeed;
            if (p_t < e_t)
            {
                projectileTime = p_t;
                enemyTime = e_t;
                right = m;
            }
            else if (p_t > e_t)
            {
                projectileTime = p_t;
                enemyTime = e_t;
                left = m;
            }
            else
            {
                projectileTime = p_t;
                enemyTime = e_t;
                interceptionPoint = m;
                break;
            }
            interceptionPoint = m;
            precision--;
        }
        return interceptionPoint;
    }
    public void Attack()
    {
        Vector3 offset = new Vector3(0f, yOffset, 0f);
        float luckyPoints = (float)(random.NextDouble() * (attackDamage / 4f));
        Collider2D collider = Physics2D.OverlapCircle(transform.position-offset,effectRadius);
        if (collider != null)
        {
            if (collider.TryGetComponent(out Entity targetEnemy))
            {
                if (targetEnemy.Equals(target))
                {
                    (targetEnemy as IDestructibleObject).Damage(targetEnemy.transform.position, attackDamage + luckyPoints);
                    Destroy(gameObject);     
                }
            }
        }
        else if (Vector3.Distance(transform.position, targetPosition) <= 0.05f)
        {
            Destroy(gameObject);
        }
    }
    public void MultiAttack()
    {
        Vector3 offset = new Vector3(0f, yOffset, 0f);
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position - offset, effectRadius);
        bool reachedTarget = false;
        if (colliderArray != null)
        {
            foreach (Collider2D collider in colliderArray)
            {
                if (collider.TryGetComponent(out Entity targetEnemy))
                {
                    if (targetEnemy.GetTeam()!=team && (targetEnemy is Unit || targetEnemy is Building))
                    {
                        float luckyPoints = (float)(random.NextDouble() * (attackDamage / 4f));
                        (targetEnemy as IDestructibleObject).Damage(targetEnemy.transform.position, attackDamage + luckyPoints);
                        reachedTarget = true;
                    }
                }
            }
        }
        if(reachedTarget || Vector3.Distance(transform.position, targetPosition) <= 0.05f)
            Destroy(gameObject);
    }

    private void IncreaseByDistanceAttack()
    {
        attackDamage *= timePassed*60f;
        Attack();
    }
    public void RotateToTarget()
    {
        float offset = 90f;
        Vector3 moveDir = (targetPosition - transform.position).normalized;
        transform.eulerAngles=new Vector3(0f,0f,GetRotationAngle(moveDir)-offset);
    }
    public float GetRotationAngle(Vector3 moveDir)
    {
        float radians = Mathf.Atan2(moveDir.y, moveDir.x);
        float degrees = radians * Mathf.Rad2Deg;
        return degrees;
    }
    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }
}
