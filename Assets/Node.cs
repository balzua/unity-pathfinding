using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int x;
    public int y;
    public int z;

    public int gCost;
    public int hCost;

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public Node parentNode;
    public bool isWalkable = true;

    public GameObject worldObject;

}
