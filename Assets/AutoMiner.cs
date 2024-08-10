using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMiner : MonoBehaviour
{
    public ResourceType buildingResource;
    private Entity resourceEntity;
    public bool CanBuild()
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
                    return true; // Found a matching resource
                }
            }
        }

        return false; // No matching resource found
    }

    private void Start()
    {
        StartCoroutine(UpdateResourceCoroutine());
    }

    private IEnumerator UpdateResourceCoroutine()
    {
        while (true)
        {
            if (GetComponent<Building>().GetBuildingState() == Building.BuildingState.BUILT)
            {
                ResourceManager.Instance.updateResource(GetComponent<Building>().buildingSO.resourceType, 1);
            }
            yield return new WaitForSeconds(1);
        }
    }





}
