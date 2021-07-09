using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletState : GameState
{
    protected BulletData bd;
    public TrailRenderer trail;
    public ParticleSystem startParticle;
    public ParticleSystem endParticle;
    protected Rigidbody2D rb;
    protected float timer;

    public BulletState(GameObject t, GameStateMachine s, BulletData bdata) : base(t, s)
    {
        bd = bdata;
        timer = 0;
        rb = ((BulletSM)sm).rb;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        timer = ((BulletSM)sm).GetTimer();
    }

    public override void OnExit()
    {
        base.OnExit();
        ((BulletSM)sm).SetTimer(timer);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        timer += Time.deltaTime;
        //If at end of lifespan, switch to the destroy state
        if (timer > bd.lifespan)
        {
            ((BulletSM)sm).ChangeState(((BulletSM)sm).destState);
            return;
        }
    }
}
