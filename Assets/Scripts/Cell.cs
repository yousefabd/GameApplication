using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EntityEnum
{
    SAFE,OBSTACLE,BUILDING,CHARACTER
}
public class Cell 
{
    private Entity entity = null;
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
            SetEntity(character);
            return character;
        }
    }
    public void ClearCharacter()
    {
        entity = null;
        character = null;
    }
    public void SetCharacter(Character character)
    {
        if (character != null)
        {
            SetEntity(character);
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
    public string EntityString()
    {
        if (entity != null)
        {
            return entity.ToString(); // Assuming Entity has a meaningful ToString() method
        }
        else
        {
            return "safe"; // Or any other string representing a safe cell
        }
    }

     public override String ToString()
    {
        return GetEntity().ToSafeString();
    }

}
