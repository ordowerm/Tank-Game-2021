using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMGMTPostgameState : GameState
{
    public LevelMGMTPostgameState(GameObject t, GameStateMachine s) : base(t, s)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        ((LevelManager)sm).SendMessage("Game over!");
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
}
