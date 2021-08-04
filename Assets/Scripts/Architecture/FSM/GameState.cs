using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    protected GameObject _target; //character or actor modified by the state
    protected GameStateMachine _sm; //state machine

    public GameState(ref GameObject t, ref GameStateMachine s) {
        _target = t;
        _sm = s;
   
    }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
}
