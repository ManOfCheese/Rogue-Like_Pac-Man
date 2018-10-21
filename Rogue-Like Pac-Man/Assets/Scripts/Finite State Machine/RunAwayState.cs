using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class RunAwayState : State<Unit> {

    //Create a single instance of this state for all state machines.
    private static RunAwayState _instance;

    private RunAwayState() {
        if (_instance != null) {
            return;
        }

        _instance = this;
    }

    public static RunAwayState Instance {
        get {
            if(_instance == null) {
                new RunAwayState();
            }

            return _instance;
        }
    }

    private float blueModeTimer = 0;  //timer for tracking the duration of blue mode.
    private Vector3 target;           //Target for pathfinding.
    private GameObject player;        //Reference to the player.

    //When entering this state.
    public override void EnterState(Unit _owner) {
        _owner.CurrentState = "RunAwayState";                                              //Set the currentState of our unit to the RunAway state.
        _owner.ConsumableScript.enabled = true;                                            //Enable the consumableScript.
        player = GameObject.FindGameObjectWithTag("Player");                               //Get the player.
        target = _owner.Pathfinding.FindFurthestNode(player.transform.position).worldPos;  //Set target equal to the node that is the farthest away from the player.
        _owner.Target = target;                                                            //Set the units target equal to our target.
        _owner.Animator.SetInteger("BlueMode", 1);                                         //Tell the animator we are entering blue mode.
        blueModeTimer = 0;                                                                 //Reset the blue mode timer.
    }


    //When exiting this state.
    public override void ExitState(Unit _owner) {
        //...
    }


    //Update this state.
    public override void UpdateState(Unit _owner) {
        blueModeTimer += 1 * Time.deltaTime;                    //Timer for blue mode.
        if (blueModeTimer >= _owner.BlueModeDuration * 0.75) {  //If we are a three quarters of the duration of blue mode.
            _owner.Animator.SetInteger("BlueMode", 2);          //Tell the animator blue mode is almost over.
        }
        if (blueModeTimer >= _owner.BlueModeDuration) {         //If the timer is equal to the duration.
            _owner.Animator.SetInteger("BlueMode", 0);          //Tell the animator blue mode is over.
            EventManager.Instance.OnBlueModeEnd();              //Call the EndBlueMode event.            
        }
    }


    //Update the target of this state.
    public override void UpdateTarget(Unit _owner) {
        target = _owner.Pathfinding.FindFurthestNode(player.transform.position).worldPos;  //Set target equal to the node that is the farthest away from the player. 
        _owner.Target = target;                                                            //Set the units target equal to our target.
    }
}
