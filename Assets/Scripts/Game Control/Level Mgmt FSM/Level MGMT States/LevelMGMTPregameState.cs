using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMGMTPregameState : GameState
{
    public LevelMGMTPregameState(GameObject t, GameStateMachine s) : base(t, s)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        ((LevelManager)sm).SendUIMessages(new string[3] {"Ready?","Set.","Go!" });
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
