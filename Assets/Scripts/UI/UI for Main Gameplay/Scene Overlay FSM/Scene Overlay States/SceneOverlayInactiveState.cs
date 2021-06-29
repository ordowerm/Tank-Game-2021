using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneOverlayInactiveState : GameState
{
    public SceneOverlayInactiveState(GameObject t, GameStateMachine s) : base(t, s)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
