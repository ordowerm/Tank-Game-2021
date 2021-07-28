using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyState : GameState
{


    public EnemyState(GameObject t, GameStateMachine s) : base(t, s)
    {
        target = t;
        sm = s;

    }

    //used so that I don't have to manually type out every time I cast.
    protected EnemyStateMachine esm()
    {
        return (EnemyStateMachine)sm;
    }
}
