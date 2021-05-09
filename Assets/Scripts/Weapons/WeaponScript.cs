using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public WeaponData wdata;
    public GameObject reticle;
    public GameObject bullet1;
    public GameObject bullet2;
    public float defaultReticleDistance; //distance from gun for reticle to spawn when in gamepad mode
    float shotTimer;
    float chargeTimer;
    bool pressed;
    bool useGamepad;

    //have the size of the reticle change when locked on
    public float reticleMinScale; //size when not locked on
    public float reticleMaxScale; //size when locked on


    protected void Awake()
    {
        shotTimer = 0;
        chargeTimer = 0;
        pressed = false;
        UpdateWeapon(this.wdata);
        
    }

    //Call this when changing weapons
    public void UpdateWeapon(WeaponData wd)
    {
        wdata = wd;
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = wdata.weaponsprite;
        sprite.color = wdata.weaponcolor;

    }

    //call Press and Unpress in the HandleInput() methods of AimStates for player
    //call elsewhere for enemies, depending on enemy AI
    public void Press()
    {
        if (!pressed && !wdata.isAutomatic)
        {
            Fire();
        }
        pressed = true;
    }
    public void Unpress()
    {
        if (pressed && wdata.chargeable)
        {
            Fire();
        }
        pressed = false;
    }


    //spawn bullet and reset timers
    public void Fire()
    {
        //If timer is less than firing delay, don't fire.
        if (shotTimer < wdata.firingDelay)
        {
            return;
        }

        GameObject fireme = bullet1; //default bullet
        
        //change bullet type if fully charged and reset charge timer
        if (wdata.chargeable)
        {
            if (chargeTimer > wdata.chargetime)
            {
                fireme = bullet2;
            }
            chargeTimer = 0;
        }

        //spawn bullet
        GameObject spawned = Instantiate(fireme);

        //position bullet
        spawned.transform.SetParent(this.transform);
        spawned.transform.localPosition = wdata.spriteOffset;
        spawned.transform.SetParent(null);

        BulletMovement bulletSm = spawned.GetComponent<BulletMovement>();
        bulletSm.SetInitialDirection(transform.parent.rotation.eulerAngles.z);
        bulletSm.StartBullet();
        bulletSm.Initialize(bulletSm.stdState); //might have to change this to StartState, depending on where we go.
        shotTimer = 0; //reset shot timer
    }

    
    //Sets reticle scale and position when locked on
    public void SetReticleTarget(GameObject t)
    {
        if (t == null)
        {
            Debug.Log("Setting null target");

            return;
        }

        //if the target changes, notify the current target
        if (reticle.transform.parent != null)
        {
            if (
                reticle.transform.parent.gameObject != this.gameObject &&
                reticle.transform.gameObject != t
                )
            {
                reticle.transform.parent.GetComponent<EnemyStateMachine>().UnregisterReticle();
            }
        }

        t.GetComponent<EnemyStateMachine>().RegisterReticle(reticle);
        reticle.transform.SetParent(t.transform);
        reticle.transform.localPosition = new Vector2(0,0);
        reticle.transform.localScale = new Vector3(reticleMaxScale, reticleMaxScale, 1);
        
        //Attempt to turn on glow animation on reticle
        GlowAnimation g = reticle.GetComponent<GlowAnimation>();
        if (g)
        {
            g.SetActive(true);
        }
    }
    public void ResetReticle()
    {
        if (reticle.transform.parent == gameObject.transform) { return; }
        reticle.transform.position = this.transform.position;
        reticle.transform.SetParent(this.transform);
        reticle.transform.localPosition = new Vector2(defaultReticleDistance,0);
        reticle.transform.localScale = new Vector3(reticleMinScale, reticleMinScale, 1);

        //Attempt to turn off glow animation on reticle
        GlowAnimation g = reticle.GetComponent<GlowAnimation>();
        if (g)
        {
            g.SetActive(false);
        }
    }

    protected void Update()
    {
        //advance shot timer
        if (shotTimer < wdata.firingDelay)
        {
            shotTimer += Time.deltaTime;
        }

        //advance charge timer if applicable
        if (pressed && wdata.chargeable)
        {
            if (chargeTimer < wdata.chargetime)
            {
                chargeTimer+= Time.deltaTime;
            }
        }
        //if unpressed, lose charge
        if (!pressed && wdata.chargeable)
        {
            chargeTimer = 0;
        }

        //fire gun if pressed and automatic
        if (pressed && wdata.isAutomatic)
        {
            Fire();
        }
    }
}
