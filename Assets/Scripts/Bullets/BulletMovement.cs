using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Default state machine class for bullet movement
//[System.Serializable()]
public class BulletMovement : GameStateMachine
{
    protected bool started; //if set to false, don't use update methods.
    public void StartBullet() {
        //Debug.Log("Started.");    
        started = true; 
    } //call in cannon/weapon script after inputting transform parameters

    public BulletData bdata;
    protected SpriteRenderer sprite;
    public Rigidbody2D rb;
    protected Vector2 dir;

    //States
    public BulletStartState startState;
    public BulletStdState stdState;
    public BulletDestroyState destState;
    public BulletHitState hitState;
    public BulletBounceState bounceState;

    protected void Awake()
    {
        started = false; //assume not started

        //Initialize visual components from bullet data
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = bdata.element.primary;


        //TO DO: CONDITIONAL ASSIGNMENT if variables aren't publicly initialized, perform these steps.

        stdState = new BulletStdState(gameObject, this, bdata);
        destState = new BulletDestroyState(gameObject, this, bdata);
        hitState = new BulletHitState(gameObject, this, bdata);
        bounceState = new BulletBounceState(gameObject, this, bdata);
    }

    
    public void SetBulletData(BulletData b)
    {
        bdata = b;
        sprite.sprite = bdata.sprite;
        sprite.color = bdata.element.primary;
    }

    public void SetInitialDirection(float rot)
    {
        //Debug.Log("Rotation: " + rot);
        float x = Mathf.Cos(rot * Mathf.PI / 180.0f);
        float y = Mathf.Sin(rot * Mathf.PI / 180.0f);
        //Debug.Log(new Vector2(x, y));
        dir = new Vector2(x,y);
    }

    public Vector2 GetInitialDirection()
    {
        return dir;
    }

    protected override void Update()
    {
        if (started)
        {
            base.Update();
        }
    }

    protected override void FixedUpdate()
    {
        if (started) { base.FixedUpdate(); }
    }
}
