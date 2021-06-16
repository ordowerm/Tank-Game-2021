using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSM : InputStateMachine
{
    

    public enum AnimationNumbers
    {
        STAND = 0, WALK, ROLL
    }
    public int playerId;

    public Animator anim;
    public PlayerParameters pparams;
    public GameObject arm;
    public GameObject gun;
    public Collider2D[] hitboxes; //player hitboxes

    
    //State Names
    public PlayerWalkState walkState;
    public PlayerStandState standState;
    public PlayerRollState rollState;


    //Rolling parameters
    float rolltimer = 0;
    Vector2 lastpress=new Vector2(0,0); //use to obtain roll direction
    public void SetLastPress(Vector2 v)
    {
        lastpress = v;
    }
    public Vector2 GetLastPress() { return lastpress; }


    //Aiming/Lock-On parameters
    bool lockedOn=false;
    List<GameObject> lockOnList;
    float lockOnTimer;
    float lockOnResetTimer;
    int lockOnId=0;
    int lastLockOnPressed = 0;

    //Invincibility parameters
    float iFrameTimer = 0;


    //VFX params
    public GameObject dustParticle;


    // Start is called before the first frame update
    void Start()
    {
        lockOnList = new List<GameObject>();
        walkState = new PlayerWalkState(this.gameObject, this,keyconfig,arm,gun);
        standState = new PlayerStandState(this.gameObject, this,keyconfig,arm,gun);
        rollState = new PlayerRollState(this.gameObject, this,keyconfig);
        rollState.particle = dustParticle;
        hitboxes = GetComponentsInChildren<Collider2D>();
        Initialize(startState: standState);
    }


    //Animation state updates
    public void SetAnimationState(AnimationNumbers an){
        //Debug.Log("Setting animation: " + an);
        switch (an)
        {

            case AnimationNumbers.STAND:
                anim.SetInteger("animID", 0);
                break;
            case AnimationNumbers.WALK:
                anim.SetInteger("animID", 1);
                anim.speed = 1;
                break;
            case AnimationNumbers.ROLL:
                anim.SetInteger("animID", 2);
                break;
        }
        //Debug.Log(anim.GetCurrentAnimatorStateInfo(0).length);
    }
    public void SetAnimationTimer(float time)
    {
        anim.SetFloat("timer", time);
    }


    //Call when entering/exiting an AimState
    public float GetRollTimer()
    {
        return rolltimer;
    }
    public void SetRollTimer(float time)
    {
        rolltimer = time;
    }
    public bool GetLockedOn() { return lockedOn; }
    public void SetLockedOn(bool isLocked, List<GameObject> elist, int num)
    {
        lockedOn = isLocked;
        lockOnList = elist;
        lockOnId = num;
    }
    public void SetLockTimer(float t)
    {
        lockOnTimer = t;
    }
    public float GetLockTimer() { return lockOnTimer; }
    public List<GameObject> GetLockOnList() { return lockOnList; }
    public void SetLockOnList(List<GameObject> l) { lockOnList = l; }
    public int GetLockOnId() { return lockOnId; }
    public void SetLockOnId(int i) { lockOnId = i; }
    public float GetLockOnResetTimer() { return lockOnResetTimer; }
    public void SetLockOnResetTimer(float r)
    {
        lockOnResetTimer = r;
    }

    public int GetLastLockOnPressed() { return lastLockOnPressed; }
    public void SetLastLockOnPressed(int i) { lastLockOnPressed = i; }
    
    //Call in level manger when an enemy is destroyed
    public void SetEnemyPool() {
        if (currentState is PlayerAimState)
        {
            ((PlayerAimState)currentState).MakeEnemyList(true);
        }
    }

    //Call when invincibility frames start, end, etc.
    public void SetHitboxActive(bool val)
    {
        foreach (Collider2D c in hitboxes)
        {
            c.enabled = val;
        }
    }
    public float GetIFrameTimer() { return iFrameTimer; }
    public void SetIFrameTimer(float time) { iFrameTimer = time; }


    //Calls for weapon modification
    public void SetWeapon(WeaponScript.WeaponName wn) {
        WeaponScript wscript = gun.GetComponent<WeaponScript>();
        wscript.UpdateWeapon(wn);
        levelManager.levelUI.UpdateUIPaneWeaponDisplay(playerId, wscript.GetBulletData());
    }
}
