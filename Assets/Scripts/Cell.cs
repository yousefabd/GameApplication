using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Entity
{
    SAFE,OBSTACLE,BUILDING,CHARACTER
}
public class Cell
{
    private Entity entity = Entity.SAFE;
    private Character character;
    
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
    public bool TryGetCharacter(out Character character)
    {
        character = this.character;
        return this.character != null;
    }
    public Character SpawnCharacter(CharacterSO characterSO,Vector3 position)
    {
        if(character != null)
        {
            Debug.LogError("A character in the cell ("+indices.I+", "+indices.J+")" + " already exists!");
            return character;
        }
        else
        {
            character = Character.Spawn(characterSO, position);
            entity=Entity.CHARACTER;
            return character;
        }
    }
    public void ClearCharacter()
    {
        entity = Entity.SAFE;
        character = null;
    }
    public void SetCharacter(Character character)
    {
        if (character != null)
        {
            entity = Entity.CHARACTER;
        }
        this.character = character;
    }
    public void GetIndices(out int I,out int J)
    {
        I=indices.I;
        J=indices.J;
    }
    public Indices GetIndices()
    {
        return indices; 
    }
    public override string ToString()
    {
        return entity.ToString();
    }
}
