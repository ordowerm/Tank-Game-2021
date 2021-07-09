using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This is the main state for gameplay.
 */
public class LevelMGMTRunState : LevelMgmtState
{
    float timer;

    public LevelMGMTRunState(GameObject t, GameStateMachine s) : base(t, s)
    {
        timer = ((LevelManager)(sm)).timeLimit;
    }

    public override void NotifyEnemyWaveCleared()
    {
        base.NotifyEnemyWaveCleared();

        //Start a new wave 
        if (timer>3)
        {
            SendEnemyWaveMessage();
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    void SendEnemyWaveMessage()
    {
        
            ((LevelManager)sm).SendUIMessages(new SceneOverlayMessage[]{
            new SceneOverlayMessage("Enemies Approaching!",TextDisplayer.TextSpeed.SKIP,0.7f),
            new SceneOverlayMessage("Wave "+lm().GetWaveNumber(),TextDisplayer.TextSpeed.SKIP,0.7f)


            });
        
        
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
