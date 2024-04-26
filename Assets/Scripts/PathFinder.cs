using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PathFinder
{
    private const int diagonal_move = 14;
    private const int symmetric_move = 10;
    public List<Vector3> FindPath(Cell start, Cell target)
    {
        
        Grid<Cell> gridMap = GameManager.Instance.gridMap;

        //a list of all the possible branches of the path
        List<PathNode> openSet = new List<PathNode>();
        HashSet<PathNode> closedSet = new HashSet<PathNode>();
        start.GetXY(out int startX, out int startY);
        target.GetXY(out int targetX,out int targetY);
        PathNode[,] pathGrid = new PathNode[gridMap.GetWidth(), gridMap.GetHeight()];
        for(int i = 0; i < gridMap.GetWidth(); i++)
        {
            for(int j = 0; j < gridMap.GetHeight(); j++)
            {
                pathGrid[i, j] = new PathNode(-1, -1);
            }
        }
        pathGrid[startX,startY]= new PathNode(startX, startY);
        pathGrid[targetX,targetY]=new PathNode(targetX, targetY);
        openSet.Add(pathGrid[startX, startY]);
        //while there are still nodes that we can visit
        while (openSet.Count > 0)
        {
            PathNode currentNode = openSet[0];
            //choose the node with the lowest f cost 
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || 
                    openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost
                ){
                    currentNode = openSet[i];
                }
            }
            //mark the chosen node as visited
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            
            //if we reached our target
            
            if(currentNode == pathGrid[targetX,targetY])
            {
                return RetracePath(currentNode); ;
            }
            //else traverse to the neighbors of the chosen node
            int[] xMove = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] yMove = { -1, 0, 1, -1, 1, -1, 0, 1 };
            
            for(int i = 0; i < 8; i++)
            {
                
                int X=currentNode.X;
                int Y=currentNode.Y;
                X += xMove[i];
                Y += yMove[i];
                bool safe = 
                    X  >=0 && 
                    Y  >=0 && 
                    X  < gridMap.GetWidth() && 
                    Y  < gridMap.GetHeight();
                if (!safe)
                {
                    continue;
                }
                PathNode neighbor = pathGrid[X,Y] = new PathNode(X,Y);
                if (gridMap.GetValue(X, Y).GetEntity() != Entity.SAFE || closedSet.Contains(neighbor))
                {
                    continue;
                }
                int newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                
                if(newCostToNeighbor<neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, pathGrid[targetX,targetY]);
                    
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                    
                }
            }
        }
        return new List<Vector3>();
    }
    List<Vector3> RetracePath(PathNode target)
    {
        List<Vector3> path=new List<Vector3>();
        PathNode currentNode = target;
        while (currentNode.parent != null) {

            path.Add(GameManager.Instance.gridMap.GetWorldPositionCentered(currentNode.X,currentNode.Y));
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }
    int GetDistance(PathNode nodeA,PathNode nodeB)
    {
        int xA = nodeA.X;
        int yA = nodeA.Y;
        int xB = nodeB.X;
        int yB = nodeB.Y;

        int dstX = Mathf.Abs(xA - xB);
        int dstY = Mathf.Abs(yA - yB);
        int min = Mathf.Min(dstX, dstY);
        int max = Mathf.Max(dstX, dstY);
        return diagonal_move*min + symmetric_move*(max-min);
    }
}
