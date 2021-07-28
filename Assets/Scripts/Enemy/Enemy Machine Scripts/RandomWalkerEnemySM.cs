using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWalkerEnemySM : EnemyStateMachine
{
    public override void CreateStates()
    {
        //Debug.Log("overridden create states");
        dormant = new EnemyDormant(this.gameObject, this);
        approach = new EnemyApproach(gameObject, this);
        attack = new EnemyAttack(gameObject, this);
        hitState = new EnemyHitState(gameObject, this);
        idle = new EnemyRunAroundRandomlyState(gameObject,this);

    }

}
