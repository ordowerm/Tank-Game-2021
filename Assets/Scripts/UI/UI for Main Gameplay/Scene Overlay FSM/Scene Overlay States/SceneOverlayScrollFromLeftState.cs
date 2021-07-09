using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneOverlayScrollFromLeftState : SceneOverlayState
{
    float maxScrollTime;
    float timer;

    public SceneOverlayScrollFromLeftState(GameObject t, GameStateMachine s) : base(t, s)
    {
        maxScrollTime = ((SceneOverlayMessageUIStateMachine)sm).scrollMaxtime;
        timer = 0;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        timer += Time.deltaTime;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        timer = 0;
        rt().localPosition = new Vector3(-size().x / 2.0f - 50, 0); //assumes anchors and pivots = 0.5, 0.5, 0.5, 0.5, along with reference resolution = 100x100; moves left of camera
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        float tempWidth = Mathf.Lerp(-50 - size().x / 2.0f, 0, Mathf.Min(timer, maxScrollTime) / maxScrollTime);
        timer += Time.deltaTime;
        rt().localPosition = new Vector3(tempWidth, 0, 0);
        if (timer >= maxScrollTime)
        {
            ((SceneOverlayMessageUIStateMachine)sm).ChangeState((GameState)(((SceneOverlayMessageUIStateMachine)sm).renderTextState)); //change to next state 

        }
    }
}
