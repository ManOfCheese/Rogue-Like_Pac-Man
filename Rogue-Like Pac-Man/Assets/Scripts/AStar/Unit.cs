using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public Transform target;  //The target to move towards.
    public float speed;       //Speed of movement, later multiplied by time.DeltaTime
    Vector3[] path;   //The path in an array of Vector3's.
    int targetIndex;  //The current index of the waypoint we are moving to towards.

    private void Start() {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);  //Request a path from the PathRequestManager.
    }

    //When a path is returned from the PathRequestManager.
    public void OnPathFound(Vector3[] newPath, bool pathSuccesful) {  
        if (pathSuccesful) {               //If a path has been found.
            path = newPath;                //Set the current path to be the new found path.
            StopCoroutine(FollowPath());   //Makes sure the coroutine isn't already running.
            StartCoroutine(FollowPath());  //Run the follow path coroutine.
        }
    }

    //Follow the path.
    IEnumerator FollowPath() {
        Vector3 currentWaypoint = path[0];                  //The current waypoint we are moving towards, starting with the first one.

        while (true) {                                     //Enter a loop.
            if (transform.position == currentWaypoint) {   //If we are at the waypoint.
                targetIndex++;                             //Add one to the targetIndex.
                if (targetIndex >= path.Length) {          //If the targetIndex is more than or equal to the path length.
                    yield break;                           //Were done here exit the coroutine.
                }
                currentWaypoint = path[targetIndex];       //Other set the waypoint to be the next waypoint in the path.
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);  //Move towards the waypoint.
            yield return null;                                                                                      //Wait one frame and continue.
        }
    }

    //Draw the path in gizmos.
    public void OnDrawGizmos() {
        if (path != null) {                                         //If there is a path.
            for (int i = targetIndex; i < path.Length; i++) {       //Loop through it starting at the current waypoint index.
                Gizmos.color = Color.black;                 
                Gizmos.DrawCube(path[i], Vector3.one);              //Draw cubes at each waypoint that has not yet been reached.
                 
                if (i == targetIndex) {                             //Draw a line to the waypoint we are currently moving towards.
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else {                                              //Otherwise.
                    Gizmos.DrawLine(path[i - 1], path[i]);          //Draw a line from the first waypoint to the current iteration.
                }
            }
        }
    }
}
