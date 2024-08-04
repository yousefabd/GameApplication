using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Entity, IDestructibleObject,IRecourses
{
    public GameObject prefab;
    public float HealthPoints { get; set; }
    public event Action<float> OnDamaged;
    public event Action OnDestroyed;


    protected Team team;

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
           // boxCollider.size = new Vector2(1.501378f, 2.130224f); 
        }
        else if (size == 2f)
        {
            HealthPoints = 10f;
            //boxCollider.size = new Vector2(3.002756f, 4.260448f);
        }
        else if (size == 3f)
        {
            HealthPoints = 25f;
          //  boxCollider.size = new Vector2(4.504134f, 6.390672f);
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
       //ResourceManager.Instance.updateResource("WOOD", 1);
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
        GridManager.Instance.SetEntity(null, new Indices(x + 1, y + 1));
        Debug.Log("true");
        Destroy(gameObject);
    }
   


}
