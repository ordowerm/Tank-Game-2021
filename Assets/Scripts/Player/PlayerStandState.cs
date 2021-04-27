using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandState : PlayerAimState
{

    public PlayerStandState(GameObject t, PlayerSM playerSM, IControllerInput c, GameObject a,GameObject g) : base(t, playerSM,c,a,g)
    {
        
     }


    public override void OnEnter()
    {
        base.OnEnter();
        rb.velocity = new Vector2(0, 0);
        ((PlayerSM)sm).SetAnimationState(PlayerSM.AnimationNumbers.STAND);
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (movedir.magnitude>0 && !rollpress)
        {
            //if (!cont.GetButton(ButtonID.ROLL))
            //{
                sm.ChangeState(((PlayerSM)sm).walkState);
            //}
            //else
            //{
               // sm.ChangeState(((PlayerSM)sm).rollState);
            //}
        }
        CheckRoll();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
