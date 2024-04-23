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
        HashSet<PathNode> visitedCells = new HashSet<PathNode>();
        PathNode startNode = start.GetPathNode();
        PathNode targetNode = target.GetPathNode();
        openSet.Add(startNode);
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
            visitedCells.Add(currentNode);
            
            //if we reached our target
            
            if(currentNode == targetNode)
            {
                Debug.Log("foundPath");
                return RetracePath(targetNode); ;
            }
            //else traverse to the neighbors of the chosen node
            int[] xMove = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] yMove = { -1, 0, 1, -1, 1, -1, 0, 1 };
            
            for(int i = 0; i < 8; i++)
            {
                
                currentNode.GetXY(out int X, out int Y);
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
                PathNode neighbor = gridMap.GetValue(X,Y).GetPathNode();
                if (gridMap.GetValue(X, Y).entity != Entity.SAFE || visitedCells.Contains(neighbor))
                {
                    continue;
                }
                int newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                
                if(newCostToNeighbor<neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, target.GetPathNode());
                    
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
            currentNode.GetXY(out int currentX,out int currentY);
            currentNode.parent.GetXY(out int parentX, out int parentY);
            int xDir= currentX - parentX;
            int yDir = currentY-parentY;
            path.Add(new Vector3(xDir, yDir));
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }
    int GetDistance(PathNode nodeA,PathNode nodeB)
    {
        nodeA.GetXY(out int xA, out int yA);
        nodeB.GetXY(out int xB, out int yB);

        int dstX = Mathf.Abs(xA - xB);
        int dstY = Mathf.Abs(yA - yB);
        int min = Mathf.Min(dstX, dstY);
        int max = Mathf.Max(dstX, dstY);
        return diagonal_move*min + symmetric_move*(max-min);
    }
}
