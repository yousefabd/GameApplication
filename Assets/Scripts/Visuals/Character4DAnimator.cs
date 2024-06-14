using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Character4DAnimator : MonoBehaviour
{
    private Character4D character4D;
    private AnimationManager animationManager;
    private Vector2[] initialSpawnPosition = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    [SerializeField] private Unit unit;
    private Vector2 lastDir;
    private System.Random random;
    private void Awake()
    {
        character4D = GetComponent<Character4D>();
        animationManager = GetComponent<AnimationManager>();
        unit.OnMoveCell += Unit_OnMoveCell;
        unit.OnSpawn += Unit_OnSpawn;
        unit.OnDie += Unit_OnDie;
        unit.OnDamaged += Unit_OnDamaged;
        random = new System.Random();
    }

    private void Unit_OnDamaged(float value)
    {
        animationManager.Hit();
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
        if(moveDir.x>=0.65f)
        {
            direction.x = 1.0f;
            direction.y = 0.0f;
        }
        else if (moveDir.x <= -0.65f)
        {
            direction.x = -1.0f;
            direction.y = 0.0f;
        }
        else if(moveDir.y>=0.65f)
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
