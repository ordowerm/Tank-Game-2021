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
    protected GameObject reticle=null; //current implementation only allows one reticle to be aiming at this dude. refactor

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hitboxes = GetComponentsInChildren<Collider2D>();
    }

    //PLACEHOLDER STUFF TO TEST LEVEL MANAGER
    protected override void Update()
    {
        //base.Update();
        

    }

    protected override void FixedUpdate()
    {
        //base.FixedUpdate();
    }

    protected void OnDestroy()
    {
        //Ensures that the reticle object won't be destroyed along with this.gameObject
        if (reticle)
        {
            reticle.transform.parent = null;
        }
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
            levelManager.NotifyEnemyDestroyed(this.id);
            Destroy(gameObject);
        }
    }
}
