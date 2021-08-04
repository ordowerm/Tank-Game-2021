using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMGMTPostgameState : GameState
{
    public LevelMGMTPostgameState(GameObject t, GameStateMachine s) : base(ref t,ref s)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        ((LevelManager)_sm).SendMessage("Game over!");
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
