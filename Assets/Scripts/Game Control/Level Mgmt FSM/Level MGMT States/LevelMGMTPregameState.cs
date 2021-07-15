using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMGMTPregameState : LevelMgmtState
{
    public LevelMGMTPregameState(GameObject t, GameStateMachine s) : base(t, s)
    {
    }

    public override void OnEnter()
    {
     //   Debug.Log("Entering pregame state");
        base.OnEnter();
        ((LevelManager)sm).SendUIMessages(new SceneOverlayMessage[3] {
            new SceneOverlayMessage("Ready?",TextDisplayer.TextSpeed.SKIP,1.3f),
            new SceneOverlayMessage("Set",TextDisplayer.TextSpeed.FAST,1f),
            new SceneOverlayMessage("Go!",TextDisplayer.TextSpeed.FAST,1f)
            }
        );
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }


    /*
     Once the opening "ready, set, go" messages have finished, change to the main game state
     */
    public override void NotifyMessageFinished()
    {
        base.NotifyMessageFinished();
        lm().ChangeState(lm().runState);
    }

}
