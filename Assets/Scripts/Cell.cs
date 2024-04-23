using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Entity
{
    SAFE,OBSTACLE,BUILDING,CHARACTER
}

public class PathNode
{
    private int X;
    private int Y;
    public PathNode parent;

    public int gCost;
    public int hCost;
    public PathNode(int X,int Y)
    {
        this.X= X; this.Y = Y;
    }
    public int fCost
    {
        get { return gCost + hCost; }
    }
    public void GetXY(out int X,out int Y)
    {
        X = this.X;
        Y = this.Y;
    }
}
public class Cell
{
    public Entity entity = Entity.SAFE;
    public int X;
    public int Y;
    public PathNode pathNode;
    public Cell(int X,int Y)
    {
        this.X = X;
        this.Y = Y;
        pathNode = new PathNode(X,Y);
    }
    
    public PathNode GetPathNode()
    {
        return pathNode;
    }
    public override string ToString()
    {
        return entity.ToString();
    }
}
