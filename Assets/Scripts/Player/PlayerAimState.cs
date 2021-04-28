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
    protected float lockResetTimer; //once this number reaches the lockOnResetLimit, set lockId to 0
    protected float lockOnResetLimit;

    //constructor takes target game object, state machine running it, controller configuration, and arm gameobject for aiming
    public PlayerAimState(GameObject t, PlayerSM playerSM, IControllerInput c, GameObject a, GameObject g) : base(t, playerSM,c)
    {
        this.target = t;
        //Debug.Log("In stand state: " + target);
        this.sm = playerSM;
        rb = target.GetComponent<Rigidbody2D>();
        arm = a;
        gun = g;
        enemyList = new List<GameObject>();
        lockOnResetLimit = playerSM.pparams.lockOnResetTime;
        lockResetTimer = lockOnResetLimit + 1;

    }


    public override void OnEnter()
    {
        base.OnEnter();
        rolltimer = ((PlayerSM)sm).GetRollTimer();
        lockedOn = ((PlayerSM)sm).GetLockedOn();
        //enemyList = ((PlayerSM)sm).GetLockOnList();
        lockId = ((PlayerSM)sm).GetLockOnId();
        lockTimer = ((PlayerSM)sm).GetLockTimer();
        lockResetTimer = ((PlayerSM)sm).GetLockOnResetTimer();
    }

    public override void OnExit()
    {
        base.OnExit();
        ((PlayerSM)sm).SetRollTimer(rolltimer);
        ((PlayerSM)sm).SetLockedOn(lockedOn,enemyList,lockId);
        ((PlayerSM)sm).SetLockTimer(lockTimer);
        ((PlayerSM)sm).SetLockOnResetTimer(lockResetTimer);
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
                if (lockId > -1 && enemyList.Count > 0)
                {
                    Debug.Log("Lock id = " + lockId + " Count = " + enemyList.Count);
                    arm.transform.rotation = Quaternion.Euler(0, 0, GetEnemyAngle(enemyList[lockId]));
                    aimdir = new Vector2(enemyList[lockId].transform.position.x - target.transform.position.x,target.transform.position.y-enemyList[lockId].transform.position.y).normalized; //update stored aim direction so that cannon remains facing the enemy once lockon ends

                }
                /*Transform ot = enemyList[lockId].transform;
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
                }*/
            }
            catch (System.IndexOutOfRangeException)
            {
                Debug.LogError("Aiming at a nonexistent enemy.");
            }
        }

        
    }
    
    //TODO
    //Construct array of aim-able enemies and sort by distance 
    //Call in moderation since FindGameObjectsWithTag is slooooooow
    //Consider refactoring, if there's another object keeping the enemy list
    protected float GetEnemyAngle(GameObject g)
    {
        float result = 0;
        float deltaX = g.transform.position.x - arm.transform.position.x;
        float deltaY = g.transform.position.y - arm.transform.position.y;
        if (deltaX == 0)
        {
            if (deltaY > 0) { result= 90; }
            else { result= 270; }
        }
        else
        {
            result= Mathf.Atan2(-deltaY, -deltaX) * 180.0f / Mathf.PI + 180.0f;
        }
        //Debug.Log("Arm: " + arm.transform.rotation.eulerAngles.z + "; Enemy: " + result);
        return result;
    }
    
    //returns error between aiming angle and enemy angle relative to player
    protected float GetEnemyAngleError(GameObject g)
    {
        float enemyAngle = GetEnemyAngle(g);
        //calculate error, accounting for 360 being equivalent to 0. Return minimum of two error calculations.
        float error1 = Mathf.Abs(enemyAngle - arm.transform.rotation.eulerAngles.z);
        float error2 = Mathf.Abs(enemyAngle - 360 - arm.transform.rotation.eulerAngles.z);
        return Mathf.Min(error1, error2);
    }

    //gets distance between player and enemy
    protected float GetEnemyDistance(GameObject g)
    {
        return (target.transform.position - g.transform.position).magnitude;
    }

    //returns a priority score representing for a given enemy, re: which enemy to aim at first
    protected float EnemySortHeuristic(GameObject enemy)
    {
        float distanceScore = GetEnemyDistance(enemy) * ((PlayerSM)sm).pparams.aimingHeuristicEnemyDistanceWeight;
        float aimingScore = GetEnemyAngleError(enemy)* ((PlayerSM)sm).pparams.aimingHeuristicAngleErrorWeight;
        float result= aimingScore-distanceScore;
        //Debug.Log("Heuristic score for " + enemy.name + ": " + result);
        return result;
    }

    //Makes list of enemies that you can lock onto and sorts them by priority.
    protected void MakeEnemyList()
    {
        GameObject[] candidates = GameObject.FindGameObjectsWithTag("Enemy");
        enemyList.Clear(); //reset enemy array
        for (int i =0; i< candidates.Length; i++)
        {
            if (
                    GetEnemyAngleError(candidates[i])<= ((PlayerSM)sm).pparams.maxAngleError &&
                    GetEnemyDistance(candidates[i]) <= ((PlayerSM)sm).pparams.maxEnemyDistanceForLockOn
               )
            {
                enemyList.Add(candidates[i]);
            }
        }
        //debug sorting alg
        string liststring = "";
        foreach (GameObject g in enemyList){
            liststring += g.name + "   ";
        }
        Debug.Log("Unsorted list: " + liststring);
        
        enemyList.Sort((IComparer<GameObject>)Comparer<GameObject>.Create((i1, i2) => this.EnemySortHeuristic(i2).CompareTo(this.EnemySortHeuristic(i1)))); //sort list, lowest score first

        //debug sorting alg
        liststring = "";
        foreach (GameObject g in enemyList)
        {
            liststring += g.name + "   ";
        }
        Debug.Log("Sorted list: " + liststring);
    }

    //TODO
    //Change target
    protected void ChangeLockOnTarget()
    {
        if (enemyList.Count > 0)
        {
            if (lockResetTimer >= lockOnResetLimit)
            {
                lockId = -1;
            }
            lockId++;
            lockId = lockId % enemyList.Count;
        }
    }


    public override void HandleInput()
    {
        base.HandleInput();
        movedir = cont.GetAxis();

        //If player isn't locked on, get the analog axes of player aiming, compare it to current aiming direction, and update.
        if (!lockedOn) {
            Vector2 comp = cont.GetAim();
            if (
            (comp.x != aimdir.x) ||
            (comp.y != aimdir.y) &&
            !(comp.x == 0 && comp.y == 0)

            )
            {
                aimdir = comp; //update aiming direction
            }
        }


        //Check for shot button presses
        if (cont.GetButtonDown(ButtonID.SHOT))
        {
            gun.GetComponent<WeaponScript>().Press();
        }     
        if (cont.GetButtonUp(ButtonID.SHOT))
        {
            gun.GetComponent<WeaponScript>().Unpress();
        }
        
        
        //Check for lock-on presses
        if (cont.GetButtonDown(ButtonID.LOCK_ON))
        {
            if (!lockedOn)
            {
                MakeEnemyList();
                ChangeLockOnTarget();
                if (enemyList.Count > 0)
                {
                    lockedOn = true;
                    gun.GetComponent<WeaponScript>().SetReticleTarget(enemyList[lockId]);
                }
            }
            
        }
        //if lock-on button no longer held, quit locking on
        if (!cont.GetButton(ButtonID.LOCK_ON))
        {
            if (lockedOn) { 
                gun.GetComponent<WeaponScript>().ResetReticle();
                lockedOn = false;
                lockResetTimer = 0;
            }

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
        if (!lockedOn && lockResetTimer < lockOnResetLimit) { lockResetTimer += Time.deltaTime; } //reset aim after a few seconds of not locking on to anything
        AimCannon();

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
