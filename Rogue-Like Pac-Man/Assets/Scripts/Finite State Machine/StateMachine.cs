using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine {
    public class StateMachine<T> {

        public State<T> CurrentState { get; private set; }  //Tracks what our current state is.
        public T Owner;                                     //Holds the owner of this state machine.

        public StateMachine(T _owner) {  //Constructor.
            Owner = _owner;
            CurrentState = null;
        }


        //Change our current state.
        public void ChangeState(State<T> _newState) {
            if (CurrentState != null) {         //If we are in a state.
                CurrentState.ExitState(Owner);  //Run the ExitState of our current state.
            }
            CurrentState = _newState;           //Set the current state to be our new state.
            CurrentState.EnterState(Owner);     //Run the EnterState of our new state.
        }


        //Update this state.
        public void Update() {
            if (CurrentState != null) {           //If we are in a state.
                CurrentState.UpdateState(Owner);  //Update that state.
            }
        }


        //Update the target of this state.
        public void UpdateTarget() {
            if (CurrentState != null) {            //If we are in a state.
                CurrentState.UpdateTarget(Owner);  //Update the target of that state.
            }
        }
    }


    //Abstract class containing all the functionality a state should have.
    public abstract class State<T> {
        public abstract void EnterState(T _owner);
        public abstract void ExitState(T _owner);
        public abstract void UpdateState(T _owner);
        public abstract void UpdateTarget(T _owner);
    }
}

