using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    public bool debug;
    protected State currentState;
    

    public void Initialize(State startState)
    {
        currentState = startState;
        currentState.OnEnter();
    }


    public void ChangeState(State nextState)
    {
        currentState.OnExit();
        currentState = nextState;
        if (debug)
        {
            Debug.Log("Entering state: " + currentState);
        }
        currentState.OnEnter();
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        currentState.LogicUpdate();
    }

    protected virtual void FixedUpdate()
    {
        currentState.PhysicsUpdate();
    }
}
