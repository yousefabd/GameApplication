using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : Entity, IDestructibleObject, IRecourses
{
    public GameObject prefab;
    public float HealthPoints { get; set; }
    public event Action<float> OnDamaged;
    private BoxCollider2D boxCollider;
    public event Action OnDestroyed;
    public ResourceType resourceType = ResourceType.GOLD;


    public void Initialize(Vector3Int cellPosition, float size)
    {

        transform.localScale = new Vector3(size, size, 1);

        transform.position = new Vector3(cellPosition.x, cellPosition.y, 0);

        foreach (Transform child in transform)
        {
            child.localPosition = Vector3.zero;
        }
        if (size == 1f)
        {
            HealthPoints = 5f;
        }
        else if (size == 2f)
        {
            HealthPoints = 10f;
        }
        else if (size == 3f)
        {
            HealthPoints = 25f;
        }
    }

  
    public override Entity Spawn(Vector3 position)
    {
        GameObject instance = Instantiate(prefab, position, Quaternion.identity);
        var entity = instance.GetComponent<Entity>();
        team = Team.HUMANS;

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
        //ResourceManager.Instance.updateResource("GOlD", 6);
        OnDestroyed?.Invoke();

        Vector3 worldPosition = transform.position;

        GridManager.Instance.WorldToGridPosition(worldPosition, out int x, out int y);

        Debug.Log(x);
        Debug.Log(y);

        GridManager.Instance.SetEntity(null, new Indices(x, y));
        GridManager.Instance.SetEntity(null, new Indices(x + 1, y));
        GridManager.Instance.SetEntity(null, new Indices(x - 1, y));
        GridManager.Instance.SetEntity(null, new Indices(x, y + 1));
        GridManager.Instance.SetEntity(null, new Indices(x, y - 1));
        GridManager.Instance.SetEntity(null, new Indices(x -1, y -1));
        Destroy(gameObject);
    }
}
