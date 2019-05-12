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
    LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
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
                        Vector3[] path = pathfinder.FindPath(transform.position, hit.transform.position);
                        line.positionCount = path.Length;
                        line.SetPositions(path);
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
