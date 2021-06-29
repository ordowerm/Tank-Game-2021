using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    public bool debug;
    protected GameState currentState;
    protected GameState previousState; //for now, let's not include a full stack of states. Just use the most recent
    public LevelManager levelManager;



    public void Initialize(GameState startState)
    {
        currentState = startState;
        currentState.OnEnter();
        previousState = null;
    }


    public virtual void ChangeState(GameState nextState)
    {
        if (previousState != currentState)
        {
            previousState = currentState;
        }
        currentState.OnExit();
        currentState = nextState;
        if (debug)
        {
            Debug.Log("Entering state: " + currentState);
        }
        currentState.OnEnter();
    }

    public virtual void ChangePrevious()
    {
        ChangeState(previousState);
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
