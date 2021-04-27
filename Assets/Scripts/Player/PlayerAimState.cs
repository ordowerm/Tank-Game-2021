using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimState : InputState
{
    protected Vector2 movedir = new Vector2(0, 0);
    protected Vector2 aimdir = new Vector2(0, 0);

    protected float rolltimer;
    protected bool rollpress = false;

    protected Rigidbody2D rb;
    protected GameObject arm;
    protected GameObject gun;

    //Parameters for locking onto an enemy
    protected bool lockedOn=false;
    protected List<GameObject> enemyList;
    protected int lockId=-1; //index of current enemy number
    protected float lockTimer; //once this timer surpasses PlayerParameters.lockOnTime, then lock-on is cancelled.

    //constructor takes target game object, state machine running it, controller configuration, and arm gameobject for aiming
    public PlayerAimState(GameObject t, PlayerSM playerSM, IControllerInput c, GameObject a, GameObject g) : base(t, playerSM,c)
    {
        this.target = t;
        //Debug.Log("In stand state: " + target);
        this.sm = playerSM;
        rb = target.GetComponent<Rigidbody2D>();
        arm = a;
        gun = g;
    }


    public override void OnEnter()
    {
        base.OnEnter();
        rolltimer = ((PlayerSM)sm).GetRollTimer();
    }

    public override void OnExit()
    {
        base.OnExit();
        ((PlayerSM)sm).SetRollTimer(rolltimer);
    }

    //After calling Handle Input, 
    protected void AimCannon()
    {
        bool gamepad = cont.IsController();
        //Debug.Log("Is Controller? "+gamepad);
        if (!lockedOn)
        {
            if (gamepad)
            {
                if (aimdir.x == 0 && aimdir.y != 0)
                {
                    if (aimdir.y < 0)
                    {
                        arm.transform.rotation = Quaternion.Euler(0, 0, 90);
                    }
                    else
                    {
                        arm.transform.rotation = Quaternion.Euler(0, 0, 270);
                    }
                    return; //if there's no horizontal component to aiming, stop here 
                }

                if (aimdir.x != 0)
                {
                    arm.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(aimdir.y, -aimdir.x) * 180.0f / Mathf.PI + 180.0f); //set rotation of cannon
                }


            }
            else
            {
                //ADJUST MOUSE

            }
        }
        else
        {
            //IMPLEMENT LOCK-ON HERE
            try
            {
                Transform ot = enemyList[lockId].transform;
                float deltaX = ot.position.x - arm.transform.position.x;
                float deltaY = ot.position.y - arm.transform.position.y;
                
                //if directly above/below target, aim toward it
                if (deltaX == 0)
                {
                    if (deltaY < 0)
                    {
                        arm.transform.rotation = Quaternion.Euler(0, 0, 90);
                    }
                    else
                    {
                        arm.transform.rotation = Quaternion.Euler(0, 0, 270);
                    }
                }
                else
                {
                    arm.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(deltaY, -deltaX) * 180.0f / Mathf.PI + 180.0f); //set rotation of cannon
                }
            }
            catch (System.IndexOutOfRangeException)
            {
                Debug.LogError("Aiming at a nonexistent enemy.");
            }
        }

        
    }
    
    //TODO
    //Construct array of aim-able enemies and sort by distance 
    protected void MakeEnemyList()
    {

    }

    //TODO
    //Change target
    protected void ChangeLockOnTarget()
    {

    }


    public override void HandleInput()
    {
        base.HandleInput();
        movedir = cont.GetAxis();
        Vector2 comp = cont.GetAim();
        if (
            (comp.x != aimdir.x) ||
            (comp.y !=aimdir.y) &&
            !(comp.x == 0 && comp.y ==0)
            )
        {
            aimdir = comp;
            AimCannon();
        }
        if (cont.GetButtonDown(ButtonID.SHOT))
        {
            gun.GetComponent<WeaponScript>().Press();
        }
        if (cont.GetButtonUp(ButtonID.SHOT))
        {
            gun.GetComponent<WeaponScript>().Unpress();
        }

        //check if roll button is pressed
        rollpress = cont.GetButtonDown(ButtonID.ROLL);
    }

    protected void CheckRoll()
    {
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
        }
    }

    public override void LogicUpdate()
    {
        //Debug.Log("Rolltimer: " + rolltimer);
        base.LogicUpdate();
        //mirror gun vertically depending on Euler angle of arm.
        float rotvalue = arm.transform.eulerAngles.z;
        if (
            rotvalue > -90 && rotvalue < 90 ||
            rotvalue > 270 && rotvalue < 360
            )
        {
            gun.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            gun.transform.localScale = new Vector3(1, -1, 1);
        }
    
        
    }

}
