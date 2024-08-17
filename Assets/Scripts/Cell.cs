using System;
using UnityEngine;

public enum EntityEnum
{
    SAFE, OBSTACLE, BUILDING, UNIT
}
public class Cell
{
    private Entity entity = null;

    private readonly Indices indices;

    public Cell(int I, int J)
    {
        indices.I = I;
        indices.J = J;
    }

    public Entity GetEntity()
    {
        return entity;
    }
    public void SetEntity(Entity entity)
    {
        this.entity = entity;
    }
    public void UpdateEnity(Entity entity)
    {
        this.entity = entity;
        GridManager.Instance.UpdateValues();

    }
    public Unit SpawnUnit(UnitSO characterSO,Vector3 position)
    {
        if (entity != null)
        {
            Debug.LogError("A unit in the cell (" + indices.I + ", " + indices.J + ")" + " already exists!");
            return entity as Unit;
        }
        
        
            entity = Unit.Spawn(characterSO, position);
        if (entity.gameObject.TryGetComponent<SoldierAI>(out SoldierAI c))
            if (c.gameObject.tag.Equals("Goblin"))
                entity.gameObject.AddComponent<CharacterOpponentAI>();
            SetEntity(entity);
            return entity as Unit;
        
    }
    public void ClearEntity()
    {
        entity = null;
    }
    public void GetIndices(out int I, out int J)
    {
        I = indices.I;
        J = indices.J;
        return;
    }

    public Indices GetIndices()
    {
        return indices;
    }

    public override String ToString()
    {
        return GetEntity()?.GetType().Name;
    }

    public bool IsOccupied()
    {
        return entity != null;
    }

}
