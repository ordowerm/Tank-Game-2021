using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerAimState
{
    float speed; //hardcoded movement speed placeholder
    
    public PlayerWalkState(GameObject t, PlayerSM playerSM, IControllerInput c, GameObject a,GameObject g):base(t,playerSM,c,a,g)
    {
        speed = ((PlayerSM)sm).pparams.walkspeed; //fetch walk speed from player parameters asset
    }

    public override void OnEnter()
    {
        //Debug.Log("in walk state");
        base.OnEnter();
        ((PlayerSM)sm).SetAnimationState(PlayerSM.AnimationNumbers.WALK);

    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (movedir.magnitude==0)
        {
            sm.ChangeState(((PlayerSM)sm).standState);
        }

        /*
        //if roll is available and pressed, start rolling
        if (
            rollpress &&
            rolltimer >= ((PlayerSM)sm).pparams.rolldelay &&
            movedir.magnitude > 0
            )
        {
            rolltimer = 0;
            gun.GetComponent<WeaponScript>().Unpress();
            rollpress = false; //reset roll button when changing state
            ((PlayerSM)sm).SetLastPress(movedir); //feed into roll state
            ((PlayerSM)sm).ChangeState(((PlayerSM)sm).rollState);
        }
        else
        {
            //update roll timer
            rolltimer = Mathf.Min(((PlayerSM)sm).pparams.rolldelay, rolltimer + Time.deltaTime);
        }*/
        CheckRoll();

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        rb.velocity = speed * movedir;

    }
}
