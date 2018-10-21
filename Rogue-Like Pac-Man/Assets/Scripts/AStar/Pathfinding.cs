using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pathfinding : MonoBehaviour {

    PathRequestManager requestManager;  //Reference to the PathRequestManager
    Grid grid;                          //Reference to grid class.

    private void Awake() {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>(); //Get a reference to the grid component.
    }


    public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    //Find the fastest path between two points
    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos) {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPos);    //Starting node calculated from a start position.
        Node targetNode = grid.NodeFromWorldPoint(targetPos);  //Target node calculated from a target postion.

        if (startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);     //Open nodes are the ones currently being calculated.
            HashSet<Node> closedSet = new HashSet<Node>();         //Closed nodes are the ones already calculated.
            openSet.Add(startNode);                                //Add the starting node to the openSet.  

            while (openSet.Count > 0)
            {                    //While there is a node in the openSet.
                Node currentNode = openSet.RemoveFirst();  //Set the current node to the first node in the openSet.
                closedSet.Add(currentNode);                //And add it to the closed set.

                if (currentNode == targetNode) {           //If the current node is the target node.

                    pathSuccess = true;
                    break;                                 //Exit the loop.
                }

                foreach (Node neighbor in grid.GetNeighbors(currentNode)) {                                  //For every node neighboring the current node.
                    if (!neighbor.walkable || closedSet.Contains(neighbor)) {                                //If it is not walkable or in the closedSet.
                        continue;                                                                            //Skip this iteration.
                    }

                    int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);  //Calculate the new movement cost for getting to neighboring nodes by adding the distance between the two to the gCost.
                    if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor)) {         //If the newMovementCostToNeighbor is less than the neighbors gCost or it is in the openSet.
                        neighbor.gCost = newMovementCostToNeighbor;                                          //Set the gCost equal to the newMovementCostToNeighbor.
                        neighbor.hCost = GetDistance(neighbor, targetNode);                                  //Set the hCost equal to the distance between the neighbor and the target node.
                        neighbor.parent = currentNode;                                                       //parent it to the current node thus adding it to the path.

                        if (!openSet.Contains(neighbor)) {                                                   //If it is not already in the openSet.
                            openSet.Add(neighbor);                                                           //Add it.
                        }
                        else {
                            openSet.UpdateItem(neighbor);
                        }
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess) {
            waypoints = RetracePath(startNode, targetNode);    //Retrace the path that was taken to get here.
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    //Retraces the succesful path to the target node.
    Vector3[] RetracePath(Node startNode, Node endNode) {  
        List<Node> path = new List<Node>();    //New list of nodes for storing the succesful path.
        Node currentNode = endNode;            //reference to the current node which is the end node for the first iteration.

        while (currentNode != startNode) {     //While the current node is not the start node.
            path.Add(currentNode);             //Add the current node to the path.
            currentNode = currentNode.parent;  //Set the current node to the parent of the current current node.
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);              //Once all nodes are added to the path reverse it to be the right way around (from start to target)
        return waypoints;                           
    }

    //Simplifies path into only the necessary waypoints.
    Vector3[] SimplifyPath(List<Node> path)  {
        List<Vector3> waypoints = new List<Vector3>();  //This list will contain our waypoints.

        for (int i = 1; i < path.Count; i++) {          //Enter a for loop on the path nodes.
                waypoints.Add(path[i].worldPos);        //Add this waypoint to our waypoints list.
        }
        return waypoints.ToArray();                     //Return the waypoint list as an array.
    }

    public Node FindFurthestNode(Vector3 playerTransform) {
        Node furthestNode = grid.grid[1,1];

        foreach (Node node in grid.grid) {
            if (node.walkable) {
                float distance = GetDistance(grid.NodeFromWorldPoint(playerTransform), node);
                float furthestNodeDistance = GetDistance(grid.NodeFromWorldPoint(playerTransform), grid.NodeFromWorldPoint(furthestNode.worldPos));
                if (distance >= furthestNodeDistance) {
                    furthestNode = node;
                }
            }
        }
        return furthestNode;
    }

    //Calculates distance between two nodes.
    private int GetDistance(Node nodeA, Node nodeB) {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);  //Distance on the x axis is equal to nodaA - nodeB's grid position.
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);  //Same is true for the y axis.

        if (distX > distY)                                 //If the x distance is more than the y distance.
            return 14 * distY + 10 * (distX - distY);      //Return this
        return 14 * distX + 10 * (distY - distX);          //If the x distance is less than the y distance return this.
    }
}
