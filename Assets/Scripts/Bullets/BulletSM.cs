using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Default state machine class for bullet movement
public class BulletSM : GameStateMachine
{
    protected bool started; //if set to false, don't use update methods.
    public void StartBullet() {
        //Debug.Log("Started.");    
        started = true; 
    } //call in cannon/weapon script after inputting transform parameters

    //References to various components of the prefab
    public BulletData bdata;
    protected SpriteRenderer sprite;
    public Rigidbody2D rb;
    protected Vector2 dir;
    public Collider2D col;

    //Particle control
    public TrailRenderer trail;
    public ParticleSystem startParticle;
    public ParticleSystem hitParticle;
    public ParticleSystem ricochetParticle;
    const float trailAlpha = 0.5f;

    //States
    public BulletStartState startState;
    public BulletStdState stdState;
    public BulletDestroyState destState;
    public BulletHitState hitState;
    public BulletBounceState bounceState;

    //Miscellaneous vars
    protected float timer = 0; //pass this for lifespan states
    public int sourcePlayerId=0;
    public LevelManager mgmt;

    protected void Awake()
    {
        started = false; //assume not started

        //Initialize visual components from bullet data
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = bdata.sprite;
        sprite.color = bdata.element.primary;


        //TO DO: CONDITIONAL ASSIGNMENT if variables aren't publicly initialized, perform certain steps.

        stdState = new BulletStdState(gameObject, this, bdata);
        destState = new BulletDestroyState(gameObject, this, bdata);
        hitState = new BulletHitState(gameObject, this, bdata);
        bounceState = new BulletBounceState(gameObject, this, bdata);

        //Disable particle effects. Reenable them in appropriate calls to OnEnter for states
        if (trail) { 
            //trail.enabled = false;
            trail.startColor = new Color(bdata.element.primary.r, bdata.element.primary.g, bdata.element.primary.b, trailAlpha);
            trail.endColor = new Color(1,1,1, trailAlpha);
        }
        if (startParticle) { startParticle.Stop(); }
        if (hitParticle) { hitParticle.Stop(); }
        if (ricochetParticle) { ricochetParticle.Stop(); }

    }


    public void SetBulletData(BulletData b)
    {
        bdata = b;
        sprite.sprite = bdata.sprite;
        sprite.color = bdata.element.primary;
        if (trail)
        {
            //trail.enabled = false;
            trail.startColor = new Color(bdata.element.primary.r, bdata.element.primary.g, bdata.element.primary.b, trailAlpha);
            trail.endColor = new Color(1, 1, 1, trailAlpha);
        }
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

    public void SetHitboxActive(bool val)
    {
        col.enabled = val;
    }

    public float GetTimer() { return timer; }
    public void SetTimer(float v) { timer = v; }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EnemyStateMachine esm = collision.gameObject.GetComponent<EnemyStateMachine>();
            if (bdata.element.Equals(esm.data.element))
            {
                //Debug.Log("Successful hit");
                ChangeState(hitState);
            }
            else
            {
                //Debug.Log("Wrong weapon");
                ChangeState(bounceState);
            }
        }
    }

}
