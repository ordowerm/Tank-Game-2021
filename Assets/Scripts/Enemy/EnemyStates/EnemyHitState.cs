
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitState : EnemyState
{
    float iFrameTimer=0;
    Rigidbody2D rb;
    Vector2 storedVelocity;

    public EnemyHitState(GameObject t, GameStateMachine s) : base(t, s)
    {
       rb = _target.GetComponent<Rigidbody2D>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        iFrameTimer = 0;
        storedVelocity = rb.velocity;
        rb.velocity *= 0;
        ((EnemyStateMachine)_sm).SetAnimatorTimeScale(0);
    }

    public override void OnExit()
    {
        base.OnExit();
        iFrameTimer = 0;
        Animate(false);
        ((EnemyStateMachine)_sm).SetAnimatorTimeScale(1); //continue animation
        rb.velocity = storedVelocity; //return enemy to previous velocity
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        iFrameTimer += Time.deltaTime;
        Animate(true);
        //Debug.Log(iFrameTimer);
        if (iFrameTimer> ((EnemyStateMachine)_sm).data.iFrameTime)
        {
            if (((EnemyStateMachine)_sm).GetHP() > 0)
            {
                ((EnemyStateMachine)_sm).ChangePrevious();
            }
            else
            {
                ((EnemyStateMachine)_sm).SpawnExplosion();
                GameObject.Destroy(_sm.gameObject);

            }
        }
    }

    //Animates the color shifting for each of the sprites
    protected void Animate(bool val)
    {
        float prop = Mathf.Sin(Mathf.PI * (iFrameTimer / ((EnemyStateMachine)_sm).data.iFrameTime)); //reminder: sin(0) = 0, sin(pi) = 0, sin(pi/2) = 1.
        ((EnemyStateMachine)_sm).DamageShader(val, prop);
    }

}
