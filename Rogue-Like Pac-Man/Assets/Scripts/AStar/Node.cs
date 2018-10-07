using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node> {

    public bool walkable;     //This bool wether or not you this node is walkable.
    public Vector3 worldPos;  //This holds the position of this node in the world.
    public int gridX;         //Location in the grid on the x axis.
    public int gridY;         //Location in the grid on the y axis.

    public int gCost;         //Cost of movement from the start to target.
    public int hCost;         //Cost of movement from the target to start.
    public Node parent;       //Parent of this node (previous in the path).
    int heapIndex;
	
    //Constructor.
    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY) {
        walkable = _walkable;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int FCost { //Calculate FCost when it is requested.
        get {
            return gCost + hCost;
        }
    }

    public int HeapIndex { //Index of node on the heap.
        get {
            return heapIndex;
        }
        set {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare) {               //Compare two nodes.
        int compare = FCost.CompareTo(nodeToCompare.FCost);  //compare is equal to this nodes fCost compared to the nodeToCompares fCost.
        if (compare == 0) {                                  //The fCost values are the same.
            compare = hCost.CompareTo(nodeToCompare.hCost);  //Compare the hCost values as a tie breaker.
        }
        return -compare;                                     //Return the inverted value of compare.
    }
}
