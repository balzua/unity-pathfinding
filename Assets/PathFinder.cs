using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    //Temporary
    private NodeGrid grid;

    public List<Node> FindRange(Vector3 startPosition, int range)
    {
        Node startingNode = grid.GetNodeFromWorldPosition(startPosition);
        List<Node> inRange = new List<Node>();
        Queue<Node> BFSQueue = new Queue<Node>();
        bool[,,] searched = new bool[grid.maxX, grid.maxY, grid.maxZ];
        BFSQueue.Enqueue(startingNode);
        while (BFSQueue.Count > 0)
        {
            Node current = BFSQueue.Dequeue();
            inRange.Add(current);
            List<Node> neighbors = grid.GetNodeNeighbors(current);
            foreach (Node neighbor in neighbors)
            {
                if (range - GetDistance(startingNode, neighbor) > 0 && !searched[neighbor.x, neighbor.y, neighbor.z])
                {
                    BFSQueue.Enqueue(neighbor);
                    searched[neighbor.x, neighbor.y, neighbor.z] = true;
                }
            }
        }
        return inRange;
    }

    public List<Node> FindPath(Vector3 startPosition, Vector3 endPosition)
    {
        Node start = grid.GetNodeFromWorldPosition(startPosition);
        Node target = grid.GetNodeFromWorldPosition(endPosition);

        // Later: On openset, we need to be able to find minimum quickly. Will replace this with a heap data structure later.
        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();

        // Add the starting node to the open set.
        openSet.Add(start);

        while (openSet.Count > 0)
        {
            Node current = openSet.GetMinimum(); // Change later, this line should actually be current = Minimum of openset.

            // Add the current node to the closed set. We do this, then consider all of this node's neighbors. 
            // After all neighbors are considered, the node is considered closed.
            closedSet.Add(current);
            if (current == target)
            {
                return RetracePath(start, target);
            }
            foreach (Node neighbor in grid.GetNodeNeighbors(current))
            {
                // If the neighbor is not walkable, or it has already been closed, skip to the next neighbor.
                if (!neighbor.isWalkable || closedSet.Contains(neighbor))
                {
                    continue;
                }
                // Compute the distance if the path is adjusted to contain this neighbor node.
                int newPathCost = current.gCost + GetDistance(current, neighbor);
                // If the new path to the neighbor node (i.e. it is already in the open set but we've arrived at it through a different way) is shorter than the previous
                // gCost we computed for it, OR if we haven't even considered this neighbor yet (it is not in the openSet), we want to update its cost.
                if (newPathCost < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    // Update the neighbor's fCost by setting its g and h costs.
                    neighbor.gCost = newPathCost;
                    neighbor.hCost = GetDistance(neighbor, target);
                    openSet.UpdateItem(neighbor);
                    // The "parentNode" field refers to which node led us to this one, i.e. the node that preceded this in the path
                    neighbor.parentNode = current;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }

        }

        return new List<Node>();
    } 

    public Node TargetNodeFromWorldPosition(Vector3 position)
    {
        return grid.GetNodeFromWorldPosition(position);
    }

    // Once the path has been found, this function retraces the path and generates a list of nodes along the way
    private List<Node> RetracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node current = end;
        while (current.parentNode != null)
        {
            path.Add(current);
            current = current.parentNode;
        }
        path.Reverse();
        return path;
    }

    // Get the distance between two arbitrary nodes
    private int GetDistance(Node a, Node b)
    {
        int distX = Mathf.Abs(a.x - b.x);
        int distY = Mathf.Abs(a.y - b.y);
        int distZ = Mathf.Abs(a.z - b.z);
        if (distX > distZ)
        {
            return 14 * distZ + 10 * (distX - distZ) + 10 * distY;
        }
        return 14 * distX + 10 * (distZ - distX) + 10 * distY;
    }

    private void Awake()
    {
        grid = GetComponent<NodeGrid>();
    }



}
