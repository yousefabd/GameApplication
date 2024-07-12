using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponProjectile : MonoBehaviour
{
    [SerializeField] private ProjectileSO projectileSO;
    float moveSpeed;
    float attackDamage;
    static float yOffset = 0.5f;
    Vector3 targetPosition;
    System.Random random;
    private void Awake()
    {
        moveSpeed = projectileSO.speed;
        attackDamage = projectileSO.damage;
        random = new System.Random();
    }
    private void Update()
    {
        if (Vector3.Distance(transform.position,targetPosition)<=0.05f)
        {
            Attack();
        }
        Vector3 moveDir = targetPosition - transform.position;
        transform.position = Vector3.MoveTowards(transform.position,targetPosition,moveSpeed*Time.deltaTime);
    }
    public static WeaponProjectile Throw(Vector3 originPosition, Transform prefab,Vector3 targetPosition)
    {
        Vector3 offset = new Vector3(0f, yOffset, 0f);
        Transform projectileTransform = Instantiate(prefab, originPosition+offset, Quaternion.identity);
        WeaponProjectile projectile = projectileTransform.GetComponent<WeaponProjectile>();
        projectile.targetPosition = targetPosition;
        projectile.RotateToTarget();
        return projectile;
    }
    public void Attack()
    {
        float luckyPoints = (float)(random.NextDouble() * (attackDamage / 4f));
        Player.Instance.OnAttackCallback(targetPosition, attackDamage + luckyPoints);
        Destroy(gameObject);
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
}
