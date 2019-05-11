using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour {

    // Default grid dimensions
    public int maxX = 10;
    public int maxY = 1;
    public int maxZ = 10;

    public int xOffset = 1;
    public int yOffset = 1;
    public int zOffset = 1;

    public Node[,,] grid;

    public GameObject gridFloorPrefab;


    // Start is called before the first frame update
    void Start()
    {
        grid = new Node[maxX, maxY, maxZ];
        for (int x = 0; x < maxX; x++)
        {
            for (int y = 0; y < maxY; y++)
            {
                for (int z = 0; z < maxZ; z++)
                {
                    float xPos = x * xOffset;
                    float yPos = y * yOffset;
                    float zPos = z * zOffset;
                    //Instantiate a tile at this grid position
                    GameObject tile = Instantiate(gridFloorPrefab, new Vector3(xPos, yPos, zPos), gridFloorPrefab.transform.rotation, transform) as GameObject;
                    tile.transform.name = "(" + x.ToString() + ", " + y.ToString() + ", " + z.ToString() + ")";

                    //Create a node and assign it the proper coordinates and assign it the corresponding world tile, then place it into the grid array of nodes
                    Node node = new Node();
                    node.x = x;
                    node.y = y;
                    node.z = z;
                    node.worldObject = tile;
                    grid[x, y, z] = node;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Gets a Node from a set of grid coordinates
    public Node GetNode(int x, int y, int z)
    {
        Node returnNode = null;
        // Check to make sure the requested node is within the limits of the grid.
        if (x < maxX && x >= 0 && y < maxY && y >= 0 && z < maxZ && z >= 0)
        {
            returnNode = grid[x, y, z];
        }
        return returnNode;
    }

    // Gets a node from a world position (in Vector3 format)
    public Node GetNodeFromWorldPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.y);
        int z = Mathf.RoundToInt(position.z);
        return GetNode(x, y, z);
    }

    // Given a Node, provides a list of all accessible (in range and flagged as walkable) neighbors to that Node.
    public List<Node> GetNodeNeighbors(Node node)
    {
        List<Node> allNeighbors = new List<Node>();
        List<Node> allowedNeighbors = new List<Node>();

        Node topLeft = GetNode(node.x - 1, node.y, node.z + 1);
        if (topLeft != null) allNeighbors.Add(topLeft);

        Node top = GetNode(node.x, node.y, node.z + 1);
        if (top != null) allNeighbors.Add(top);

        Node topRight = GetNode(node.x + 1, node.y, node.z + 1);
        if (topRight != null) allNeighbors.Add(topRight);

        Node left = GetNode(node.x - 1, node.y, node.z);
        if (left != null) allNeighbors.Add(left);

        Node right = GetNode(node.x + 1, node.y, node.z);
        if (right != null) allNeighbors.Add(right);

        Node botLeft = GetNode(node.x - 1, node.y, node.z - 1);
        if (botLeft != null) allNeighbors.Add(botLeft);

        Node bot = GetNode(node.x, node.y, node.z - 1);
        if (bot != null) allNeighbors.Add(bot);

        Node botRight = GetNode(node.x + 1, node.y, node.z - 1);
        if (botRight != null) allNeighbors.Add(botRight);


        foreach (Node neighbor in allNeighbors)
        {
            if (neighbor.isWalkable) allowedNeighbors.Add(neighbor);
        }
        return allowedNeighbors;
    }

    // For making this class into a Singleton
    public static NodeGrid instance;
    public static NodeGrid GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

}
