
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : EnemyState
{
    float iFrameTimer=0;
    Rigidbody2D rb;

    public EnemyHit(GameObject t, GameStateMachine s) : base(t, s)
    {
       rb = target.GetComponent<Rigidbody2D>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        iFrameTimer = 0;
        rb.velocity *= 0;
        ((EnemyStateMachine)sm).SetAnimatorTimeScale(0);
    }

    public override void OnExit()
    {
        base.OnExit();
        iFrameTimer = 0;
        Animate(false);
        ((EnemyStateMachine)sm).SetAnimatorTimeScale(1);

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        iFrameTimer += Time.deltaTime;
        Animate(true);
        //Debug.Log(iFrameTimer);
        if (iFrameTimer> ((EnemyStateMachine)sm).data.iFrameTime)
        {
            ((EnemyStateMachine)sm).ChangePrevious();
        }
    }

    protected void Animate(bool val)
    {
        float prop = Mathf.Sin(Mathf.PI * (iFrameTimer / ((EnemyStateMachine)sm).data.iFrameTime)); //reminder: sin(0) = 0, sin(pi) = 0, sin(pi/2) = 1.
        ((EnemyStateMachine)sm).DamageShader(val, prop);
    }

}
