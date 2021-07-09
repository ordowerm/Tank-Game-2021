using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroyState : BulletState
{
    public BulletDestroyState(GameObject t, GameStateMachine s, BulletData bdata) : base(t, s, bdata)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
        //Debug.Log("DESTRUCTION.");
        GameObject.Destroy(target);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        OnExit();
    }

}
