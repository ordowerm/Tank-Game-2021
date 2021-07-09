using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMgmtState : GameState
{
    public LevelMgmtState(GameObject t, GameStateMachine s) : base(t, s)
    {
    }

    public virtual void NotifyMessageFinished()
    {

    }
    public virtual void NotifyEnemyWaveCleared() { }

    //Method just does a cast so that I don't have to type as much.
    public LevelManager lm()
    {
        return ((LevelManager)sm);
    }
}
