using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PathFinder pathfinder;
    public int range = 5;
    bool selected = false;
    public Material inPath;
    public Material inRange;
    public Material undecorated;
    List<Node> walkableArea;
    Node lastTargetTile;
    List<Node> lastPath;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (selected)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    if (hit.collider.CompareTag("Tile"))
                    {
                        // If the mouse cursor is over a new tile than it was in the previous frame, remove the old path and calculate a new one.
                        Node targetNode = pathfinder.TargetNodeFromWorldPosition(hit.transform.position);
                        if (!targetNode.Equals(lastTargetTile))
                        {
                            if (lastPath != null)
                            {
                                foreach (Node node in lastPath)
                                {
                                    node.worldObject.GetComponent<Renderer>().material = inRange;
                                }
                            }
                            List<Node> path = pathfinder.FindPath(transform.position, hit.transform.position);
                            lastTargetTile = targetNode;
                            foreach (Node node in path)
                            {
                                node.worldObject.GetComponent<Renderer>().material = inPath;
                            }
                            lastPath = path;
                        }
                    }
                }
            }
            else
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        selected = true;
                        // Find the range.
                        walkableArea = pathfinder.FindRange(transform.position, range);
                        foreach (Node node in walkableArea)
                        {
                            node.worldObject.GetComponent<Renderer>().material = inRange;
                        }
                    }
                }
            }
        }
    }
}
