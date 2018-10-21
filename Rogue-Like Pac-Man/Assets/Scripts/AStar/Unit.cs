using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class Unit : MonoBehaviour {

    public string currentState;

    public Pathfinding pathfinding;
    public Vector3 cagePos;
    public float speed;       //Speed of movement, later multiplied by time.DeltaTime
    public StateMachine<Unit> StateMachine { get; set; }

    public int respawnTime = 10;

    public Ghost consumableScript;
    public Animator animator;
    public Vector3 target;  //The target to move towards.
    public int blueModeDuration = 10;

    private Vector3[] path;           //The path in an array of Vector3's.
    private int targetIndex;          //The current index of the waypoint we are moving to towards.

    private void Start() {
        animator = GetComponent<Animator>();                                               //Get the animator.
        StateMachine = new StateMachine<Unit>(this);                                       //Create a new state machine.
        StateMachine.ChangeState(ChaseState.Instance);                                     //Enter the Chase state.
        PathRequestManager.RequestPath(transform.position, target, OnPathFound);
        consumableScript = GetComponent<Ghost>();
        pathfinding = GameObject.Find("A*").GetComponent<Pathfinding>();
    }

    private void Update() {
        StateMachine.Update();
        StateMachine.UpdateTarget();
    }

    private void OnEnable() {
        EventManager.BlueMode += BlueModeActive;  //Scubscribe the BlueModeActive function to the BlueMode event.
        EventManager.EndBlueMode += BlueModeEnd;
        EventManager.PacManDeath += OnPacManDeath;
        EventManager.UltraPellet += OnUltraPelletEaten;
    }

    private void OnDisable() {
        EventManager.BlueMode -= BlueModeActive;  //Unscubscribe the BlueModeActive function to the BlueMode event.
        EventManager.EndBlueMode -= BlueModeEnd;
        EventManager.PacManDeath -= OnPacManDeath;
        EventManager.UltraPellet -= OnUltraPelletEaten;
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
        targetIndex = 0;
        Vector3 currentWaypoint;
        if (path.Length < 1) {
            currentWaypoint = transform.position;
        }
        else
            currentWaypoint = path[0];                  //The current waypoint we are moving towards, starting with the first one.

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
        PathRequestManager.RequestPath(transform.position, target, OnPathFound);                                    //Request a new path.
    }

    public void BlueModeActive() {
        if (currentState != "DeadState") {
            StateMachine.ChangeState(RunAwayState.Instance);
        }
    }

    public void BlueModeEnd() {
        if (currentState != "DeadState") {
            StateMachine.ChangeState(ChaseState.Instance);
        }
    }

    public void OnGhostEaten() {
        StateMachine.ChangeState(DeadState.Instance);
    }
    
    public void OnPacManDeath() {
        StateMachine.ChangeState(ChaseState.Instance);
    }

    public void OnUltraPelletEaten() {
        StateMachine.ChangeState(RunAwayState.Instance);
        blueModeDuration = 999999999;
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
