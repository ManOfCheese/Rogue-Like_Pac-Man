using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public bool displayGridGizmos;    //When true only display path gizmos.
    public LayerMask unwalkableMask;  //Mask that contains all objects that cannot be traversed.
    public Vector2 gridWorldSize;     //This the size of the grid in world space.
    public float nodeRadius;          //The radius of the node determines it's size.
    public Node[,] grid;             //An array of all the nodes in the grid.

    float nodeDiameter;               //Simply twice the radius, declared here for quick reusability in favour of multiplying nodeRadius each time.
    int gridSizeX, gridSizeY;         //Holds how many nodes fit in the grid on the x and y axes. 

    public int MaxSize {              //Holds the maximum size of the grid.
        get {
            return gridSizeX * gridSizeY;
        }
    }

    private void Awake() {
        nodeDiameter = nodeRadius * 2;                                  //Calculates the nodeDiameter by multiplying the nodeRadius by 2.
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);   //Calculates the amount of nodes that fit on the x axis by dividing it by the nodeDiameter. Rounds to int because we only use whole nodes.
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);   // Does the same for the y axis /\.
        CreateGrid();                                                   //Calls the function that creates the grid.
    }

    //Creates the grid.
    private void CreateGrid() {
        grid = new Node[gridSizeX, gridSizeY];                                                                                                          //Creates new node array Node and sets it's size equal to the gridSizeX and gridSizeY.
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;                          //Calculates the Vector3 of the bottom left corner of the grid for instantiating nodes.

        for (int x = 0; x < gridSizeX; x++) {                                                                                                           //Loops for an amount of times equal to gridSizeX
            for (int y = 0; y < gridSizeY; y++) {                                                                                                       //Loops for an amount of times equal to gridSizeY, meaning it now loops gridSizeX * gridSizeY amount of times.
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);  //Sets the worldposition for current node starting in the bottom left and expanding on from there by mulitplying it by x/y.
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));                                                     //Sets the node to walkable it has no colliders within radius, otherwise sets it to unwalkable.
                grid[x, y] = new Node(walkable, worldPoint, x, y);                                                                                            //Adds a new node to the grid Node array with the position and walkable values just calculated.
            }
        }
    }

    //Gets the neighbors to the current node.
    public List<Node> GetNeighbors(Node node) {
        List<Node> neighbors = new List<Node>();                                                 //Create new list of nodes.

        for (int x = -1; x <= 1; x++) {                                                          //This for loop searches in a 3x3 block of nodes (so only those directly neighboring the current node.                                                  
            for (int y = -1; y <= 1; y++) {
                if ((x == 0 && y == 0) || (x != 0 && y != 0))                                    //If x and y are both 0 then this is the currentnode so skip this iteration.
                    continue;

                int checkX = node.gridX + x;                                                     //temporary integers for checking wether or not the node is actually inside the grid.
                int checkY = node.gridY + y;                                                     //Same as for the x value /\.

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {    //Checks if the node is inside the grid.
                    neighbors.Add(grid[checkX, checkY]);                                         //If it is it adds it to the list.
                }
            }
        }

        return neighbors;                                                                        //We return the list of neighboring nodes.
    }

    //Calculates what node something is in from a position.
    public Node NodeFromWorldPoint(Vector3 worldPos) {
        float percentX = (worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x;  //Takes your object position + the half the size of the grid divided by the size of the grid resulting in a percentage of where it is on the grid.
        float percentY = (worldPos.y + gridWorldSize.y / 2) / gridWorldSize.y;  //Does the same for the y axis /\.
        percentX = Mathf.Clamp01(percentX);                                     //Clams the values between 0 and 1.
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);                   //Multiplies the grid size (-1 because it starts at 0) and multiplies it by the percentX. Rounding this gives us the number of this node from the left.
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);                   //Does the same for the y axis /\
        return grid[x, y];                                                      //Returnds the node that corresponds to the position in the grid array.
    }

    //Draw Gizmos to display grid and wether or not a node is walkable.
    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));  //Draw a wire cube that shows the size of the grid.
        if (grid != null && displayGridGizmos == true) {                                            //Quick null check on the grid                            
            foreach (Node n in grid) {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;                              //If the node is walkable color it white, if it is not color it red.
                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - 0.1f));                   //Draw a cube one every node just smaller than the actual node so that they can be differentiated on the screen. 
            }
        }
    }
}
