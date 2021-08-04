using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimState : PlayerState
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
    protected int lastLockOnPressed=0;

    //constructor takes _target game object, state machine running it, controller configuration, and arm gameobject for aiming
    public PlayerAimState(GameObject t, PlayerSM playerSM, IControllerInput c, GameObject a, GameObject g) : base(t, playerSM,c)
    {
        this._target = t;
        //Debug.Log("In stand state: " + _target);
        this._sm = playerSM;
        rb = _target.GetComponent<Rigidbody2D>();
        arm = a;
        gun = g;
        enemyList = new List<GameObject>();
        lockOnResetLimit = playerSM.pparams.lockOnResetTime;
        lockResetTimer = lockOnResetLimit + 1;

    }


    public override void OnEnter()
    {
        base.OnEnter();
        rolltimer = ((PlayerSM)_sm).GetRollTimer();
        lockedOn = ((PlayerSM)_sm).GetLockedOn();
        
        //Get Lockon Parameters
        enemyList = ((PlayerSM)_sm).GetLockOnList();
        lockId = ((PlayerSM)_sm).GetLockOnId();
        lockTimer = ((PlayerSM)_sm).GetLockTimer();
        lockResetTimer = ((PlayerSM)_sm).GetLockOnResetTimer();
        lastLockOnPressed = ((PlayerSM)_sm).GetLastLockOnPressed();
    }

    public override void OnExit()
    {
        base.OnExit();
        ((PlayerSM)_sm).SetRollTimer(rolltimer);
        ((PlayerSM)_sm).SetLockedOn(lockedOn,enemyList,lockId);
        ((PlayerSM)_sm).SetLockTimer(lockTimer);
        ((PlayerSM)_sm).SetLockOnResetTimer(lockResetTimer);
        ((PlayerSM)_sm).SetLastLockOnPressed(lastLockOnPressed);
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
           if (lockId > -1 && enemyList.Count > 0)
                {
                    //Debug.Log("Lock id = " + lockId + " Count = " + enemyList.Count);
                    if (enemyList[lockId])
                {
                    arm.transform.rotation = Quaternion.Euler(0, 0, GetEnemyAngle(enemyList[lockId]));
                    aimdir = new Vector2(enemyList[lockId].transform.position.x - _target.transform.position.x, _target.transform.position.y - enemyList[lockId].transform.position.y).normalized; //update stored aim direction so that cannon remains facing the enemy once lockon ends

                }
                    else
                {
                    lockedOn = false;
                    gun.GetComponent<WeaponScript>().ResetReticle();
                }
            }
            
        }

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
    
    //TODO
    //Construct array of aim-able enemies and sort by distance 
    //Call in moderation since FindGameObjectsWithTag is slooooooow
    //Consider refactoring, if there's another object keeping the enemy list
    protected float GetEnemyAngle(GameObject g)
    {
        if (!g) { return 0; }
        float result;
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
        return (_target.transform.position - g.transform.position).magnitude;
    }

    //returns a priority score representing for a given enemy, re: which enemy to aim at first
    protected float EnemySortHeuristic(GameObject enemy)
    {
        float distanceScore = GetEnemyDistance(enemy) * ((PlayerSM)_sm).pparams.aimingHeuristicEnemyDistanceWeight;
        float aimingScore = GetEnemyAngleError(enemy)* ((PlayerSM)_sm).pparams.aimingHeuristicAngleErrorWeight;
        float result= aimingScore-distanceScore;
        //Debug.Log("Heuristic score for " + enemy.name + ": " + result);
        return result;
    }

    //Makes list of enemies that you can lock onto and sorts them by priority.
   public void MakeEnemyList(bool sort)
    {
         /*
         * Since this might be called while we're locked on, 
         * we don't want to lose track of the enemy we're currently locked onto.
         * However, if the enemy list is reconstructed, the index of the enemy we're locked onto,
         * in the enemyList array, might have changed.
         * If lockedOn is true, iterate through the new enemy list,
         * and find the new lockon index of the _target you're already locked onto.
         * If no such index exists, then the enemy was destroyed, and you should lock onto a new _target.
         */

        GameObject currentLockOnTarget=null;
        if (enemyList.Count > 0)
        {
            currentLockOnTarget = enemyList[lockId];
        }
        Dictionary<int, GameObject> candidates = ((PlayerSM)_sm).levelManager.GetEnemyList();           
        enemyList.Clear(); //reset enemy array

        //iterate through candidates and construct lock-on list, assuming they're within lock-on range
        foreach (KeyValuePair<int,GameObject> can in candidates)
        {
            //Check whether enemy candidate is within desired aiming range
            bool withinAngleError = GetEnemyAngleError(can.Value) <= ((PlayerSM)_sm).pparams.maxAngleError;

            //If using a switch lock-on type, let any enemy within firing distance be included, even if you're not aiming that direction.
            if (cont.GetLockOnType() == LockOnType.SWITCH)
            {
                withinAngleError = true;
            }

            //populate list of enemies you can lock onto based on above criteria
            if (
                    withinAngleError &&
                    GetEnemyDistance(can.Value) <= ((PlayerSM)_sm).pparams.maxEnemyDistanceForLockOn
               )
            {
                enemyList.Add(can.Value);
            }

        }
        
        //Sort enemy list to determine best initial _target
        if (sort)
        {
            enemyList.Sort((IComparer<GameObject>)Comparer<GameObject>.Create((i1, i2) => this.EnemySortHeuristic(i2).CompareTo(this.EnemySortHeuristic(i1)))); //sort list, lowest score first
        }
             
        //iterate through new enemy list to stay locked onto the same enemy
        if (lockedOn && currentLockOnTarget != null)
        {
            bool targetFound = false;
            for (int i = 0; i<enemyList.Count; i++)
            {
                if (enemyList[i] == currentLockOnTarget)
                {
                    lockId = i;
                    targetFound = true;
                }
            }
            if (!targetFound)
            {

                if (enemyList.Count == 0)
                {
                    lockedOn = false;
                    lockResetTimer = 0;
                    gun.GetComponent<WeaponScript>().ResetReticle();
                }
                else
                {
                    lockId = 0;
                    gun.GetComponent<WeaponScript>().SetReticleTarget(enemyList[lockId]);
                }

            }
        }
        if (enemyList.Count == 0)
        {
            lockedOn = false;
            lockResetTimer = 0;
            gun.GetComponent<WeaponScript>().ResetReticle();
        }
    }



    //TODO
    //Change _target if on hold mode
    protected void ChangeHoldLockOnTarget()
    {
             
            if (cont.GetLockOnType() == LockOnType.HOLD && enemyList.Count>0)
            {

                lockId = (lockId + 1) % enemyList.Count;
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
        
        
        //Run when locked on
        if (lockedOn)
        {
            //Check for switching lock-on _target
            if (cont.GetLockOnType() == LockOnType.SWITCH)
            {
                int change = cont.GetSwitchAxis(); //get status of "Change Target" button press
                
                //If there's at least one enemy, and the "Change Target" button status has changed
                if (
                    enemyList.Count > 0 &&
                    lastLockOnPressed != change
                    )
                {
                    //Debug.Log("Changed lockon state");

                    if (change != 0) {
                        //Change lockId number
                        lockId = (lockId + change);
                        if (lockId < 0)
                        {
                            lockId = enemyList.Count - 1;
                        }
                        lockId = lockId % enemyList.Count;

                        //Debug.Log("LockOn Id " + lockId + " locked onto: " + enemyList[lockId].name);
                        gun.GetComponent<WeaponScript>().SetReticleTarget(enemyList[lockId]);
                    }

                    
                   
                    lastLockOnPressed = change;
                }
                else if (enemyList.Count == 0)
                {
                    gun.GetComponent<WeaponScript>().ResetReticle();
                    lockedOn = false;
                    lockResetTimer = 0; //start the lockon reset timer
                }
            }

            //if lock-on button no longer held, or if lock cancel button is pressed, quit locking on
            if (
                (!cont.GetButton(ButtonID.LOCK_ON) && cont.GetLockOnType() == LockOnType.HOLD) ||
                cont.GetButtonDown(ButtonID.CANCEL_LOCK_ON) ||
                enemyList.Count == 0
                )
            {
                
                    gun.GetComponent<WeaponScript>().ResetReticle();
                    lockedOn = false;
                    lockResetTimer = 0; //start the lockon reset timer
            }

        }

        //Check for lock-on presses
        else {

            gun.GetComponent<WeaponScript>().ResetReticle();
            if (cont.GetButtonDown(ButtonID.LOCK_ON))
            {
                
                    MakeEnemyList(true);
                    ChangeHoldLockOnTarget();
                    if (enemyList.Count > 0)
                    {
                        lockedOn = true;
                        gun.GetComponent<WeaponScript>().SetReticleTarget(enemyList[lockId]);
                    }
                
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
            rolltimer >= ((PlayerSM)_sm).pparams.rolldelay &&
            movedir.magnitude > 0
            )
        {
            rolltimer = 0;
            gun.GetComponent<WeaponScript>().Unpress();
            rollpress = false; //reset roll button when changing state
            ((PlayerSM)_sm).SetLastPress(movedir); //feed into roll state
            ((PlayerSM)_sm).ChangeState(((PlayerSM)_sm).rollState);
        }
        else
        {
            //update roll timer
            rolltimer = Mathf.Min(((PlayerSM)_sm).pparams.rolldelay, rolltimer + Time.deltaTime);
        }
    }

    public override void LogicUpdate()
    {
        //Debug.Log("Rolltimer: " + rolltimer);
        base.LogicUpdate();
        
        if (!lockedOn)
        {
            if (lockResetTimer < lockOnResetLimit) { lockResetTimer += Time.deltaTime; } //reset aim after a few seconds of not locking on to anything
            else { lockId = 0; }
        }
       
        
        AimCannon();


    
        
    }


  
}
