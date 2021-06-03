using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 
 * This state represents an offscreen, inactive enemy object.
 * It might be a good idea to include a reference to a level manager as a field for this class.
 * 
 */
public class EnemyDormant : EnemyState
{
    public EnemyDormant(GameObject t, GameStateMachine s) : base(t, s)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        ((EnemyStateMachine)sm).ActivateHitboxes(false);
    }
    public override void OnExit()
    {
        base.OnExit();
        ((EnemyStateMachine)sm).ActivateHitboxes(true);
    }
}
