using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMGMTRunState : GameState
{
    float timer;

    public LevelMGMTRunState(GameObject t, GameStateMachine s) : base(t, s)
    {
        timer = ((LevelManager)(sm)).timeLimit;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            ((LevelManager)(sm)).levelUI.SetTimerText(timer);
        }
        else
        {
            timer = 0;
            ((LevelManager)(sm)).levelUI.SetTimerText("Time\'s Up!");
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

}
