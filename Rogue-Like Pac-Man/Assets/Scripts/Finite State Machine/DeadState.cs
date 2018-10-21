using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class DeadState : State<Unit> {
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

    private float respawnTimer = 0;
    private int respawnTime = 10;
    private Vector3 target;

    public override void EnterState(Unit _owner) {
        _owner.GetComponent<CircleCollider2D>().enabled = false;
        _owner.currentState = "DeadState";
        _owner.consumableScript.enabled = false;
        target = _owner.cagePos;
        _owner.target = target;
        //PathRequestManager.RequestPath(_owner.transform.position, target, _owner.OnPathFound);
        _owner.animator.SetInteger("BlueMode", 0);
        _owner.animator.SetBool("Dead", true);
        respawnTimer = 0;
    }

    public override void ExitState(Unit _owner) {
        _owner.animator.SetBool("Dead", false);
        _owner.GetComponent<CircleCollider2D>().enabled = true;
    }

    public override void UpdateState(Unit _owner) {
        respawnTimer += Time.deltaTime;
        if (respawnTimer >= respawnTime) {
            _owner.StateMachine.ChangeState(ChaseState.Instance);
            respawnTimer = 0;
        }
    }

    public override void UpdateTarget(Unit _owner) {
        target = _owner.cagePos;
        _owner.target = target;
    }
}
