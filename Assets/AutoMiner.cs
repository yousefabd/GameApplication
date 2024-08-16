using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AutoMiner : MonoBehaviour
{
    public ResourceType buildingResource;
    private Entity resourceEntity;
    public bool CanBuild(out Entity rresource)
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        Vector2 colliderSize = boxCollider.size;
        float width = colliderSize.x;
        float height = colliderSize.y;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,(float) 0.3 * width);
        foreach (Collider2D collider in colliders)
        {
            // Ignore the collider of this building
            if (collider.gameObject == gameObject) continue;


            if (collider.gameObject.TryGetComponent<Entity>(out Entity resource))
            {
                Debug.Log(resource.resourceType);
                // Compare the resource type
                if (resource.resourceType == buildingResource)
                {
                    resourceEntity = resource;
                    Debug.Log("miner collision: " + collider);
                    rresource = resourceEntity;
                    return true; // Found a matching resource
                }
            }
        }
        rresource = null;
        return false; // No matching resource found
    }

    private void Start()
    {
        if(TryGetComponent<Building>(out Building building))
        {
            if(building.buildingSO.buildingType == BuildingType.ResourceGenerator && building.resource != null)
            {
              if(building.resource is IRecourses)
                {
                    StartCoroutine(ProcessResourceAfterDelay(building.resource));
                }
            }
        }
    }
    private IEnumerator ProcessResourceAfterDelay(Entity resource)
    {
        while (!(resource.gameObject.IsDestroyed()))
        {
            switch (resource)
            {
                case Gold:
                    resource.Damage(resource.gameObject.transform.position, 500);
                    break;
                case Stone:
                    resource.Damage(resource.gameObject.transform.position, 5);
                    break;
                case Wood:
                    resource.Damage(resource.gameObject.transform.position, 20);
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(1f);

        }

    }





}
