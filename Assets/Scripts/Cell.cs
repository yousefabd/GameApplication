using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;

public enum EntityEnum
{
    SAFE,OBSTACLE,BUILDING,CHARACTER
}
public class Cell 
{
    private Entity entity = null;
    
    private readonly Indices indices;

    public Cell(int I,int J)
    {
        indices.I= I;
        indices.J= J;
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
        this.entity=entity;
        GridManager.Instance.UpdateValues();

    }
    //this method is temporarily here, it should exist in the building class
    public Character SpawnCharacter(CharacterSO characterSO,Vector3 position)
    {
        if(entity != null)
        {
            Debug.LogError("A character in the cell ("+indices.I+", "+indices.J+")" + " already exists!");
            return entity as Character;
        }
        else
        {
            entity = Character.Spawn(characterSO, position);
            SetEntity(entity);
            return entity as Character;
        }
    }
    public void ClearEntity()
    {
        entity = null;
    }
    public void GetIndices(out int I,out int J)
    {
        I=indices.I;
        J=indices.J;
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
