using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : GameState
{
    public EnemyState(GameObject t, GameStateMachine s) : base(t, s)
    {
        target = t;
        sm = s;

    }
}
