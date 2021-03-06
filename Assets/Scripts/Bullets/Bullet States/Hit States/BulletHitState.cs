using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 State for when bullet successfuly damages opponent
 */

public class BulletHitState : BulletState
{
    float fadeTimer = 0;

    public BulletHitState(GameObject t, GameStateMachine s, BulletData bdata) : base(t, s, bdata)
    {
    }


    public override void OnEnter()
    {
        base.OnEnter();
        //((BulletSM)_sm).SetHitboxActive(false);
        fadeTimer = ((BulletSM)_sm).bdata.destroyTime;
        rb.velocity = new Vector2(); //set velocity to zero
        //_target.GetComponent<SpriteRenderer>().enabled = false; //replace with destroy timer
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        fadeTimer = Mathf.Max(fadeTimer-Time.deltaTime,0);
        Color color = ((BulletSM)_sm).sprite.color;
        ((BulletSM)_sm).sprite.color = new Color(color.r,color.g,color.b,fadeTimer/((BulletSM)_sm).bdata.destroyTime);
        if (fadeTimer <= 0)
        {
            ((BulletSM)_sm).ChangeState(((BulletSM)_sm).destState);
        }
    }



}
