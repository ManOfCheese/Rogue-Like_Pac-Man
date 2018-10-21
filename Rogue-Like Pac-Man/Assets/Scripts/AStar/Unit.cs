using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class Unit : MonoBehaviour {

    public string CurrentState { get; set; }   //The current state of the finite state machine.
    public float Speed = 4;                    //Speed of the unit. Needs to be set in the editor.
    public int BlueModeDuration { get; set; }  //Duration of blue mode.
    public Vector3 cagePos;                    //Needs to be assigned in editor.

    public Pathfinding Pathfinding { get; set; }          //A* pathfinding.
    public StateMachine<Unit> StateMachine { get; set; }  //Finite statemachine.
    public Ghost ConsumableScript { get; set; }           //Script that enables the ghost to be eaten.
    public Animator Animator { get; set; }                //Animator.
    public Vector3 Target { get; set; }                   //The target to move towards.

    private Vector3[] path;           //The path in an array of Vector3's.
    private int targetIndex;          //The current index of the waypoint we are moving to towards.

    //Initialize Variables.
    private void Start() {
        Pathfinding = GameObject.Find("A*").GetComponent<Pathfinding>();          //Get pathfinding script.
        Animator = GetComponent<Animator>();                                      //Get the animator.
        StateMachine = new StateMachine<Unit>(this);                              //Create a new state machine.
        StateMachine.ChangeState(ChaseState.Instance);                            //Enter the Chase state.
        ConsumableScript = GetComponent<Ghost>();                                 //Get Consumable script.
        BlueModeDuration = 10;                                                    //Set BlueModeDuration.
        PathRequestManager.RequestPath(transform.position, Target, OnPathFound);  //Request a path.
    }


    //Update Function.
    public void OnUpdate() {
        StateMachine.Update();        //Update the current state.
        StateMachine.UpdateTarget();  //Update the target for pathfinding.
    }


    //Subscribe to events when enabled.
    private void OnEnable() {
        EventManager.BlueMode += BlueModeActive;         //Scubscribe BlueModeActive to the BlueMode event.
        EventManager.EndBlueMode += BlueModeEnd;         //Subscribe BlueModeEnd to the EndBlueMode event.
        EventManager.PacManDeath += OnPacManDeath;       //Subscribe OnPacManDeath to the PacManDeath event
        EventManager.UltraPellet += OnUltraPelletEaten;  //Subscribe OnUltraPelletEaten to the UltraPellet event.
    }


    //Unsubscribe to events when disabled.
    private void OnDisable() {
        EventManager.BlueMode -= BlueModeActive;         //Unscubscribe BlueModeActive from the BlueMode event.
        EventManager.EndBlueMode -= BlueModeEnd;         //Unsubscribe BlueModeEnd from the EndBlueMode event.
        EventManager.PacManDeath -= OnPacManDeath;       //Unsubscribe OnPacManDeath from the PacManDeath event
        EventManager.UltraPellet -= OnUltraPelletEaten;  //Unsubscribe OnUltraPelletEaten from the UltraPellet event.  
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
        targetIndex = 0;                           //Set the targetIndex to 0;
        Vector3 currentWaypoint;                   //Current waypoint to move towards.
        if (path.Length < 1) {                     //If the path is empty.
            currentWaypoint = transform.position;  //Set the current waypoint to our own position.
        }
        else                                       //Otherwise.
            currentWaypoint = path[0];             //Set it to the first waypoint in the path.

        while (true) {                                     //Enter a loop.
            if (transform.position == currentWaypoint) {   //If we are at the waypoint.
                targetIndex++;                             //Add one to the targetIndex.
                if (targetIndex >= 1) {                    //If the targetIndex is more than or equal to one (meanig we took one step along the path)
                    break;                                 //Exit the while loop
                }
                currentWaypoint = path[targetIndex];       //Otherwise set the waypoint to be the next waypoint in the path.
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, Speed * Time.deltaTime);                   //Move towards the waypoint.
            if ((transform.position - currentWaypoint).normalized == new Vector3(-1, 0, 0)) { Animator.SetInteger("Dir", 0); }       //Get the direction and put it into the animator.
            else if ((transform.position - currentWaypoint).normalized == new Vector3(0, 1, 0)) { Animator.SetInteger("Dir", 1); }
            else if ((transform.position - currentWaypoint).normalized == new Vector3(1, 0, 0)) { Animator.SetInteger("Dir", 2); }
            else if ((transform.position - currentWaypoint).normalized == new Vector3(0, -1, 0)) { Animator.SetInteger("Dir", 3); }
            yield return null;                                                                                                       //Wait one frame and continue.
        }
        PathRequestManager.RequestPath(transform.position, Target, OnPathFound);                                                     //Request a new path.
    }


    //When a power pellet is eaten.
    public void BlueModeActive() {
        if (CurrentState != "DeadState") {                    //If the unit isn't dead.
            StateMachine.ChangeState(RunAwayState.Instance);  //Enter the RunAway state.
        }
    }


    //When blue mode runs out.
    public void BlueModeEnd() {
        if (CurrentState != "DeadState") {                  //If the unit isn't dead
            StateMachine.ChangeState(ChaseState.Instance);  //Enter the Chase state.
        }
    }


    //When you are eaten.
    public void OnGhostEaten() {
        StateMachine.ChangeState(DeadState.Instance);  //Enter the Dead state.
    }
    

    //When PacMan dies.
    public void OnPacManDeath() {
        StateMachine.ChangeState(ChaseState.Instance);  //Enter the Chase state.
    }


    //When the ultra pellet has been eaten.
    public void OnUltraPelletEaten() {
        StateMachine.ChangeState(RunAwayState.Instance);  //Enter the RunAway state.
        BlueModeDuration = 999999999;                     //Set the duration of this state to A LOT.
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
