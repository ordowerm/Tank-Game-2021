using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script controls the particle systems associated with the explosion animation 
 */
public class EnemyExplosionScript : MonoBehaviour
{
    public bool debug;
    public EnemyData edata;
    public ParticleSystem[] psystems;
    public FourWaySpriteSplitScript spriteScript;
    public float lifespan;
    float timer;

    //Call this from the EnemySM's destroy state to set the appropriate colors for everything
    public void SetColorAndStart(EnemyData e)
    {
        spriteScript.SetColorAndStart(e);
        foreach (ParticleSystem p in psystems)
        {
            if (p.textureSheetAnimation.spriteCount <= 1)
            {
                //p.textureSheetAnimation.AddSprite(e.element.explosionParticleSprite);
                p.textureSheetAnimation.SetSprite(0, e.element.explosionParticleSprite);
            }
            var col = p.colorOverLifetime;
            col.enabled = true;


            Gradient grad = new Gradient();
            grad.SetKeys(new GradientColorKey[] { 
                new GradientColorKey(e.element.secondary, 0.0f), 
                new GradientColorKey(e.element.primary, 0.7f) 
            }, new GradientAlphaKey[] { 
                new GradientAlphaKey(0.5f, 0.0f), 
                new GradientAlphaKey(0.5f, 0.7f),
                new GradientAlphaKey(0.0f,1.0f)
            });

            col.color = grad;
            //var trail = p.trails.colorOverLifetime;
            //trail.gradient = grad;
            p.Play();
        }
    }


    // Start is called before the first frame update
    void Awake()
    {
        timer = lifespan;
    }

    // Update is called once per frame
    void Update()
    {
        if (debug)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                foreach (ParticleSystem p in psystems)
                {
                    SetColorAndStart(edata);
                }
            }
        }

        timer -= Time.deltaTime;
        if (timer <= 0 && !debug) { Destroy(gameObject); }
    }
}
