using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : InputState
{
    protected bool isActive;
    protected float iFrameTimer;

    public PlayerState(GameObject t, GameStateMachine s, IControllerInput c) : base(t, s, c)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        iFrameTimer = ((PlayerSM)sm).GetIFrameTimer();
    }

    public override void OnExit()
    {
        base.OnExit();
        ((PlayerSM)sm).SetIFrameTimer(iFrameTimer);
    }

    public override void HandleInput()
    {
        base.HandleInput();
        //TO-DO: button presses change the weapon

    }
}
