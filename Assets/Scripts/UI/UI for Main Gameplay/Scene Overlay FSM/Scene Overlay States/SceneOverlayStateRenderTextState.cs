using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneOverlayStateRenderTextState : SceneOverlayState
{
    public SceneOverlayStateRenderTextState(GameObject t, GameStateMachine s) : base(t, s)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //Debug.Log("Render state: entering");
        rt().localPosition = new Vector2(0, 0); //moves to center of Canvas
        
        //Start displaying message
        if (((SceneOverlayMessageUIStateMachine)_sm).messageQueue.Count > 0)
        {
            SendOverlayMessage();
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
