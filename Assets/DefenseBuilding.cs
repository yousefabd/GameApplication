using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class DefenseBuilding : MonoBehaviour
{
    [SerializeField] Transform shootingTransform;
    [SerializeField] ProjectileSO projectileSO;
    private Team team;

    private void Start()
    {
        TryGetComponent<Building>(out Building building);
        team = building.GetTeam();
        BuildingManager.Instance.built += Building_built;

    }

    private void Building_built(Building obj)
    {
        Debug.Log("works");
    }

    private void Update()
    {
        
                  WeaponProjectile.Throw(shootingTransform.position, projectileSO.prefab, SearchForEnemy(), Team.HUMANS);
        
        }
    private Entity SearchForEnemy()
    {
        Collider2D[] Colliders = Physics2D.OverlapCircleAll(transform.position, 4 * GridManager.Instance.GetCellSize());
        Unit closestUnit = null;
        Vector3 closestTarget = Vector3.positiveInfinity;
        foreach (Collider2D col in Colliders)
        {
            if (col.gameObject.TryGetComponent<Unit>(out Unit entity))
            {
                float currentDistance = Vector3.Distance(transform.position, col.transform.position);
                if (currentDistance < Vector3.Distance(transform.position, closestTarget))
                {
                    closestUnit = entity;
                    closestTarget = col.transform.position;
                }
            }
        }
        Debug.Log(closestUnit);
        return closestUnit;
    }

    


}
