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
        if (selected)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider.CompareTag("Tile"))
                {
                    // Find the path.
                    List<Node> path = pathfinder.FindPath(transform.position, hit.transform.position);
                    if (path[path.Count - 1] != lastTargetTile)
                    {
                        lastTargetTile = path[path.Count - 1];
                        if (lastPath != null)
                        {
                            foreach (Node node in lastPath)
                            {
                                node.worldObject.GetComponent<Renderer>().material = inRange;
                            }
                        }
                        foreach (Node node in path)
                        {
                            node.worldObject.GetComponent<Renderer>().material = inPath;
                        }
                        lastPath = path;
                    }
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
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
