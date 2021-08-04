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
    WaveList wlist;
    int subwaveId = 0;

    public LevelMGMTRunState(GameObject t, GameStateMachine s) : base(t, s)
    {
        gameTimer = ((LevelManager)(_sm)).timeLimit;
    }

    public override void NotifyEnemyWaveCleared()
    {
        base.NotifyEnemyWaveCleared();

        //Start a new wave 
        if (gameTimer> spawnDelay + messageDelay)
        {
            Debug.Log("Running coroutine");
            RunCoroutine();
        }

       
    }

    public override void NotifyMessageFinished()
    {
        base.NotifyMessageFinished();
        //Debug.Log("Notified in run state");

    }

    public override void OnEnter()
    {
        base.OnEnter();
        wlist = lm().GetCurrentWave();
        RunCoroutine();
        //lm().StartCoroutine(this.EnemySpawnDelayCoroutine());

    }



    //Displays a message notifying the player of a new wave of approaching enemies
    void SendEnemyWaveMessage()
    {
        
            ((LevelManager)_sm).SendUIMessages(new SceneOverlayMessage[]
            {
                new SceneOverlayMessage("Enemies Approaching!",TextDisplayer.TextSpeed.FAST,0.7f),
                new SceneOverlayMessage("Wave #"+lm().GetWaveNumber(),TextDisplayer.TextSpeed.FAST,0.7f)

            });       
    }

    /*
     * Refactor this to conform to best practices 
     * 
     */
    IEnumerator SubWaveSpawnCoroutine()
    {
        EnemyWave[] subwaves = wlist.subwaves;
        //Debug.Log("Subwave number: "+subwaveId+". Content: "+subwaves.Length);
        yield return new WaitForSeconds(subwaves[subwaveId].spawnDelay);
        foreach (EnemySpawnData esd in subwaves[subwaveId].enemies)
        {
            lm().SpawnEnemy(esd);
        }
        subwaveId++;

        if (subwaveId < wlist.subwaves.Length)
       {
            //If you don't have to wait for the current subwave of enemies to clear, automatically spawn the next subwave
            if (!subwaves[subwaveId].waitAllClear)
            {
                lm().StopCoroutine(this.SubWaveSpawnCoroutine());
                lm().StartCoroutine(this.SubWaveSpawnCoroutine());
            }
       }


    }


    IEnumerator SpawnNewWaveCoroutine()
    {
        //yield return new WaitWhile(delegate { return spawning; });
        //Debug.Log("Wave message delay coroutine");
        subwaveId = 0;
        yield return new WaitForSeconds(messageDelay);
        SendEnemyWaveMessage();
        yield return new WaitForSeconds(spawnDelay);
        RunCoroutine();
    }

    protected void RunCoroutine()
    {
        if (subwaveId < wlist.subwaves.Length)
        {
            //Debug.Log("About to run SubwaveSpawn");
            lm().StartCoroutine(this.SubWaveSpawnCoroutine());
        }
        else
        {
            //Debug.Log("About to run SpawnNewWave");
            lm().StopCoroutine(SubWaveSpawnCoroutine());
            lm().AdvanceWave();
            wlist = lm().GetCurrentWave();
            lm().StartCoroutine(this.SpawnNewWaveCoroutine());
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (gameTimer > 0)
        {
            gameTimer -= Time.deltaTime;
            ((LevelManager)(_sm)).levelUI.SetTimerText(gameTimer);
        }
        else
        {
            gameTimer = 0;
            ((LevelManager)(_sm)).levelUI.SetTimerText("Time\'s Up!");
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

}
