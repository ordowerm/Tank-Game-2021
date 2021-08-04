using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public enum WeaponName
    {
        WEAPON0 , WEAPON1, WEAPON2
    }
    public WeaponData[] wdata;
    public GameObject reticle;
    public GameObject reticleOutline; //with certain colors, it was hard to see the reticle. Initially I tried making a shader to generate this outline, but it was giving me unexpected behavior. Instead, I've just made a game object that displays a second, slightly bigger, white reticle right under the main reticle. Hacky? Yes. Functional? Also yes.
    public GameObject bullet1;
    public GameObject bullet2;
    public float defaultReticleDistance; //distance from gun for reticle to spawn when in gamepad mode
    float shotTimer;
    float chargeTimer;
    bool pressed;
    bool useGamepad;
    int weaponId=0;

    //have the size of the reticle change when locked on
    public float reticleMinScale; //size when not locked on
    public float reticleMaxScale; //size when locked on
    //public float reticleOutlineThickness; 

    //Reference to level manager. Pass this to bullets so that score can update appropriately
    public int sourcePlayerId = 0;
    public LevelManager mgmt;



    protected void Awake()
    {
        shotTimer = 0;
        chargeTimer = 0;
        pressed = false;
        UpdateWeapon(WeaponName.WEAPON0);
        
    }



    //Call in UpdateWeapon
    protected void UpdateWeaponAndReticleSprites()
    {
        /*
        Debug.Log("Weapon number before: "+weaponId);
        Debug.Log("Primary color before: " + wdata[weaponId].bullettype.element.primary);
        Debug.Log("Outline color before: " + wdata[weaponId].bullettype.element.outlineColor);
        */

        //Update weapon sprite
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = wdata[weaponId].weaponsprite;
        sprite.color = wdata[weaponId].bullettype.element.primary;

        //Update outline sprite
        SpriteRenderer outlineSprite = reticleOutline.GetComponent<SpriteRenderer>();
        outlineSprite.sprite = wdata[weaponId].bullettype.element.reticleOutlineSprite; //update sprite for outline, too.
        outlineSprite.color = wdata[weaponId].bullettype.element.outlineColor; //update sprite for outline, too.

        //Update reticle sprite
        SpriteRenderer retSprite = reticle.GetComponent<SpriteRenderer>();
        retSprite.sprite = wdata[weaponId].bullettype.element.reticleSprite;
        retSprite.color = wdata[weaponId].bullettype.element.primary;

        /*
        Debug.Log("Weapon number after: " + weaponId);
        Debug.Log("Reticle color after: " + sprite.color);
        Debug.Log("Outline color after: " + outlineSprite.color);
        */
    }
    //Unused (as of 7/4) function that modifies shader parameters for the reticle, as opposed to modifying its sprite renderer directly.
    //Since I stopped using the OutlineShader material (it was giving unexpected behavior), this isn't currently used.
    protected void UpdateReticleShaders()
    {
        SpriteRenderer retSprite = reticle.GetComponent<SpriteRenderer>();
        Material retMat = retSprite.material;
        retMat.SetColor("_Color", wdata[weaponId].bullettype.element.primary);
    }

    //Call this when changing weapons
    public void UpdateWeapon(WeaponName wn)
    {
        //Set weapon
        switch (wn)
        {
            case WeaponName.WEAPON0:
            default:
                weaponId = 0;
                break;
            case WeaponName.WEAPON1:
                weaponId = 1;
                break;
            case WeaponName.WEAPON2:
                weaponId = 2;
                break;
        }
        
        //Update weapon sprites
        UpdateWeaponAndReticleSprites();
        //UpdateReticleShaders();


        /*
        //Attempt to turn on glow animation on reticle
        GlowAnimation g = reticle.GetComponent<GlowAnimation>();
        if (g)
        {
            g.c0 = wdata[weaponId].bullettype.element.primary;
        }
        */
    }
    public BulletData GetBulletData() { return wdata[weaponId].bullettype; }

    //call Press and Unpress in the HandleInput() methods of AimStates for player
    //call elsewhere for enemies, depending on enemy AI
    public void Press()
    {
        if (!pressed && !wdata[weaponId].isAutomatic)
        {
            Fire();
        }
        pressed = true;
    }
    public void Unpress()
    {
        if (pressed && wdata[weaponId].chargeable)
        {
            Fire();
        }
        pressed = false;
    }

    //spawn bullet and reset timers
    public void Fire()
    {
        //If timer is less than firing delay, don't fire.
        if (shotTimer < wdata[weaponId].firingDelay)
        {
            return;
        }

        GameObject fireme = bullet1; //default bullet
        
        //change bullet type if fully charged and reset charge timer
        if (wdata[weaponId].chargeable)
        {
            if (chargeTimer > wdata[weaponId].chargetime)
            {
                fireme = bullet2;
            }
            chargeTimer = 0;
        }

        //spawn bullet
        GameObject spawned = Instantiate(fireme);

        //position bullet
        spawned.transform.SetParent(this.transform);
        spawned.transform.localPosition = wdata[weaponId].spriteOffset;
        spawned.transform.SetParent(null);

        BulletSM bulletSm = spawned.GetComponent<BulletSM>();
        bulletSm.SetBulletData(wdata[weaponId].bullettype);
        bulletSm.SetInitialDirection(transform.parent.rotation.eulerAngles.z);
        bulletSm.StartBullet();
        bulletSm.Initialize(bulletSm.stdState); //might have to change this to StartState, depending on where we go.
        bulletSm.sourcePlayerId = sourcePlayerId;
        bulletSm.levelManager = mgmt;
        shotTimer = 0; //reset shot timer
    }

    
    //Sets reticle scale and position when locked on
    public void SetReticleTarget(GameObject t)
    {
        if (t == null)
        {
            Debug.Log("Setting null _target");
            ResetReticle();
            return;
        }

        //if the _target changes, notify the current _target
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
        reticle.transform.localScale = new Vector3(reticleMaxScale/t.transform.localScale.x, Mathf.Sign(transform.localScale.y)*reticleMaxScale/t.transform.localScale.y, 1);
        
        //Attempt to turn on glow animation on reticle
        GlowAnimation g = reticle.GetComponent<GlowAnimation>();
        if (g)
        {
            g.SetActive(true);
        }
    }
    //When canceling lockon, or when the enemy currently targeted gets destroyed, reset reticle to non-locked-on state
    public void ResetReticle()
    {
        if (reticle.transform.parent == null ||
            reticle.transform.parent != gameObject.transform
            )
        {
            reticle.transform.position = this.transform.position;
            reticle.transform.SetParent(this.transform);
            reticle.transform.localPosition = new Vector2(defaultReticleDistance, 0);
            reticle.transform.localScale = new Vector3(reticleMinScale, reticleMinScale, 1);
        }       
       
    }


    protected void Update()
    {
        //advance shot timer
        if (shotTimer < wdata[weaponId].firingDelay)
        {
            shotTimer += Time.deltaTime;
        }

        //advance charge timer if applicable
        if (pressed && wdata[weaponId].chargeable)
        {
            if (chargeTimer < wdata[weaponId].chargetime)
            {
                chargeTimer+= Time.deltaTime;
            }
        }
        //if unpressed, lose charge
        if (!pressed && wdata[weaponId].chargeable)
        {
            chargeTimer = 0;
        }

        //fire gun if pressed and automatic
        if (pressed && wdata[weaponId].isAutomatic)
        {
            Fire();
        }

        //Set local rotation of reticle to 0
        reticle.transform.eulerAngles = new Vector3(0, 0, 0);
        if (reticle.transform.lossyScale.y < 0)
        {
            reticle.transform.localScale = new Vector3(reticle.transform.localScale.x, -1 * reticle.transform.localScale.y, reticle.transform.localScale.z);
        }
    }
}
