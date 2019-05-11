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
