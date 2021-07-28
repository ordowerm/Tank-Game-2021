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
        //((BulletSM)sm).SetHitboxActive(false); //remove collider
        fadeTimer = ((BulletSM)sm).bdata.ricochetTime;
        rb.velocity = ((BulletSM)sm).GetInitialDirection();
        rb.velocity *= ((BulletSM)sm).bdata.speed;

        //Debug.Log("In BulletBounce OnEnter: " + rb.velocity);
        //((BulletSM)sm).trail.enabled = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        fadeTimer = Mathf.Max(fadeTimer - Time.deltaTime, 0);
        Color color = ((BulletSM)sm).sprite.color;
        ((BulletSM)sm).sprite.color = new Color(color.r, color.g, color.b, fadeTimer / ((BulletSM)sm).bdata.ricochetTime);
        if (fadeTimer <= 0)
        {
            ((BulletSM)sm).ChangeState(((BulletSM)sm).destState);
        }
    }

}
