using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Entity
{
    SAFE,OBSTACLE,BUILDING,CHARACTER
}
public class Cell
{
<<<<<<< Updated upstream
    public Entity entity = Entity.SAFE;
    public int X;
    public int Y;
    public PathNode pathNode;
=======
    private Entity entity = Entity.SAFE;
    private Character character;
    private int X;
    private int Y;
>>>>>>> Stashed changes
    public Cell(int X,int Y)
    {
        this.X = X;
        this.Y = Y;
    }
<<<<<<< Updated upstream
    
    public PathNode GetPathNode()
    {
        return pathNode;
=======
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
    public void SpawnCharacter(CharacterSO characterSO,Vector3 position)
    {
        if(character != null)
        {
            Debug.LogError("A character in the cell ("+X+", "+Y+")" + " already exists!");
        }
        else
        {
            character = Character.Spawn(characterSO, position);
        }
    }
    public void GetXY(out int x, out int y)
    {
        x=this.X; y=this.Y;
>>>>>>> Stashed changes
    }
    public override string ToString()
    {
        return entity.ToString();
    }
}
