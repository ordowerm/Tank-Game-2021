using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 
 
 
 */
public class EnemyDormantMoveHorizontal : EnemyDormant
{
    bool offscreen = true;

    public EnemyDormantMoveHorizontal(GameObject t, GameStateMachine s) : base(t, s)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

    }


    //Periodically check if offscreen.
    IEnumerator CheckOffscreen()
    {
        while (!offscreen)
        {
            yield return new WaitForSeconds(0.5f);
        }
    }

}
