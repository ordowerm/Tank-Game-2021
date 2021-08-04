using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Standard bullet movement state. Implement specific movement patterns in Update methods.
public class BulletStdState : BulletState
{

    public BulletStdState(GameObject t, GameStateMachine s, BulletData bdata) : base(t, s, bdata)
    {
        rb = _target.GetComponent<Rigidbody2D>();
    }


    public override void OnEnter()
    {
        base.OnEnter();
        rb.velocity = ((BulletSM)_sm).GetInitialDirection();
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
