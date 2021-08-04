using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 This state should move the textbox offscreen, reset the displayed text, and clear the message queue
 */
public class SceneOverlayInactiveState : SceneOverlayState
{
    public SceneOverlayInactiveState(GameObject t, GameStateMachine s) : base(t, s)
    {
        //Debug.Log("Inactive state spawned");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void OnEnter()
    {
        //Debug.Log("SceneOverlay: inactive: onEnter");
        base.OnEnter();
        rt().localPosition = new Vector3(-size().x / 2.0f - 50, 0); //assumes anchors and pivots = 0.5, 0.5, 0.5, 0.5, along with reference resolution = 100x100
        ((SceneOverlayMessageUIStateMachine)_sm).textDisplayer.text.text = "";
        while (((SceneOverlayMessageUIStateMachine)_sm).messageQueue.Count > 0) //clear message queue
        {
            ((SceneOverlayMessageUIStateMachine)_sm).messageQueue.Dequeue();
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
