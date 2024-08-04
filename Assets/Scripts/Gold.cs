using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : Entity, IDestructibleObject, IRecourses
{
    public GameObject prefab;
    public float HealthPoints { get; set; }
    public event Action<float> OnDamaged;
    public event Action OnDestroyed;
    static string Team = null;

    public void Initialize(Vector3Int cellPosition, float size)
    {
        // Set the size of the stone object
        transform.localScale = new Vector3(size, size, 1);

        // Set the position of the stone object based on the cell position
        transform.position = new Vector3(cellPosition.x, cellPosition.y, 0);

        foreach (Transform child in transform)
        {
            child.localPosition = Vector3.zero;
        }
        if (size == 1f)
        {
            HealthPoints = 5f;
           // boxCollider.size = new Vector2(1f,1f); 
        }
        else if (size == 2f)
        {
            HealthPoints = 10f;
            //boxCollider.size = new Vector2(1f, 1f);
        }
        else if (size == 3f)
        {
            HealthPoints = 25f;
            //boxCollider.size = new Vector2(1.422243f, 1.4886762f);
        }
    }
    public override Entity Spawn(Vector3 position)
    {
        GameObject instance = Instantiate(prefab, position, Quaternion.identity);
        var entity = instance.GetComponent<Entity>();
        return entity;
    }

    public void Damage(Vector3 position, float value)
    {
        HealthPoints -= value;
        OnDamaged?.Invoke(value);
        if (HealthPoints <= 0)
        {
            Destruct();
        }
    }

    public void Destruct()
    {
        Destroy(gameObject);
    }
}
