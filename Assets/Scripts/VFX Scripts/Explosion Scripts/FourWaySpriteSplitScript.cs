using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This script animates an enemy splitting into four pieces and breaking apart (it's intended as a death animation). 
    
    It should contain references to four GameObjects, each of which is a copy of the same sprite texture, except that only 1/4 of the sprite is visible, thanks to a sprite mask.
    
    This script should move each of these four sprite masks/sprites while decreasing their alpha values.
 
 */

public class FourWaySpriteSplitScript : MonoBehaviour
{
    public bool debug;
    public float lifespan;
    public float speed;
    public SpriteRenderer[] quadrants; //enumerate the quadrants counterclockwise
    Transform t;
    float timer;
    Vector3 baseVector;
    bool started = false;
    

    //
    private void Awake()
    {
        t = gameObject.transform;
        baseVector = new Vector3(1, 1,0).normalized;
    }

    //Call after spawning this prefab using information from the EnemyStateMachine
    public void SetColorAndStart(EnemyData e)
    {
        foreach (SpriteRenderer s in quadrants)
        {
            s.sprite = e.baseSprite;
            s.color = e.element.primary;
        }
        timer = lifespan;
        started = true;
    }


    private void Update()
    {
         if (debug)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                timer = lifespan;
                started = true;
                foreach (SpriteRenderer q in quadrants)
                {
                    q.gameObject.transform.parent.transform.localPosition = new Vector3();
                }
            }
         
        }
         if (started)
        {
            //Update transforms
            quadrants[0].gameObject.transform.parent.position += speed * Time.deltaTime * baseVector;
            quadrants[1].gameObject.transform.parent.position += speed * Time.deltaTime * new Vector3(baseVector.x,-baseVector.y,0);
            quadrants[2].gameObject.transform.parent.position += -speed * Time.deltaTime * baseVector;
            quadrants[3].gameObject.transform.parent.position += speed * Time.deltaTime * new Vector3(-baseVector.x, baseVector.y, 0);



            //Update timer
            timer -= Time.deltaTime;
            
            
            if (timer <= 0)
            {  if (!debug)
                {
                    Destroy(gameObject);
                }
                else
                {
                    foreach(SpriteRenderer q in quadrants)
                    {
                        q.gameObject.transform.parent.transform.localPosition = new Vector3();
                    }
                }
            }
        }
    }


}
