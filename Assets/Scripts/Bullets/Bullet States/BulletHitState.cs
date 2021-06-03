using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 State for when bullet successfuly damages opponent
 */

public class BulletHitState : BulletState
{
    protected Collision2D col;
    protected float timer = 0;

    public BulletHitState(GameObject t, GameStateMachine s, BulletData bdata) : base(t, s, bdata)
    {
    }


    public override void OnEnter()
    {
        base.OnEnter();
        ((BulletSM)sm).SetHitboxActive(false);
        target.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void SetCollision(Collision2D c)
    {
        col = c;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        timer += Time.deltaTime;
        if (timer > bd.destroyTime)
        {
            ((BulletSM)sm).ChangeState(((BulletSM)sm).destState);
        }
    }

}
