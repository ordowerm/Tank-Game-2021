using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected GameObject target; //character or actor modified by the state
    protected GameStateMachine sm; //state machine

    public State(GameObject t, GameStateMachine s) {
        target = t;
        sm = s;
   
    }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
}
