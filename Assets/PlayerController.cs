using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PathFinder pathfinder;
    public Material inRange;
    public Material current;
    public int range = 5;

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
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    // Find the range.
                    List<Node> walkableArea = pathfinder.FindRange(transform.position, range);
                    foreach (Node node in walkableArea)
                    {
                        node.worldObject.GetComponent<Renderer>().material = inRange;
                    }
                }
                if (hit.collider.CompareTag("Tile"))
                {
                    // Find the path.
                    List<Node> path = pathfinder.FindPath(transform.position, hit.transform.position);
                    foreach (Node node in path)
                    {
                        node.worldObject.GetComponent<Renderer>().material = current;
                    }
                }
            }
        }
    }
}
