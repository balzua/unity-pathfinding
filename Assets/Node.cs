using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public int x;
    public int y;
    public int z;
    int heapIndex;

    public int gCost;
    public int hCost;

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node node)
    {
        int compare = fCost.CompareTo(node.fCost);
        // If the fCosts are equal, hCost is used as a tie breaker
        if (compare == 0)
        {
            compare = hCost.CompareTo(node.hCost);
        }
        // We need to return "1" if the cost is LOWER (need to flip the sign)
        return -compare;
    }

    public Node parentNode;
    public bool isWalkable = true;

    public GameObject worldObject;

}
