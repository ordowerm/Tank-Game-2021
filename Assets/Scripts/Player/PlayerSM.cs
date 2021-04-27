using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSM : InputStateMachine
{

    public enum AnimationNumbers
    {
        STAND = 0, WALK, ROLL
    }

    public Animator anim;
    public PlayerParameters pparams;
    public GameObject arm;
    public GameObject gun;
    public Collider2D[] hitboxes;

    
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


    // Start is called before the first frame update
    void Start()
    {
        walkState = new PlayerWalkState(this.gameObject, this,keyconfig,arm,gun);
        standState = new PlayerStandState(this.gameObject, this,keyconfig,arm,gun);
        rollState = new PlayerRollState(this.gameObject, this,keyconfig);

        Initialize(startState: standState);
    }


    //Animation state updates
    public void SetAnimationState(AnimationNumbers an){
        Debug.Log("Setting animation: " + an);
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
        Debug.Log(anim.GetCurrentAnimatorStateInfo(0).length);
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
       
}
