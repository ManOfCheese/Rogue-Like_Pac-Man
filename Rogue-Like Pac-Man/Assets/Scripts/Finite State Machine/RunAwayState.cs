using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class RunAwayState : State<Unit> {
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

    private float blueModeTimer = 0;
    private Vector3 target;
    private GameObject player;

    public override void EnterState(Unit _owner) {
        _owner.currentState = "RunAwayState";
        _owner.consumableScript.enabled = true;
        player = GameObject.FindGameObjectWithTag("Player");
        target = _owner.pathfinding.FindFurthestNode(player.transform.position).worldPos;
        _owner.target = target;
        _owner.animator.SetInteger("BlueMode", 1);
        blueModeTimer = 0;
    }

    public override void ExitState(Unit _owner) {
    }

    public override void UpdateState(Unit _owner) {
        blueModeTimer += 1 * Time.deltaTime;
        if (blueModeTimer >= _owner.blueModeDuration * 0.75) {
            _owner.animator.SetInteger("BlueMode", 2);
        }
        if (blueModeTimer >= _owner.blueModeDuration) {
            _owner.animator.SetInteger("BlueMode", 0);
            EventManager.Instance.OnBlueModeEnd();
            blueModeTimer = 0;
        }
    }

    public override void UpdateTarget(Unit _owner) {
        target = _owner.pathfinding.FindFurthestNode(player.transform.position).worldPos;
        _owner.target = target;
    }
}
