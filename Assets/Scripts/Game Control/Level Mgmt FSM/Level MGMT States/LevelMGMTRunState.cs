using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This is the main state for gameplay.
 */
public class LevelMGMTRunState : LevelMgmtState
{
    float gameTimer;
    const float spawnDelay = 2; //after the "Enemies Approaching" message is displayed, wait a few seconds before spawning the new enemies.
    const float messageDelay = 1; //after last enemy of the wave is destroyed, wait a few seconds before sending the spawn message

    public LevelMGMTRunState(GameObject t, GameStateMachine s) : base(t, s)
    {
        gameTimer = ((LevelManager)(sm)).timeLimit;
    }

    public override void NotifyEnemyWaveCleared()
    {
        base.NotifyEnemyWaveCleared();

        //Start a new wave 
        if (gameTimer> spawnDelay + messageDelay)
        {
            lm().StartCoroutine(this.WaveMessageDelayCoroutine());
        }
    }

    public override void NotifyMessageFinished()
    {
        base.NotifyMessageFinished();
        Debug.Log("Notified in run state");

    }

    public override void OnEnter()
    {
        base.OnEnter();
        lm().StartCoroutine(this.EnemySpawnDelayCoroutine());

    }



    //Displays a message notifying the player of a new wave of approaching enemies
    void SendEnemyWaveMessage()
    {
        
            ((LevelManager)sm).SendUIMessages(new SceneOverlayMessage[]{
            new SceneOverlayMessage("Enemies Approaching!",TextDisplayer.TextSpeed.FAST,0.7f),
            new SceneOverlayMessage("Wave #"+lm().GetWaveNumber(),TextDisplayer.TextSpeed.FAST,0.7f)


            });
        
        
    }
    //Coroutine that delays spawning new enemies until the enemy wave message disappears
    IEnumerator EnemySpawnDelayCoroutine()
    {
        //Debug.Log("Starting enemy spawning delay coroutine");
        yield return new WaitForSeconds(spawnDelay);
        lm().SpawnEnemyWave();
    }
    IEnumerator WaveMessageDelayCoroutine()
    {
        Debug.Log("Wave message delay coroutine");
        yield return new WaitForSeconds(messageDelay);
        SendEnemyWaveMessage();
        lm().StartCoroutine(this.EnemySpawnDelayCoroutine());
    }






    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (gameTimer > 0)
        {
            gameTimer -= Time.deltaTime;
            ((LevelManager)(sm)).levelUI.SetTimerText(gameTimer);
        }
        else
        {
            gameTimer = 0;
            ((LevelManager)(sm)).levelUI.SetTimerText("Time\'s Up!");
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

}
