using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class ChaseState : State<Unit> {

    //Create a single instance of this state for every statemachine that uses it.
    private static ChaseState _instance;

    private ChaseState() {
        if (_instance != null) {
            return;
        }

        _instance = this;
    }

    public static ChaseState Instance {
        get {
            if (_instance == null) {
                new ChaseState();
            }

            return _instance;
        }
    }

    private GameObject player;  //Reference to the player.
    private Vector3 target;     //Target of pathfinding.

    //When entering this state.
    public override void EnterState(Unit _owner) {
        _owner.CurrentState = "ChaseState";                   //Change the currentState in Unit.
        _owner.Animator.SetInteger("BlueMode", 0);            //Tell the animator to exit bluemode.
        player = GameObject.FindGameObjectWithTag("Player");  //Get the player object.
        target = player.transform.position;                   //Set the player to be the target.
        _owner.Target = target;                               //Set the Unit target equal to our target.
    }


    //When exiting this state.
    public override void ExitState(Unit _owner) {
        //Nothing needs to be done.
    }


    //Update this state.
    public override void UpdateState(Unit _owner) {
        //...
    }


    //Update the target.
    public override void UpdateTarget(Unit _owner) {
        target = player.transform.position;  //Get the player position and put it into the target.
        _owner.Target = target;              //Set the Unit target equal to our target.
    }
}
