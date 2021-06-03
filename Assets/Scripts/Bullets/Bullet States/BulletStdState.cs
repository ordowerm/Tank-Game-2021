using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Standard bullet movement state. Implement specific movement patterns in Update methods.
public class BulletStdState : BulletState
{
    protected float timer = 0;
    protected Rigidbody2D rb;

    public BulletStdState(GameObject t, GameStateMachine s, BulletData bdata) : base(t, s, bdata)
    {
        rb = target.GetComponent<Rigidbody2D>();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        timer += Time.deltaTime; //update timer

        //If at end of lifespan, switch to the destroy state
        if (timer > bd.lifespan)
        {
            ((BulletSM)sm).ChangeState(((BulletSM)sm).destState);
            return;
        }
    }
    public override void OnEnter()
    {
        base.OnEnter();
        rb.velocity = ((BulletSM)sm).GetInitialDirection();
        timer = 0;
        rb.velocity *= bd.speed;
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        MovementPath();
    }
    //Default implementation of this function assumes constant speed and linear path
    public void MovementPath() {
        //Debug.Log("Velocitas" + rb.velocity);

       
    }
}
