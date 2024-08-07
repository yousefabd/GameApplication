using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMiner : MonoBehaviour
{
    public bool CanBuild()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        Vector2 colliderSize = boxCollider.size;
        float width = colliderSize.x;
        float height = colliderSize.y;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,(float) 1.5 * width);
        foreach (Collider2D collider in colliders)
        {
            // Ignore the collider of this building
            if (collider.gameObject == gameObject) continue;


            if (collider.gameObject.TryGetComponent<Entity>(out Entity resource))
            {
                // Compare the resource type
                if (resource.resourceType == GetComponent<Building>().buildingSO.resourceType)
                {
                    Debug.Log("miner collision: " + collider);
                    Debug.Log("buildingso resource: " + GetComponent<Building>().buildingSO.resourceType);
                    return true; // Found a matching resource
                }
            }
        }

        return false; // No matching resource found
    }





}
