using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private Indices indices;
    public PathNode parent;

    public int gCost;
    public int hCost;
    public PathNode(int i, int j)
    {
        indices.I = i;
        indices.J = j;
    }
    public int fCost
    {
        get { return gCost + hCost; }
    }
    public void SetIndices(int i,int j)
    {
        indices.I = i;
        indices.J = j;
    }
    public void GetIndices(out int i, out int j)
    {
        i = indices.I;
        j = indices.J;
    } 
}
