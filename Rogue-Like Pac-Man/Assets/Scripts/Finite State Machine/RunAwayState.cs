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

    public override void EnterState(Unit _owner) {

    }

    public override void ExitState(Unit _owner) {

    }

    public override void UpdateState(Unit _owner) {

    }
}
