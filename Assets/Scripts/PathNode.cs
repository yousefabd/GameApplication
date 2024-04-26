using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public int X, Y;
    public PathNode parent;

    public int gCost;
    public int hCost;
    public PathNode(int x, int y)
    {
        X = x;
        Y = y;
    }
    public int fCost
    {
        get { return gCost + hCost; }
    }
}
