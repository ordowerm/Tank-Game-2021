using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBounceState : BulletState
{
    float fadeTimer = 0;

    public BulletBounceState(GameObject t, GameStateMachine s, BulletData bdata) : base(t, s, bdata)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        //((BulletSM)_sm).SetHitboxActive(false); //remove collider
        fadeTimer = ((BulletSM)_sm).bdata.ricochetTime;
        rb.velocity = ((BulletSM)_sm).GetInitialDirection();
        rb.velocity *= ((BulletSM)_sm).bdata.speed;

        //Debug.Log("In BulletBounce OnEnter: " + rb.velocity);
        //((BulletSM)_sm).trail.enabled = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        fadeTimer = Mathf.Max(fadeTimer - Time.deltaTime, 0);
        Color color = ((BulletSM)_sm).sprite.color;
        ((BulletSM)_sm).sprite.color = new Color(color.r, color.g, color.b, fadeTimer / ((BulletSM)_sm).bdata.ricochetTime);
        if (fadeTimer <= 0)
        {
            ((BulletSM)_sm).ChangeState(((BulletSM)_sm).destState);
        }
    }

}
