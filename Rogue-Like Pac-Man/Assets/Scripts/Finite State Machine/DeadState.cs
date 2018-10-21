using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class DeadState : State<Unit> {

    //Create a single instance of this state for every statemachine that uses it.
    private static DeadState _instance;

    private DeadState() {
        if (_instance != null) {
            return;
        }

        _instance = this;
    }

    public static DeadState Instance {
        get {
            if (_instance == null) {
                new DeadState();
            }

            return _instance;
        }
    }

    private float respawnTimer = 0;  //Timer for when dead.
    private int respawnTime = 10;    //Amount in seconds the ghost remains dead.
    private Vector3 target;          //Target for pathfinding.


    //When entering this state.
    public override void EnterState(Unit _owner) {
        _owner.GetComponent<CircleCollider2D>().enabled = false;  //Turn of the collider for the unit to prevent being eaten twice.
        _owner.CurrentState = "DeadState";                        //Change the current state for unit.
        _owner.ConsumableScript.enabled = false;                  //Disable the consumable script to prevent beign eaten twice.
        target = _owner.cagePos;                                  //Set the target to be the cage.
        _owner.Target = target;                                   //Set the units target equal to the states target.
        _owner.Animator.SetInteger("BlueMode", 0);                //Tell the animator to exit blue mode.
        _owner.Animator.SetBool("Dead", true);                    //Tell the animator we have died.
        respawnTimer = 0;                                         //Reset the respawn timer.
    }


    //When exiting this state.
    public override void ExitState(Unit _owner) {
        _owner.Animator.SetBool("Dead", false);                  //Tell the animator we are no longer dead.
        _owner.ConsumableScript.enabled = true;                  //Enable the consumable script.
        _owner.GetComponent<CircleCollider2D>().enabled = true;  //Enable the collider.
    }


    //Update this state.
    public override void UpdateState(Unit _owner) {
        respawnTimer += Time.deltaTime;                            //Timer for respawn.
        if (respawnTimer >= respawnTime) {                         //When ten seconds have passed.
            _owner.StateMachine.ChangeState(ChaseState.Instance);  //Enter the Chase state.
        }
    }


    //Update the pathfinding target.
    public override void UpdateTarget(Unit _owner) {
        target = _owner.cagePos;  //Set the target to be the cage.
        _owner.Target = target;   //Set the units target equal to out target.
    }
}
