using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public Pathfinding pathfinding;
    public Transform targetTransform;
    public Vector3 cagePos;
    public float speed;       //Speed of movement, later multiplied by time.DeltaTime
    private int blueModeDur = 8;
    private int respawnTime = 10;
    private bool isEaten = false;

    private Animator animator;
    private GameObject player;
    private Transform target;  //The target to move towards.

    private float blueModeTimer;
    private bool blueModeActive;

    Vector3[] path;           //The path in an array of Vector3's.
    int targetIndex;          //The current index of the waypoint we are moving to towards.

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);  //Request a path from the PathRequestManager.
        animator = GetComponent<Animator>();
    }

    private void OnEnable() {
        EventManager.BlueMode += BlueModeActive;
    }

    private void OnDisable() {
        EventManager.BlueMode -= BlueModeActive;
    }

    //When a path is returned from the PathRequestManager.
    public void OnPathFound(Vector3[] newPath, bool pathSuccesful) {  
        if (pathSuccesful) {               //If a path has been found.
            path = newPath;                //Set the current path to be the new found path.
            StopCoroutine(FollowPath());   //Makes sure the coroutine isn't already running.
            StartCoroutine(FollowPath());  //Run the follow path coroutine.
        }
        else {
            StopCoroutine(FollowPath());   //Makes sure the coroutine isn't already running.
            StartCoroutine(FollowPath());  //Run the follow path coroutine.
        }
    }

    //Follow the path.
    IEnumerator FollowPath() {
        targetIndex = 0;
        Vector3 currentWaypoint = path[0];                  //The current waypoint we are moving towards, starting with the first one.

        while (true) {                                     //Enter a loop.
            if (transform.position == currentWaypoint) {   //If we are at the waypoint.
                targetIndex++;                             //Add one to the targetIndex.
                if (targetIndex >= 1) {                    //If the targetIndex is more than or equal to one (meanig we took one step along the path)
                    break;                                 //Exit the while loop
                }
                currentWaypoint = path[targetIndex];       //Otherwise set the waypoint to be the next waypoint in the path.
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);  //Move towards the waypoint.
            if ((transform.position - currentWaypoint).normalized == new Vector3(-1, 0, 0)) { animator.SetInteger("Dir", 0); }
            else if ((transform.position - currentWaypoint).normalized == new Vector3(0, 1, 0)) { animator.SetInteger("Dir", 1); }
            else if ((transform.position - currentWaypoint).normalized == new Vector3(1, 0, 0)) { animator.SetInteger("Dir", 2); }
            else if ((transform.position - currentWaypoint).normalized == new Vector3(0, -1, 0)) { animator.SetInteger("Dir", 3); }
            yield return null;                                                                                      //Wait one frame and continue.
        }
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);                                    //Request a new path.
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

    public void BlueModeActive() {
        StartCoroutine(RunAway());
        blueModeActive = true;
    }

    public void OnGhostEaten() {
        target.position = cagePos;
        isEaten = true;
        animator.SetBool("Dead", true);
        animator.SetInteger("BlueMode", 0);
        StartCoroutine(RespawnTimer());
    }

    IEnumerator RunAway() {
        targetTransform.position = pathfinding.FindFurthestNode(player.transform.position).worldPos;
        target = targetTransform;
        animator.SetInteger("BlueMode", 1);
        yield return new WaitForSeconds(blueModeDur * 0.25f);
        targetTransform.position = pathfinding.FindFurthestNode(player.transform.position).worldPos;
        yield return new WaitForSeconds(blueModeDur * 0.25f);
        targetTransform.position = pathfinding.FindFurthestNode(player.transform.position).worldPos;
        yield return new WaitForSeconds(blueModeDur * 0.25f);
        if (isEaten == false) {
            targetTransform.position = pathfinding.FindFurthestNode(player.transform.position).worldPos;
            animator.SetInteger("BlueMode", 2);
        }
        yield return new WaitForSeconds(blueModeDur * 0.25f);
        if (isEaten == false) {
            targetTransform.position = pathfinding.FindFurthestNode(player.transform.position).worldPos;
            animator.SetInteger("BlueMode", 0);
            target = player.transform;
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
            EventManager.Instance.OnBlueModeEnd();
        }
    }

    IEnumerator RespawnTimer() {
        yield return new WaitForSeconds(respawnTime);
        animator.SetBool("Dead", false);
        target = player.transform;
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        isEaten = false;
    }
}
