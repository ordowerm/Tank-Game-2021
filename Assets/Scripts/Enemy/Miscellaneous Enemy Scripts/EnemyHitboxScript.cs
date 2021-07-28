using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitboxScript : MonoBehaviour
{
    public EnemyStateMachine sm;
    public Collider2D col;
    public Element element;



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            //Debug.Log("I got hitted");
            BulletSM bsm = collision.gameObject.GetComponent<BulletSM>();
            if (bsm)
            {
                bsm.SetHitboxActive(false); //remove bullet hitbox
                Element bulletElement = bsm.bdata.element.element; //get the bullet's element

                if (bulletElement == element)
                {
                    sm.NotifyHit(bsm);
                    bsm.ChangeState(bsm.hitState);
                }
                else
                {
                    sm.NotifyResist();
                    Vector2 dir = -bsm.GetInitialDirection();
                    float reflectionAngle = Random.Range(-Mathf.PI/2.0f,Mathf.PI/2.0f);
                    Vector2 dotForX = new Vector2(Mathf.Cos(reflectionAngle), -Mathf.Sin(reflectionAngle));
                    Vector2 dotForY = new Vector2(Mathf.Sin(reflectionAngle), Mathf.Cos(reflectionAngle));
                    Vector2 rotated = new Vector2(Vector2.Dot(dir, dotForX), Vector2.Dot(dir, dotForY));
                    bsm.SetInitialDirection(rotated); //updates the direction of the bullet to a random ricochet direction
                    bsm.ChangeState(bsm.bounceState);

                }

            }
            
        }
    }

    public void Enable()
    {
        col.enabled = true;
    }

    public void Disable()
    {
        col.enabled = false;
    }

    protected void NotifyScoreUpdate(int playerId, int score)
    {
        sm.levelManager.IncrementPlayerScore(playerId, score);
    }

}
