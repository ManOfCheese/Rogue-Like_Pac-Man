using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class ChaseState : State<Unit> {
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

    private GameObject player;
    private Vector3 target;

    public override void EnterState(Unit _owner) {
        _owner.currentState = "ChaseState";
        _owner.animator.SetInteger("BlueMode", 0);
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform.position;
        _owner.target = target;
        //PathRequestManager.RequestPath(_owner.transform.position, target, _owner.OnPathFound);
    }

    public override void ExitState(Unit _owner) {
    }

    public override void UpdateState(Unit _owner) {
    }

    public override void UpdateTarget(Unit _owner) {
        target = player.transform.position;
        _owner.target = target;
    }
}
