using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : GameStateMachine
{
    protected int id; //enemy id number assigned in LevelManager when spawning
    public void SetIDNumber(int i) { id = i; }
    protected Rigidbody2D rb;
    protected Collider2D[] hitboxes;
    public Animator anim;
    protected GameObject reticle=null; //current implementation only allows one reticle to be aiming at this dude. refactor later
    public EnemyData data;
    public SpriteRenderer[] sprites; //Reference to different sprite components. Modify shaders on each SpriteRenderer for certain VFX.

    //States
    public EnemyDormant dormant;
    public EnemyIdle idle;
    public EnemyApproach approach;
    public EnemyAttack attack;
    public EnemyHit hitState;

    //VFX
    protected float ricochetTimer = 0;


    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hitboxes = GetComponentsInChildren<Collider2D>();
        dormant = new EnemyDormant(this.gameObject, this);
        idle = new EnemyIdle(gameObject, this);
        approach = new EnemyApproach(gameObject, this);
        attack = new EnemyAttack(gameObject, this);
        hitState = new EnemyHit(gameObject, this);
        currentState = idle;
    }

    protected override void Update()
    {
        base.Update();
        

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected void OnDestroy()
    {
        //Ensures that the reticle object won't be destroyed along with this.gameObject
        if (reticle)
        {
            reticle.transform.parent = null;
        }
        levelManager.NotifyEnemyDestroyed(this.id);
    }

    //Call when reticle locks on
    public void RegisterReticle(GameObject ret)
    {
        reticle = ret;
    }
    public void UnregisterReticle()
    {
        if (reticle) { reticle = null; }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            if (!(currentState is EnemyHit)){
                ChangeState(hitState);
            }
            //levelManager.NotifyEnemyDestroyed(this.id);
            //Destroy(gameObject);
        }
    }

    public void ActivateHitboxes(bool activate)
    {
        foreach (Collider2D c in hitboxes)
        {
            c.enabled = activate;
        }
    }


    //takes proportion of damage shader active
    public void DamageShader(bool active, float prop)
    {
        //Debug.Log("Damage shader: " + active + prop);
        if (active)
        {
            foreach (SpriteRenderer spr in sprites)
            {
                Material m = spr.material;
                m.SetFloat("_DamageTrigger", 1);
                m.SetFloat("_DamageTimer", prop);
            }
        }
        else
        {
            foreach (SpriteRenderer spr in sprites)
            {
                Material m = spr.material;
                m.SetFloat("_DamageTrigger", 0);
                m.SetFloat("_DamageTimer", 0);
            }
        }

        
    }

    public void ResistShader(bool active, float prop)
    {
        //Debug.Log("Damage shader: " + active + prop);
        if (active)
        {
            foreach (SpriteRenderer spr in sprites)
            {
                Material m = spr.material;
                m.SetFloat("_ResistTrigger", 1);
                m.SetFloat("_ResistTimer", prop);
            }
        }
        else
        {
            foreach (SpriteRenderer spr in sprites)
            {
                Material m = spr.material;
                m.SetFloat("_ResistTrigger", 0);
                m.SetFloat("_ResistTimer", 0);
            }
        }
    }


    public void SetAnimatorTimeScale(float t)
    {
        anim.speed = t;
    }
}
