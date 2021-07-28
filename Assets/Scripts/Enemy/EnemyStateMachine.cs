using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyStateMachine : GameStateMachine
{
    protected int id; //enemy id number assigned in LevelManager when spawning --> this is used to track which enemies have been spawned / which enemies are active
    public enum Orientation
    {
        RIGHT, LEFT 
    }

    public void SetIDNumber(int i) { 
        //Debug.Log("Id number ="+ i); 
        id = i; }
    protected Rigidbody2D rb;
    public EnemyHitboxScript[] hitboxes;
    public Animator anim;
    protected GameObject reticle=null; //This is for a Player's reticle that gets locked on. Current implementation only allows one reticle to be aiming at this dude. refactor later
    public EnemyData data;
    public SpriteRenderer[] sprites; //Reference to different sprite components. Modify shaders on each SpriteRenderer for certain VFX.
    public GameObject explosionPrefab; //reference to the explosion prefab
    Orientation orientation;
    public void SetOrientation(Orientation o)
    {
        orientation = o;
    }
    public Orientation GetOrientation()
    {
        return orientation;
    }

    //Runtime variables
    int health;

    //States
    public EnemyDormant dormant;
    public EnemyIdle idle;
    public EnemyApproach approach;
    public EnemyAttack attack;
    public EnemyHitState hitState;

    //VFX
    protected float ricochetTimer = 0;
    bool resistCoroutineActive = false;

    //Getter/Setter
    public int GetHP() { return health; }


    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        CreateStates();
        currentState = idle;
        SetBodyMaterialColor(data.element.primary);
        health = data.maxHp;
    }

    //In EnemyStateMachine subclasses derived from this, override the CreateStates() method to use constructors for specific subclasses of the dormant, idle, etc. states.
    public virtual void CreateStates()
    {
        dormant = new EnemyDormant(this.gameObject, this);
        idle = new EnemyIdle(gameObject, this);
        approach = new EnemyApproach(gameObject, this);
        attack = new EnemyAttack(gameObject, this);
        hitState = new EnemyHitState(gameObject, this);
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
        /*
        //Ensures that the reticle object won't be destroyed along with this.gameObject
        if (reticle)
        {
            reticle.transform.parent = null;
        }
        levelManager.NotifyEnemyDestroyed(this.id);
        */
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


    //Called by the hitboxes when they collide with damaging objects
    public void NotifyHit(BulletSM bsm)
    {
        health -= bsm.bdata.power;
        
        //Update score
        if (health <= 0)
        {
            UpdateScore(bsm.sourcePlayerId, data.scoreForDestroyed);
            //Ensures that the reticle object won't be destroyed along with this.gameObject
            if (reticle)
            {
                reticle.transform.parent = null;
            }
            levelManager.NotifyEnemyDestroyed(this.id);
            ActivateHitboxes(false);
        }
        else
        {
            UpdateScore(bsm.sourcePlayerId, data.scoreForHit);
        }

        //Start hit state
        if (!(currentState is EnemyHitState)) //ignore hit state if you're already in a hit state
        {
            

            ChangeState(hitState);

        }
    }
    //Called by the hitboxes when they collide with a resisted bullet
    public void NotifyResist()
    {
        if (!(currentState is EnemyHitState))
        {
            if (!resistCoroutineActive)
            {
                resistCoroutineActive = true;
                StartCoroutine(ResistShaderCoroutine(data.ricochetTime)); 
            }
        }
    }

    //Activates the hitboxes for taking damage
    public void ActivateHitboxes(bool activate)
    {
        foreach (EnemyHitboxScript c in hitboxes)
        {
            if (activate)
            {
                c.Enable();
            }
            else
            {
                c.Disable();
            }
        }
    }


    //Shader manipulation functions
    //Helper function that sets the main material of the body's shader instance
    protected void SetBodyMaterialColor(Color c)
    {
        foreach (SpriteRenderer r in sprites)
        {
            if (r.tag.Equals("EnemyBodyTag")) 
            {
                //Debug.Log("body found");
                Material m = r.material;
                m.SetColor("_MainColor", c);
            }
        }
    }

    //This animates the enemy receiving damage
    public void DamageShader(bool active, float prop)
    {
       // Debug.Log("Damage shader: " + active + prop);
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
    //This animates the enemy resisting damage
    public void ResistShader(bool active, float prop)
    {
        //Debug.Log("Resist shader: " + active + prop);
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
    //Coroutine managing the resist shader stuff
    IEnumerator ResistShaderCoroutine(float period)
    {
        float lilDeltaTime = 0.05f; //timestep to use in this coroutine
        float resistShaderTimer = 0; //reset resist timer
        while (
            resistShaderTimer < period && 
            resistCoroutineActive && 
            currentState != hitState
            )

        {
            //Debug.Log("In coroutine: timer = " + resistShaderTimer);
            ResistShader(resistCoroutineActive, resistShaderTimer / period);
            resistShaderTimer += lilDeltaTime;
            yield return new WaitForSeconds(lilDeltaTime);
        }
        resistCoroutineActive = false;
        ResistShader(false, 0);
        StopCoroutine(ResistShaderCoroutine(period));

    }

    //Set the animation speed for certain effects like pausing when getting damaged, speeding up when accelerating, etc.
    public void SetAnimatorTimeScale(float t)
    {
        anim.speed = t;
    }

    //Spawns an instance of the explosion prefab
    /*
     Something to consider, as of 7/19/21:
     As currently constructed, we have to manually set the explosion prefab. We could, alternatively, programmatically generate it.
     */
    public void SpawnExplosion()
    {
               
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = transform.position;
        explosion.GetComponent<EnemyExplosionScript>().SetColorAndStart(data);
    }


    //Call in EnemyStates to check if the enemy is offscreen
    /*
     Returns a Vector2 result with the following values:
            x can be -1, 0, or 1, denoting whether the gameObject transform position is left, inside, or right, respectively of the Camera rectangle, in world space.
            y can be -1, 0, or 1, denoting whether the gameObject transform position is below, inside, or above the camera rectangle, in world space.


      Note: as of 7/18/21, this doesn't factor in sprite size, which means this only checks whether the origin of this gameObject is outside of the boundary walls.
     */
     public Vector2 CheckOnscreen()
    {
        Vector2 result = new Vector2(0, 0);
        float[] bounds = levelManager.GetBorders(); //returns min x, min y, max x, and max y coordinates, respectively
        
        //Set flag for x
        if (transform.position.x < bounds[0])
        {
            result.x = -1;
        }
        else if (transform.position.x > bounds[2])
        {
            result.x = 1;
        }

        //Set flag for y
        if (transform.position.y < bounds[1])
        {
            result.y = -1;
        }
        else if (transform.position.y > bounds[3])
        {
            result.y = 1;
        }

        return result;
    }


    //This sends a message to the LevelManager to update the score
    public void UpdateScore(int playerId, int points)
    {
        levelManager.IncrementPlayerScore(playerId, points);
    }
    

}
