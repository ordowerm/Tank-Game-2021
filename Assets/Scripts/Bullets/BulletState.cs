using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletState : State
{
    protected BulletData bd;

    public BulletState(GameObject t, GameStateMachine s, BulletData bdata) : base(t, s)
    {
        bd = bdata;
    }
}
