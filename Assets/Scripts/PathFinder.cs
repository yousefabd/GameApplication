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
    public List<Vector3> FindPath(Indices start, Indices target)
    {
        //cache the gridMap object
        Grid<Cell> gridMap = GameManager.Instance.gridMap;

        //a set of all the possible branches of the path
        List<PathNode> openSet = new List<PathNode>();
        //a set of already visited nodes
        HashSet<PathNode> closedSet = new HashSet<PathNode>();
        PathNode[,] pathGrid = new PathNode[gridMap.GetWidth(), gridMap.GetHeight()];
        for(int i = 0; i < gridMap.GetWidth(); i++)
        {
            for(int j = 0; j < gridMap.GetHeight(); j++)
            {
                pathGrid[i, j] = new PathNode(-1, -1);
            }
        }
        pathGrid[start.I,start.J]= new PathNode(start.I, start.J);
        pathGrid[target.I,target.J]=new PathNode(target.I, target.J);
        openSet.Add(pathGrid[start.I, start.J]);
        PathNode closestNode = openSet[0];
        closestNode.hCost = int.MaxValue;
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
            if(closestNode.hCost > currentNode.hCost)
            {
                closestNode = currentNode;
            }
            //mark the chosen node as visited
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            //if we reached our target
            if(currentNode == pathGrid[target.I,target.J])
            {
                return RetracePath(currentNode); 
            }
            //else traverse to the neighbors of the chosen node
            int[] xMove = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] yMove = { -1, 0, 1, -1, 1, -1, 0, 1 };
            
            for(int i = 0; i < 8; i++)
            {

                currentNode.GetIndices(out int X, out int Y);
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
                PathNode neighbor = pathGrid[X,Y];
                neighbor.SetIndices(X, Y);
                if (gridMap.GetValue(X, Y).GetEntity() != null || closedSet.Contains(neighbor))
                {
                    continue;
                }
                int newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                
                if(newCostToNeighbor<neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, pathGrid[target.I,target.J]);
                    
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                    
                }
            }
        }
        //in case no path has been found, retrace the path of the closest node to the target
        return RetracePath(closestNode);
    }
    public List<Indices> FindMultipleTargets(Indices originalTarget,int characterCount)
    {
        int gridWidth = GameManager.Instance.GetWidth();
        int gridHeight = GameManager.Instance.GetHeight();
        List<Indices> targets = new List<Indices>();
        targets.Add(originalTarget);
        characterCount--;
        bool[,] visited = new bool[gridWidth, gridHeight];
        visited[originalTarget.I, originalTarget.J] = true;
        Queue<Indices>queue = new Queue<Indices>();
        queue.Enqueue(originalTarget);
        while(queue.Count>0){
            Indices current = queue.Dequeue();
            int[] xMove = { -1, 1, 0, 0, -1, -1, 1, 1 };
            int[] yMove = { 0, 0, -1, 1, -1, 1, -1, 1 };
            for(int i = 0; i < 8; i++)
            {
                if (characterCount <= 0)
                {
                    return targets;
                }
                int newX = current.I + xMove[i];
                int newY = current.J + yMove[i];
                if(newX>=0 && newX<gridWidth && newY>=0 && newY < gridHeight)
                {
                    if (!visited[newX, newY] && GameManager.Instance.GetEntity(newX,newY) == null) 
                    {
                        visited[newX,newY]= true;
                        Indices newTarget = new Indices(newX,newY);
                        queue.Enqueue(newTarget);
                        targets.Add(newTarget);
                        characterCount--;
                    }
                }
            }
        }

        return targets;
    }
    List<Vector3> RetracePath(PathNode target)
    {
        List<Vector3> path=new List<Vector3>();
        PathNode currentNode = target;
        while (currentNode.parent != null) {
            currentNode.GetIndices(out int X, out int Y);
            path.Add(GameManager.Instance.gridMap.GetWorldPositionCentered(X,Y));
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }
    int GetDistance(PathNode nodeA,PathNode nodeB)
    {
        nodeA.GetIndices(out int xA, out int yA);
        nodeB.GetIndices(out int xB,out int yB);

        int dstX = Mathf.Abs(xA - xB);
        int dstY = Mathf.Abs(yA - yB);
        int min = Mathf.Min(dstX, dstY);
        int max = Mathf.Max(dstX, dstY);
        return diagonal_move*min + symmetric_move*(max-min);
    }
}
