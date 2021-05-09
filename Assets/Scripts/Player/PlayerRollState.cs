using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollState : PlayerState
{
    protected Rigidbody2D rb;
    protected float rolltimer;
    protected float endtime;
    protected Vector2 initialDir; //initial direction. assign in OnEnter
    protected AnimationCurve curve;
    protected float speed;

    public PlayerRollState(GameObject t, PlayerSM playerSM, IControllerInput c):base(t, playerSM,c)
    {
        this.target = t;
        this.sm = playerSM;
        rb = t.GetComponent<Rigidbody2D>();
        endtime = ((PlayerSM)sm).pparams.rolltime;
        speed = ((PlayerSM)sm).pparams.rollspeed;
        curve = ((PlayerSM)sm).pparams.rollcurve;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        rolltimer = 0; //reset roll timer
        initialDir = ((PlayerSM)sm).GetLastPress().normalized;
        ((PlayerSM)sm).SetAnimationState(PlayerSM.AnimationNumbers.ROLL);
    }

    
    //returns animation time between 0 and 1, depending on direction of roll
    //the animation should play in reverse if player is moving left, so the time should be 1-t
    protected float GetAnimationTime(Vector2 init)
    {
        float prop = Mathf.Min(rolltimer / endtime, 1.0f); //proportion of time covered by roll
        if (init.x > 0)
        {
            return 1 - prop;
        }
        else
        {
            return prop;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        


        //change to walk state if moving
        if (rolltimer >= endtime)
        {
            Vector2 movedir = cont.GetAxis();
            if (movedir.magnitude > 0)
            {
                ((PlayerSM)sm).ChangeState(((PlayerSM)sm).walkState);
            }
            else
            {
                ((PlayerSM)sm).ChangeState(((PlayerSM)sm).standState);
            }
        }
        else
        {
            //update animation
            rolltimer = Mathf.Min(endtime,rolltimer + Time.deltaTime);
            ((PlayerSM)sm).SetAnimationTimer(GetAnimationTime(initialDir));
            
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        rb.velocity = speed * curve.Evaluate(GetAnimationTime(new Vector2(-1, 0)))*initialDir;
    }

    public override void OnExit()
    {
        base.OnExit();
        //rolltimer = 0;
        ((PlayerSM)sm).SetRollTimer(0);
    }
}
