using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneOverlayScrollCenterToRightState : SceneOverlayState
{
    float maxScrollTime;
    float timer = 0;

    public SceneOverlayScrollCenterToRightState(GameObject t, GameStateMachine s) : base(t, s)
    {
        maxScrollTime = ((SceneOverlayMessageUIStateMachine)sm).scrollMaxtime;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        timer += Time.deltaTime;
      
    }

    public override void OnEnter()
    {
        base.OnEnter();
        rt().localPosition = new Vector3(0, 0); //sets position to center of Canvas
        timer = 0;
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        float tempWidth = Mathf.Lerp(0,50 +size().x / 2.0f, Mathf.Min(timer, maxScrollTime) / maxScrollTime);
        timer += Time.deltaTime;
        rt().localPosition = new Vector3(tempWidth, 0, 0);
        if (timer >= maxScrollTime)
        {
            ((SceneOverlayMessageUIStateMachine)sm).ChangeState((GameState)(((SceneOverlayMessageUIStateMachine)sm).inactiveState)); //change to next state 

        }
    }
}
