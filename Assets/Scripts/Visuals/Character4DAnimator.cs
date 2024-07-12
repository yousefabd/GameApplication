using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using UnityEngine;

public class Character4DAnimator : MonoBehaviour
{
    private Character4D character4D;
    private AnimationManager animationManager;
    private Vector2[] initialSpawnPosition = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    [SerializeField] private Unit unit;
    [SerializeField] Transform damagePopupTransform;
    private Vector2 lastDir;
    private System.Random random;
    private void Awake()
    {
        unit.OnSpawn += Unit_OnSpawn;
        animationManager = GetComponent<AnimationManager>();
        character4D = GetComponent<Character4D>();
        random = new System.Random();
    }
    private void Start()
    {
        unit.OnMoveCell += Unit_OnMoveCell;
        unit.OnDestroyed += Unit_OnDie;
        unit.OnDamaged += Unit_OnDamaged;
        if (unit is Soldier)
        {
            SoldierType soldierType = (unit as Soldier).GetSoldierType();
            switch (soldierType)
            {
                case SoldierType.SWORDSMAN:
                    (unit as Soldier).OnNormalAttack += Soldier_OnNormalAttack;
                    Debug.Log("swordsman visual");
                    break;
                case SoldierType.RANGER:
                    (unit as Soldier).OnRangedAttack += Soldier_OnRangedAttack;
                    break;

            }

        }
    }
    private void Soldier_OnRangedAttack()
    {
        animationManager.ShotBow();
    }

    private void Soldier_OnNormalAttack(Vector3 direction)
    {
        int attackAnimation = random.Next(2);
        switch (attackAnimation)
        {
            case 0:
                animationManager.Slash(false);
                break;
            case 1:
                animationManager.Jab();
                break;
        }

    }
    private void Unit_OnDamaged(float value)
    {
        animationManager.Hit();
        DamagePopup.Create(transform.parent.position + new Vector3(0f, 1f), damagePopupTransform, (int)value);
    }

    private void Unit_OnDie()
    {
        animationManager.SetState(Assets.HeroEditor4D.Common.Scripts.Enums.CharacterState.Death);
    }

    private void Unit_OnSpawn()
    {
        lastDir = initialSpawnPosition[random.Next(initialSpawnPosition.Length)];
        character4D.SetDirection(lastDir);
    }

    private void Unit_OnMoveCell(Vector3 moveDir)
    {
        moveDir = moveDir.normalized;
        Vector2 direction = Vector2.up;
        if (moveDir.x >= 0.65f)
        {
            direction.x = 1.0f;
            direction.y = 0.0f;
        }
        else if (moveDir.x <= -0.65f)
        {
            direction.x = -1.0f;
            direction.y = 0.0f;
        }
        else if (moveDir.y >= 0.65f)
        {
            direction.x = 0.0f;
            direction.y = 1.0f;
        }
        else if (moveDir.y <= -0.65f)
        {
            direction.x = 0.0f;
            direction.y = -1.0f;
        }
        else
        {
            direction = lastDir;
        }
        character4D.SetDirection(direction);
        lastDir = direction;
        if (moveDir == Vector3.zero)
        {
            animationManager.SetState(Assets.HeroEditor4D.Common.Scripts.Enums.CharacterState.Idle);
        }
        else
        {
            animationManager.SetState(Assets.HeroEditor4D.Common.Scripts.Enums.CharacterState.Walk);
        }
    }
}
